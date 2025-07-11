using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Models;
using FluentValidation;

namespace NotesApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(
            UserService userService)
        {
            _service = userService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                var user = await _service
                    .CreateUser(createUserDto);

                return Created("User: " + user.Id + " was created with success", UserToDto(user));
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
        public async Task<ActionResult> GetUserById(long userId)
        {
            try
            {
                var user = await _service
                    .GetUserById(userId);

                return Ok(UserToDto(user));
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

        [HttpPatch("{userId}")]
        public async Task<ActionResult> UpdateUser(EditUserDto editUserDto, long userId)
        {

            try
            {
                var user = await _service
                    .UpdateUser(editUserDto, userId);

                return Ok(UserToDto(user));
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

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(long userId)
        {
            try
            {
                await _service
                    .DeleteUserAsync(userId);

                return Ok("User Deleted");
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

        private static EditUserDto UserToDto(User user) =>
            new EditUserDto
            {
                Name = user.Name,
                AboutMe = user.AboutMe
            };
    }
}