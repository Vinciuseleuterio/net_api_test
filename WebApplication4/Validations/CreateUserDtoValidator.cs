using FluentValidation;
using WebApplication4.Dto;

namespace NotesApp.Validations;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(user => user.Email)
            .NotNull().NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(user => user.Name)
            .NotNull().NotEmpty().WithMessage("Name is required.")
            .MaximumLength(60).WithMessage("Name must not exceed 60 characters.");

        RuleFor(user => user.AboutMe)
            .MaximumLength(250)
            .WithMessage("About Me must not exceed 250 characters.");
    }
}
