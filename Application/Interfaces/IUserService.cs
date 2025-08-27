using NotesApp.Application.DTOs;
using NotesApp.Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserDto createUserDto);
        Task DeleteUserAsync(long userId);
        Task<User> GetUserById(long userId);
        Task<User> UpdateUser(EditUserDto editUserDto, long userId);
    }
}
