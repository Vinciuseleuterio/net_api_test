using Application.Features.UserRequests.GroupRequests;
using FluentValidation;
using MediatR;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace Application.Handlers.GroupRequestHandlers;

public class CreateGroupRequestHandler(
    IGroupRepository repo,
    Group.GroupBuilder builder,
    IValidator<CreateGroupRequest> validator) : IRequestHandler<CreateGroupRequest, Group>
{
    private readonly IGroupRepository _repo = repo;
    private readonly IValidator<CreateGroupRequest> _validator = validator;
    private readonly Group.GroupBuilder _groupBuilder = builder;

    public async Task<Group> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var result = _validator
            .Validate(request);

        if (!result.IsValid) throw new ValidationException(result.Errors);

        var group = _groupBuilder
            .SetName(request.Name)
            .SetDescription(request.Description)
            .SetCreatorId(request.UserId)
            .Build();

        await _repo
            .CreateGroup(group, request.UserId);

        return group;
    }
}
