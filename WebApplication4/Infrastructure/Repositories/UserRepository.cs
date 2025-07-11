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

        public async Task<User> CreateUser(User user)
        {
            _context.User
                .Add(user);

            await _context
                .SaveChangesAsync();

            var createdUser = await ExistingUser(user.Id);

            if (createdUser == null)
            {
                throw new DbUpdateException("Error saving user: " + user.Id + " in the database");
            }

            return createdUser;
        }

        public async Task<User> GetUserById(long userId)
        {
            var user = await ExistingUser(userId);

            return user;
        }

        public async Task<User> UpdateUser(EditUserDto editUserDto, long userId)
        {
            var user = await ExistingUser(userId);

            user.Name = editUserDto.Name;
            user.AboutMe = editUserDto.AboutMe;

            user.Updated();

            if (_context.User.Update(user) == null)
            {
                throw new DbUpdateException("Error saving user: " + user.Id + " in the database");
            }

            await _context
                .SaveChangesAsync();

            return user;
        }

        public async Task DeleteUserAsync(long userId)
        {
            var user = await ExistingUser(userId);

            user.Delete();
            user.Updated();

            await _context
                .SaveChangesAsync();
        }

        public async Task<User> ExistingUser(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            var user = await _context.User
                .FindAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            return user;
        }
    }
}
