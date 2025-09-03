using Application.Features.UserRequests;
using MediatR;
using NotesApp.Domain.Interfaces;

namespace Application.Handlers.UserRequestHandlers
{
    public class DeleteUserRequestHandler(IUserRepository repo) : INotificationHandler<DeleteUserRequest>
    {
        private readonly IUserRepository _repo = repo;

        public async Task Handle(DeleteUserRequest notification, CancellationToken cancellationToken)
        {
            if (notification.UserId <= 0) throw new ArgumentException("Invalid user ID", nameof(notification.UserId));

            var user = await _repo
                .ExistingUser(notification.UserId);

            await _repo
                .DeleteUserAsync(user);

        }
    }
}
