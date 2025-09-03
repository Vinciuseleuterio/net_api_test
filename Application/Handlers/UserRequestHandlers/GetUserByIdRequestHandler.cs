using Application.Features.UserRequests;
using Domain.Entities;
using MediatR;
using NotesApp.Domain.Interfaces;

namespace Application.Handlers.UserRequestHandlers;

public class GetUserByIdRequestHandler(IUserRepository repo) : IRequestHandler<GetUserByIdRequest, User>
{
    private readonly IUserRepository _repo = repo;

    public async Task<User> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        return await _repo
            .GetUserById(request.UserId);
    }
}
