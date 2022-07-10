using Minimal.Api.Data;
using Minimal.Api.Models;
using Minimal.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Minimal.Api.Auth;

var builder = WebApplication.CreateBuilder(args);

var domain = builder.Configuration["Auth0:Domain"];
var audience = builder.Configuration["Auth0:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{domain}/";
    options.Audience = audience;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
    options.AddPolicy("write:messages", policy => policy.Requirements.Add(new HasScopeRequirement("write:messages", domain)));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddSingleton<IDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("books", async (IBookService bookService, string? searchTerm) =>
{
    if (searchTerm is not null && !string.IsNullOrWhiteSpace(searchTerm))
    {
        var matchedBooks = await bookService.SearchByTitleAsync(searchTerm);
        return Results.Ok(matchedBooks);
    }

    var books = await bookService.GetAllAsync();
    return Results.Ok(books);
})
    .WithName("GetBooks")
    .WithTags("Books")
    .RequireAuthorization("read:messages")
    .Produces<IEnumerable<BookDisplay>>(200)
    .Produces(404);

app.MapGet("books/{isbn}", async (string isbn, IBookService bookService) =>
{
    var book = await bookService.GetByIsbnAsync(isbn);

    if (book is null)
    {
        return Results.BadRequest(Results.NotFound(value: "This book was not found."));
    }

    var bookWithoutId = new BookDisplay
    {
        Isbn = book.Isbn,
        Title = book.Title,
        Author = book.Author,
        ShortDescription = book.ShortDescription,
        PageCount = book.PageCount,
        ReleaseDate = book.ReleaseDate
    };

    return Results.Ok(bookWithoutId);
})
    .WithName("GetBook")
    .WithTags("Books")
    .Produces<BookDisplay>(200)
    .Produces(404);

app.MapPost("books", async (Book book, IBookService bookService, IValidator<Book> validator) =>
{
    var validationResult = await validator.ValidateAsync(book);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    bool created = await bookService.CreateAsync(book);
    if (!created)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    return Results.Created($"/books/{book.Isbn}", book);
})
    .WithName("CreateBook")
    .WithTags("Books")
    .RequireAuthorization("write:messages")
    .Accepts<Book>("application/json")
    .Produces<BookDisplay>(201)
    .Produces(400)
    .Produces(404);

app.MapPut("books/{isbn}", async (string isbn, Book book, IBookService bookService, IValidator<Book> validator) =>
{
    book.Isbn = isbn;

    var validationResult = await validator.ValidateAsync(book);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var updated = await bookService.UpdateAsync(book);
    return updated ? Results.Ok(book) : Results.NotFound();
})
    .WithName("UpdateBook")
    .WithTags("Books")
    .RequireAuthorization("write:messages")
    .Accepts<Book>("application/json")
    .Produces<BookDisplay>(200)
    .Produces(400);

app.MapDelete("books/{isbn}", async (string isbn, IBookService bookService) =>
{
    var deleted = await bookService.DeleteAsync(isbn);
    return deleted ? Results.NoContent() : Results.NotFound();
})
    .WithName("DeleteBook")
    .WithTags("Books")
    .RequireAuthorization("write:messages")
    .Produces(204)
    .Produces(404);

app.UseHttpsRedirection();

app.Run();