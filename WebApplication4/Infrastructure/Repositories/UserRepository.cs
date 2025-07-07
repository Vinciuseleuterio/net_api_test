using Microsoft.EntityFrameworkCore;
using NotesApp.Application.DTOs;
using NotesApp.Domain.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.Infrastructure.Data;

namespace NotesApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(long userId)
        {
            return await _context.User.FindAsync(userId);
        }

        public async Task AddUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            _context.User.Add(user);
            user.Created();

            await _context.SaveChangesAsync();
        }

        public async Task<User?> UpdateUserAsync(long userId, EditUserDto editUserDto)
        {
            if (editUserDto == null) throw new ArgumentNullException(nameof(editUserDto));

            var existingUser = await _context.User
                .FindAsync(userId);

            if (existingUser == null)
            {
                Console.WriteLine("User not found");
            }

            existingUser.Name = editUserDto.Name;
            existingUser.AboutMe = editUserDto.AboutMe;

            _context.User.Update(existingUser);
            existingUser.Updated();

            await _context.SaveChangesAsync();

            return existingUser;
        }

        public async Task DeleteUserAsync(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            var user = await GetUserByIdAsync(userId);

            if (user == null) return; // User not found, nothing to delete

            user.Delete();
            user.Updated();

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));
            return await _context.User.AnyAsync(u => u.Id == userId);
        }
    }
}
