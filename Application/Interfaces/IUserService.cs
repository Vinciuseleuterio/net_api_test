using Application.Features.UserRequests;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserRequest request);
        Task DeleteUserAsync(long userId);
        Task<User> GetUserById(long userId);
        Task<User> UpdateUser(UpdateUserRequest request, long userId);
    }
}
