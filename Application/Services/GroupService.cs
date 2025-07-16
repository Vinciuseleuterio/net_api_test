using FluentValidation;
using Microsoft.EntityFrameworkCore.Update.Internal;
using NotesApp.Application.DTOs;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Infrastructure.Repositories;
using System.Security.Cryptography.X509Certificates;

namespace NotesApp.Application.Services
{
    public class GroupService
    {
        private readonly IGroupRepository _repo;
        private readonly IValidator<GroupDto> _groupValidator;

        public GroupService(IGroupRepository repo,
            IValidator<GroupDto> groupValidator)
        {
            _repo = repo;
            _groupValidator = groupValidator;
        }

        public async Task<Group> CreateGroup(GroupDto groupDto, long userId)
        {
            var result = _groupValidator
                .Validate(groupDto);

            if (!result.IsValid) throw new ValidationException(result.Errors);


            Group group = new Group
            {
                Name = groupDto.Name,
                Description = groupDto.Description,
                CreatorId = userId
            };

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

            return await _repo
                .UpdateGroup(groupDto, userId, groupId);
        }

        public async Task DeleteGroup(long userId, long groupId)
        {
            await _repo
                .DeleteGroup(userId, groupId);
        }
    }
}
