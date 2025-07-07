using FluentValidation;
using NotesApp.Application.DTOs;

namespace NotesApp.Application.Validators
{
    public class EditNoteDtoValidator : AbstractValidator<EditNoteDto>
    {
        public EditNoteDtoValidator()
        {
            RuleFor(note => note.Title)
                .MaximumLength(60).WithMessage("Title must not exceed 60 characters.");

            RuleFor(note => note.Content)
                .MaximumLength(1000).WithMessage("Content must not exceed 1000 characters.");
        }
    }
}
