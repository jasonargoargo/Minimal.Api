using FluentValidation;
using Minimal.Api.Models;

namespace Minimal.Api.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Isbn)
                .Matches(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$")
                .WithMessage("Value is not a valid ISBN-13.");
            RuleFor(book => book.Title).NotEmpty();
            RuleFor(book => book.ShortDescription).NotEmpty();
            RuleFor(book => book.PageCount).NotEmpty().GreaterThan(0);
            RuleFor(book => book.Author).NotEmpty();
        }
    }
}
