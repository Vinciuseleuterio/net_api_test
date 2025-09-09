using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Infrastructure.Data;

namespace NotesApp.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationContext _context;

        public GroupRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Group> CreateGroup(Group group, long userId)
        {
            await ExistingUser(userId);

            var createdGroup = _context.Group
                .Add(group);

            if (createdGroup == null) throw new DbUpdateException("Error saving note in the database");

            group.SetCreatedAt();

            var groupMembership = new GroupMembership
            {
                UserId = userId,
                GroupId = group.Id
            };

            group.GroupMemberships.Add(groupMembership);

            if (_context.GroupMembership.Add(groupMembership) == null) throw new DbUpdateException("Error saving Group Memberhsip on database");

            groupMembership.SetCreatedAt();

            await _context
                .SaveChangesAsync();

            return createdGroup.Entity;
        }

        public async Task<Group> GetGroupById(long userId, long groupId)
        {
            await ExistingUser(userId);

            var group = await _context.Group
                .FindAsync(groupId);

            if (group == null) throw new ArgumentException("Group not found");

            return group;
        }

        public async Task<IEnumerable<Group>> GetGroupsFromUser(long userId)
        {
            await ExistingUser(userId);

            var groups = await _context.Group
                .Where(g => g.CreatorId == userId)
                .ToListAsync();

            if (groups.Count == 0) throw new ArgumentException("Group(s) not found");

            return groups;
        }

        public async Task<Group> UpdateGroup(Group group, long userId, long groupId)
        {
            if (_context.Group.Update(group) == null) throw new DbUpdateException("Error saving Group in the database");

            await _context
                .SaveChangesAsync();

            return group;
        }

        public async Task DeleteGroup(long userId, long groupId)
        {
            await ExistingUser(userId);

            var group = await _context.Group
                .Include(g => g.Notes)
                .Include(g => g.GroupMemberships)
                .FirstAsync(g => g.Id == groupId);

            if (group == null) throw new ArgumentException("Group not found");

            group.SetIsDeleted();
            group.SetUpdatedAt();

            ApplicationContext
                .CascadeSoftDelete(group);

            await _context.SaveChangesAsync();
        }


        public async Task<User> ExistingUser(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            var user = await _context.User
                .FindAsync(userId);

            return user ?? throw new ArgumentException("User not found");
        }

        public async Task<Group> ExistingGroup(long groupId)
        {
            if (groupId <= 0) throw new ArgumentException("Invalid group ID", nameof(groupId));

            var group = await _context.Group
                .FindAsync(groupId) ?? throw new ArgumentException("Group not found");
            return group;
        }
    }
}
