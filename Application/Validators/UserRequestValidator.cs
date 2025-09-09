using Application.Features.UserRequests;
using FluentValidation;

namespace NotesApp.Application.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
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

    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(user => user.Name)
                .NotNull().NotEmpty().WithMessage("Name is required.")
                .MaximumLength(60).WithMessage("Name must not exceed 60 characters.");

            RuleFor(user => user.AboutMe)
                .MaximumLength(250).WithMessage("About Me must not exceed 250 characters.");
        }
    }
}
