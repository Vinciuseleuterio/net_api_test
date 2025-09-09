using Application.Features.GroupRequests;
using MediatR;
using NotesApp.Domain.Interfaces;

namespace Application.Handlers.GroupRequestHandlers;

public class DeleteGroupRequestHandler(IGroupRepository repo) : INotificationHandler<DeleteGroupRequest>
{
    private readonly IGroupRepository _repo = repo;

    public async Task Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0 || request.GroupId <= 0)
        {
            throw new ArgumentException("User id or GroupId should be greater then 0");
        }

        await _repo
            .DeleteGroup(request.UserId, request.GroupId);

    }

}
