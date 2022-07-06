using Minimal.Api.Data;
using Minimal.Api.Models;
using Minimal.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("books", async (Book book, IBookService bookService) =>
{
    try
    {
        await bookService.CreateAsync(book);
        return Results.Created($"/books/{book.Isbn}", book);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.UseHttpsRedirection();

app.Run();