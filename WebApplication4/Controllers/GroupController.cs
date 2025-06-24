using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Dto;
using NotesApp.Models;
using WebApplication4.Data;

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

            _context.Group.Add(group);
            await _context.SaveChangesAsync();

            return Created("", GroupToDto(group));
        }

        [HttpPut("{userId}/{groupId}")]

        public async Task<ActionResult> EditGroup(long userId, long groupId, GroupDto groupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userIsFromGroup = _context.GroupMembership
                .Where(u => u.GroupId == groupId && u.UserId == userId)
                .FirstAsync();

            if (userIsFromGroup == null)
            {
                return BadRequest();
            }

            var group = await _context.Group.FindAsync(groupId);

            if (group == null)

            {
                return NotFound();
            }

            group.Name = groupDto.Name;
            group.Description = groupDto.Description;

            _context.Group.Add(group);
            await _context.SaveChangesAsync();

            return Ok(GroupToDto(group));
        }

        [HttpDelete("{userId}/{groupId}")]
        public async Task<ActionResult> deleteGroupFromUser(long userId, long groupId)
        {
            var group = await _context.Group
                .Where(g => g.CreatorId == userId & g.Id == groupId)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                return NotFound();
            }

            _context.Group.Remove(group);
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
