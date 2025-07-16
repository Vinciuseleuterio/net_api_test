using NotesApp.Application.DTOs;
using NotesApp.Domain.Entities;

namespace NotesApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<User> GetUserById(long userId);
        Task<User> UpdateUser(EditUserDto editUserDto, long userId);
        Task DeleteUserAsync(long userId);
        Task<User> ExistingUser(long userId);
    }
}
