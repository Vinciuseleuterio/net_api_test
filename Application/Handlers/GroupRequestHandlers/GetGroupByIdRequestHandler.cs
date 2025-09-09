using Application.Features.GroupRequests;
using MediatR;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace Application.Handlers.GroupRequestHandlers;

public class GetGroupByIdRequestHandler(IGroupRepository repo) : IRequestHandler<GetGroupByIdRequest, Group>
{
    private readonly IGroupRepository _repo = repo;

    public async Task<Group> Handle(GetGroupByIdRequest request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0 || request.GroupId <= 0)
        {
            throw new ArgumentException("User id or Group id should be greater then 0");
        }
        return await _repo
            .GetGroupById(request.UserId, request.GroupId);

    }
}
