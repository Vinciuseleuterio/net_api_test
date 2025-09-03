using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Interfaces;
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
            var createdUser = _context.User
                .Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Here you can log, wrap, or rethrow the exception
                throw new DbUpdateException("Error saving user in the database", ex);
            }

            return createdUser.Entity;
        }

        public async Task<User> GetUserById(long userId)
        {
            var user = await ExistingUser(userId);

            return user;
        }

        public async Task<User> UpdateUser(User user, long userId)
        {
            await ExistingUser(userId);

            if (_context.User.Update(user) == null) throw new DbUpdateException("Error updating user in the database");

            await _context
                .SaveChangesAsync();

            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            user.SetIsDeleted();
            user.SetUpdatedAt();

            ApplicationContext.
                CascadeSoftDelete(user);

            await _context
                .SaveChangesAsync();
        }

        public async Task<User> ExistingUser(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            var user = await _context.User
                .Include(u => u.Notes)
                .FirstAsync(u => u.Id == userId);

            return user ?? throw new ArgumentException("User not found");
        }
    }

}
