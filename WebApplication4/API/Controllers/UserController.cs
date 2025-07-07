using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Models;

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

            var user = await _service.AddUserAsync(createUserDto);

            return Created("", UserToDto(user));
        }


        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById(long userId)
        {
            var user = await _service.GetUserByIdAsync(userId);

            return Ok(UserToDto(user));
        }

        // For this method to work as intended, we need to implement JWT Authentication since we don`t know witch user is logged in
        // Only users from the same group should be able to see each other

        [HttpPatch("{userId}")]
        public async Task<ActionResult<EditUserDto>> EditUser(long userId, EditUserDto editUserDto)
        {
            var user = await _service.UpdateUserAsync(userId, editUserDto);

            return Ok(UserToDto(user));
        }

        // For this method to work as intended, we need to implement JWT Authentication since we don`t know witch user is logged in

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(long userId)
        {
            await _service.DeleteUserAsync(userId);

            return Ok("User Deleted");
        }

        // For this method to work as intended, we need to implement JWT Authentication since we don`t know witch user is logged in

        private static EditUserDto UserToDto(User user) =>
            new EditUserDto
            {
                Name = user.Name,
                AboutMe = user.AboutMe
            };

        // This DTO conversion should be on the service layer

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("UNIQUE") == true ||
                ex.InnerException?.Message.Contains("duplicar") == true)
            {
                return true;
            }

            return false;
        }

    }
}