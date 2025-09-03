using Domain.Entities;

namespace NotesApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<User> GetUserById(long userId);
        Task<User> UpdateUser(User user, long userId);
        Task DeleteUserAsync(User user);
        Task<User> ExistingUser(long userId);
    }
}
