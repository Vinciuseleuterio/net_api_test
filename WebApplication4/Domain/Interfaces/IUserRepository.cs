using NotesApp.Application.DTOs;
using NotesApp.Domain.Models;

namespace NotesApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(long userId);
        Task AddUserAsync(User user);
        Task<User?> UpdateUserAsync(long userId, EditUserDto editUserDto);
        Task DeleteUserAsync(long userId);

        // I dont know if this is needed, but it is in the original code, maybe it is used for something
        Task<bool> UserExistsAsync(long userId);

    }
}
