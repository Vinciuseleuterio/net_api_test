using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;
using NotesApp.Infrastructure.Data;

namespace NotesApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly ApplicationContext _context;
        private readonly GroupService _service;

        public GroupController(ApplicationContext applicationContext,
            GroupService service)
        {
            _context = applicationContext;
            _service = service;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> createGroup(long userId, GroupDto createGroupDto)
        {
            try
            {
                await _service.CreateGroup(createGroupDto, userId);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Errors });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error saving in the database: " + ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetGroupsFromUser(long userId)
        {
            try

            {
                var groups = await _service
                    .GetGroupsFromUser(userId);

                return Ok(groups.Select(g => GroupToDto(g)));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpGet("{userId}/{groupId}")]

        public async Task<ActionResult> GetGroupById(long userId, long groupId)
        {
            try
            {
                var group = await _service
                    .GetGroupById(userId, groupId);

                return Ok(GroupToDto(group));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpPatch("{userId}/{groupId}")]
        public async Task<ActionResult> UpdateGroup(GroupDto groupDto, long userId, long groupId)
        {

            try
            {
                var group = await _service
                     .UpdateGroup(groupDto, userId, groupId);

                return Ok(GroupToDto(group));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Errors });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error saving in the database: " + ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
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
