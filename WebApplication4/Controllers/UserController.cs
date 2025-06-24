using Microsoft.AspNetCore.Mvc;
using NotesApp.Dto;
using WebApplication4.Data;
using WebApplication4.Dto;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public UserController(ApplicationContext applicationContext)
        {
            _context = applicationContext;
        }

        [HttpPost]
        public async Task<ActionResult<CreateUserDto>> CreateUser(CreateUserDto CreateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.User.Any(u => u.Email == CreateUserDto.Email))
            {
                return Conflict("Email already exists.");
            }

            var user = new User
            {
                Name = CreateUserDto.Name,
                Email = CreateUserDto.Email
            };

            if (!string.IsNullOrEmpty(CreateUserDto.AboutMe))
            {
                user.AboutMe = CreateUserDto.AboutMe;
            }

            _context.User.Add(user);
            user.Created();
            await _context.SaveChangesAsync();

            return Created("", UserToDto(user));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<CreateUserDto>> GetUserById(long? userId)
        {
            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(UserToDto(user));
        }

        [HttpPatch("{userId}")]
        public async Task<ActionResult<CreateUserDto>> EditUser(long userId, EditUserDto editUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!string.IsNullOrEmpty(editUserDto.Name))
            {
                user.Name = editUserDto.Name;

            }

            user.AboutMe = editUserDto.AboutMe;

            user.Updated();
            await _context.SaveChangesAsync();

            return Ok(UserToDto(user));
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<CreateUserDto>> DeleteUser(long userId)
        {
            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }



            user.Delete();
            await _context.SaveChangesAsync();

            return Ok();
        }

        private static CreateUserDto UserToDto(User user) =>
            new CreateUserDto
            {
                Name = user.Name,
                Email = user.Email,
                AboutMe = user.AboutMe
            };
    }
}