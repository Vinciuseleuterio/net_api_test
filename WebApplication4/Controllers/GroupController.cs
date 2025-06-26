using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Dto;
using NotesApp.Dtos;
using NotesApp.Models;
using WebApplication4.Data;
using WebApplication4.Models;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly ApplicationContext _context;

        public GroupController(ApplicationContext applicationContext)
        {
            _context = applicationContext;
        }


        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<GroupDto>>> getGroupsFromUser(long userId)
        {
            var group = await _context.Group
                .Where(g => g.CreatorId == userId)
                .ToListAsync();

            if (group.Count == 0)
            {
                return NotFound();
            }

            return Ok(group.Select(g => GroupToDto(g)));
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> createGroup(long userId, GroupDto groupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("");
            }

            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            Group group = new Group
            {
                Name = groupDto.Name,
                Description = groupDto.Description,
                CreatorId = userId
            };

            group.Created();
            _context.Group.Add(group);

            await _context.SaveChangesAsync();

            var groupId = group.Id;

            GroupMembership groupMembership = new GroupMembership
            {
                UserId = userId,
                GroupId = groupId
            };

            groupMembership.Created();
            _context.GroupMembership.Add(groupMembership);

            await _context.SaveChangesAsync();

            return Created("", GroupToDto(group));
        }

        [HttpPatch("{userId}/{groupId}")]
        public async Task<ActionResult> EditGroup(long userId, long groupId, EditGroupDto editGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var groupMembership = await _context.GroupMembership
                .Where(u => u.GroupId == groupId && u.UserId == userId)
                .FirstOrDefaultAsync();

            if (groupMembership == null)
            {
                return NotFound();
            }

            var group = await _context.Group.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(editGroupDto.Name))
            {
                group.Name = editGroupDto.Name;
            }

            if (!string.IsNullOrEmpty(editGroupDto.Description))
            {
                group.Description = editGroupDto.Description;
            }

            group.Updated();
            _context.Group.Update(group);
            await _context.SaveChangesAsync();

            return Ok(GroupToDto(group));
        }

        [HttpDelete("{userId}/{groupId}")]
        public async Task<ActionResult> deleteGroupFromUser(long userId, long groupId)
        {
            var group = await _context.Group
                .Where(g => g.Id == groupId)
                .FirstOrDefaultAsync();

            var groupMembership = await _context.GroupMembership
                .Where(gm => gm.GroupId == groupId & gm.UserId == userId)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                return NotFound();
            }

            if (groupMembership == null)
            {
                return NotFound("Group membership not found.");
            }

            group.Delete();
            groupMembership.Delete();
            await _context.SaveChangesAsync();
            return Ok(GroupToDto(group));
        }
        private static GroupDto GroupToDto(Group group) =>
            new GroupDto
            {
                Description = group.Description,
                Name = group.Name,
            };
    }

}
