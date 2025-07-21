using Microsoft.AspNetCore.Mvc;
using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;

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
            var user = await _service
                .CreateUser(createUserDto);

            return Created("User: " + user.Id + " was created with success", UserToDto(user));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById(long userId)
        {
            var user = await _service
                .GetUserById(userId);

            return Ok(UserToDto(user));
        }

        [HttpPatch("{userId}")]
        public async Task<ActionResult> UpdateUser(EditUserDto editUserDto, long userId)
        {
            var user = await _service
                .UpdateUser(editUserDto, userId);

            return Ok(UserToDto(user));
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(long userId)
        {
            await _service
                .DeleteUserAsync(userId);

            return Ok("User Deleted");
        }

        private static EditUserDto UserToDto(User user) =>
            new EditUserDto
            {
                Name = user.Name,
                AboutMe = user.AboutMe
            };
    }
}