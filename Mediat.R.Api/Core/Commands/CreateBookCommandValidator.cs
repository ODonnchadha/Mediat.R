using FluentValidation;

namespace Mediat.R.Api.Core.Commands;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

        RuleFor(b => b.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
    }
}
