using FluentValidation;
using WebApplication4.Dto;

namespace NotesApp.Validations
{
    public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
    {
        public CreateNoteDtoValidator()
        {
            RuleFor(note => note.Title)
                .NotEmpty().NotNull().WithMessage("Title is required.")
                .MaximumLength(60).WithMessage("Title must not exceed 60 characters.");
        }
    }
}
