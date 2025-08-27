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
            if (userId <= 0 || groupId <= 0)
            {
                throw new ArgumentException("User id or Group id should be greater then 0");
            }
            return await _repo
                .GetGroupById(userId, groupId);
        }

        public async Task<IEnumerable<Group>> GetGroupsFromUser(long userId)
        {
            
            if (userId <= 0)
            {
                throw new ArgumentException("User id should be greater then 0");
            }

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

            var updatedGroup = _groupBuilder
                .SetName(groupDto.Name)
                .SetDescription(groupDto.Description)
                .SetCreatorId(userId)
                .Update(group);

            updatedGroup.SetUpdatedAt();

            return await _repo
                .UpdateGroup(group, userId, groupId);
        }

        public async Task DeleteGroup(long userId, long groupId)
        {
            if (userId <= 0 || groupId <= 0)
            {
                throw new ArgumentException("User id or GroupId should be greater then 0");
            }

            await _repo
                .DeleteGroup(userId, groupId);
        }
    }
}
