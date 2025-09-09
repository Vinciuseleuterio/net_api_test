using Application.Features.GroupRequests;
using Application.Features.UserRequests.GroupRequests;
using FluentValidation;

namespace Application.Validators
{
    public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>
    {
        public CreateGroupRequestValidator()
        {
            RuleFor(group => group.Name)
                .NotEmpty().NotNull().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(group => group.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");

            RuleFor(group => group.UserId)
                .NotEmpty().NotNull().WithMessage("UserId must not be null or empty");
        }

    }

    public class UpdateGroupRequestValidator : AbstractValidator<UpdateGroupRequest>
    {
        public UpdateGroupRequestValidator()
        {
            RuleFor(group => group.Name)
                .NotEmpty().NotNull().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(group => group.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");

            RuleFor(group => group.UserId)
                .NotEmpty().NotNull().WithMessage("UserId must not be null or empty");

            RuleFor(group => group.GroupId)
                .NotEmpty().NotNull().WithMessage("GroupId must not be null or empty");

        }

    }
}
