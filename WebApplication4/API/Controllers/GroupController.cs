using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Application.DTOs;
using NotesApp.Application.Validators;
using NotesApp.Domain.Models;
using NotesApp.Infrastructure.Data;

namespace NotesApp.API.Controllers
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
        public async Task<ActionResult> createGroup(long userId, CreateGroupDto createGroupDto)
        {

            CreateGroupDtoValidator validator = new CreateGroupDtoValidator();
            var result = validator.Validate(createGroupDto);


            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest("Property: " + error.PropertyName + "\nError was: " + error.ErrorMessage);
                }
            }

            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            Group group = new Group
            {
                Name = createGroupDto.Name,
                CreatorId = userId,
                Description = createGroupDto.Description
            };

            _context.Group.Add(group);
            group.Created();

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
            EditGroupDtoValidator validator = new EditGroupDtoValidator();
            var result = validator.Validate(editGroupDto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest("Property: " + error.PropertyName + "\nError was: " + error.ErrorMessage);
                }
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

            _context.Group.Update(group);
            group.Updated();

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
            group.Updated();
            
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> getGroupsFromUser(long userId)
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
        private static CreateGroupDto GroupToDto(Group group) =>
            new CreateGroupDto
            {
                Description = group.Description,
                Name = group.Name,
            };
    }

}
