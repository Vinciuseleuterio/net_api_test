using FluentValidation;
using NotesApp.Dtos;

namespace NotesApp.Validations
{
    public class EditGroupDtoValidator : AbstractValidator<EditGroupDto>
    {
        public EditGroupDtoValidator()
        {
            RuleFor(group => group.Name)
                .MaximumLength(60).WithMessage("Name must not exceed 60 characters.");

            RuleFor(group => group.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
