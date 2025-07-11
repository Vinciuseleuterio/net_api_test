using FluentValidation;
using NotesApp.Application.DTOs;

namespace NotesApp.Application.Validators
{
    public class NoteDtoValidator : AbstractValidator<NoteDto>
    {
        public NoteDtoValidator()
        {
            RuleFor(note => note.Title)
                .NotEmpty().NotNull().WithMessage("Title is required.")
                .MaximumLength(60).WithMessage("Title must not exceed 60 characters.");

            RuleFor(note => note.Content)
                .MaximumLength(1000).WithMessage("Content must not exceed 1000 characters.");
        }
    }
}
