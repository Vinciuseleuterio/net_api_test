using Application.Features.GroupRequests;
using FluentValidation;
using MediatR;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace Application.Handlers.GroupRequestHandlers;

public class UpdateGroupRequestHandler(IGroupRepository repo,
        IValidator<UpdateGroupRequest> validator,
        Group.GroupBuilder builder) : IRequestHandler<UpdateGroupRequest, Group>
{
    private readonly IGroupRepository _repo = repo;
    private readonly IValidator<UpdateGroupRequest> _validator = validator;
    private readonly Group.GroupBuilder _builder = builder;

    public async Task<Group> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
    {
        var result = _validator
            .Validate(request);

        if (!result.IsValid) throw new ValidationException(result.Errors);

        await _repo
            .ExistingUser(request.UserId);

        var group = await _repo
            .ExistingGroup(request.GroupId);

        var updatedGroup = _builder
            .SetName(request.Name)
            .SetDescription(request.Description)
            .SetCreatorId(request.UserId)
            .Update(group);

        updatedGroup.SetUpdatedAt();

        return await _repo
            .UpdateGroup(group, request.UserId, request.GroupId);

    }
}
