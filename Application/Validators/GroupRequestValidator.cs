using Application.Features.UserRequests.GroupRequests;
using FluentValidation;

namespace Application.Validators
{
    public class GroupDtoValidator : AbstractValidator<CreateGroupRequest>
    {
        public GroupDtoValidator()
        {
            RuleFor(group => group.Name)
                .NotEmpty().NotNull().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(group => group.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
        }

    }
}
