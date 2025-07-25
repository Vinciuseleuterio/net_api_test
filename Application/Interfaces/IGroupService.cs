using NotesApp.Application.DTOs;
using NotesApp.Domain.Entities;

namespace Application.Interfaces
{
    public interface IGroupService
    {
        Task<Group> CreateGroup(GroupDto groupDto, long userId);
        Task DeleteGroup(long userId, long groupId);
        Task<Group> GetGroupById(long userId, long groupId);
        Task<IEnumerable<Group>> GetGroupsFromUser(long userId);
        Task<Group> UpdateGroup(GroupDto groupDto, long userId, long groupId);
    }
}