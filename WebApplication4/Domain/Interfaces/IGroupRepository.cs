using NotesApp.Application.DTOs;
using NotesApp.Domain.Models;

namespace NotesApp.Domain.Interfaces
{
    public interface IGroupRepository
    {
        public Task<Group> CreateGroup(Group group, long userId);
        public Task<IEnumerable<Group>> GetGroupsFromUser(long userId);
        public Task<Group> GetGroupById(long userId, long groupId);
        public Task<Group> UpdateGroup(GroupDto groupDto, long userId, long groupId);
        public Task DeleteGroup(long userId, long groupId);
        public Task<User> ExistingUser(long userId);

    }
}
