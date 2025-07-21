using Microsoft.AspNetCore.Mvc;
using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;

namespace NotesApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly GroupService _service;

        public GroupController(GroupService service)
        {
            _service = service;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> CreateGroup(long userId, GroupDto createGroupDto)
        {
            var group = await _service
                .CreateGroup(createGroupDto, userId);

            return Ok(GroupToDto(group));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetGroupsFromUser(long userId)
        {

            var groups = await _service
                .GetGroupsFromUser(userId);

            return Ok(groups.Select(g => GroupToDto(g)));
        }

        [HttpGet("{userId}/{groupId}")]

        public async Task<ActionResult> GetGroupById(long userId, long groupId)
        {

            var group = await _service
                .GetGroupById(userId, groupId);

            return Ok(GroupToDto(group));
        }

        [HttpPatch("{userId}/{groupId}")]
        public async Task<ActionResult> UpdateGroup(GroupDto groupDto, long userId, long groupId)
        {

            var group = await _service
                 .UpdateGroup(groupDto, userId, groupId);

            return Ok(GroupToDto(group));
        }

        [HttpDelete("{userId}/{groupId}")]
        public async Task<ActionResult> DeleteGroup(long userId, long groupId)
        {
            await _service
                .DeleteGroup(userId, groupId);

            return Ok();
        }

        private static GroupDto GroupToDto(Group group) =>
            new GroupDto
            {
                Description = group.Description,
                Name = group.Name,
            };
    }

}
