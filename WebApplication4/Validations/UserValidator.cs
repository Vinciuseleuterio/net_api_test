using FluentValidation;
using WebApplication4.Models;

namespace NotesApp.Validations;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Email).NotEmpty();
    }
}
