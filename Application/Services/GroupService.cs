using Application.Interfaces;
using FluentValidation;
using NotesApp.Application.DTOs;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repo;
        private readonly IValidator<GroupDto> _groupValidator;
        private readonly Group.GroupBuilder _groupBuilder;

        public GroupService(IGroupRepository repo,
            IValidator<GroupDto> groupValidator,
            Group.GroupBuilder groupBuilder)
        {
            _repo = repo;
            _groupValidator = groupValidator;
            _groupBuilder = groupBuilder;
        }

        public async Task<Group> CreateGroup(GroupDto groupDto, long userId)
        {
            var result = _groupValidator
                .Validate(groupDto);

            if (!result.IsValid) throw new ValidationException(result.Errors);


            var group = _groupBuilder
                .SetName(groupDto.Name)
                .SetDescription(groupDto.Description)
                .SetCreatorId(userId)
                .Build();

            await _repo
                .CreateGroup(group, userId);

            return group;
        }

        public async Task<Group> GetGroupById(long userId, long groupId)
        {
            return await _repo
                .GetGroupById(userId, groupId);
        }

        public async Task<IEnumerable<Group>> GetGroupsFromUser(long userId)
        {
            return await _repo
                .GetGroupsFromUser(userId);
        }

        public async Task<Group> UpdateGroup(GroupDto groupDto, long userId, long groupId)
        {
            var result = _groupValidator
                .Validate(groupDto);

            if (!result.IsValid) throw new ValidationException(result.Errors);

            var group = await _repo
                .ExistingGroup(groupId);

            group = _groupBuilder
                .SetName(groupDto.Name)
                .SetDescription(groupDto.Description)
                .SetCreatorId(userId)
                .Build();

            group.SetUpdatedAt();

            return await _repo
                .UpdateGroup(group, userId, groupId);
        }

        public async Task DeleteGroup(long userId, long groupId)
        {
            await _repo
                .DeleteGroup(userId, groupId);
        }
    }
}
