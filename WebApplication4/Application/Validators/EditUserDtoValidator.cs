using FluentValidation;
using NotesApp.Application.DTOs;

namespace NotesApp.Application.Validators
{
    public class EditUserDtoValidator : AbstractValidator<EditUserDto>
    {
        public EditUserDtoValidator()
        {
            RuleFor(user => user.Name)
                .NotNull().NotEmpty().WithMessage("Name is required.")
                .MaximumLength(60).WithMessage("Name must not exceed 60 characters.");

            RuleFor(user => user.AboutMe)
                .MaximumLength(250).WithMessage("About Me must not exceed 250 characters.");
        }
    }
}
