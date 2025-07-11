﻿using FluentValidation;
using NotesApp.Dto;

namespace NotesApp.Validations
{
    public class EditUserDtoValidator : AbstractValidator<EditUserDto>
    {
        public EditUserDtoValidator()
        {
            RuleFor(user => user.Name)
                .MaximumLength(60)
                .WithMessage("Name must not exceed 60 characters.");

            RuleFor(user => user.AboutMe)
                .MaximumLength(250)
                .WithMessage("About Me must not exceed 250 characters.");
        }
    }
}
