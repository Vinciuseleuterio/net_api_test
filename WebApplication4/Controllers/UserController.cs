using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Dto;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Realiza a injeção de dependência da "ApplicationContext" 
        private readonly ApplicationContext _context;

        public UserController(ApplicationContext applicationContext)
        {
            _context = applicationContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return await _context.User
                .Select(x => UserToDto(x))
                .ToListAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUserById(long? userId)
        {
            if (userId == null)
            {
                return BadRequest("UserId cannot be empty or null");
            }

            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return UserToDto(user);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<UserDto>> EditUser(long userId, UserDto userDto)
        {
            var existingUser = await _context.User.FindAsync(userId);

            if (existingUser == null)
            {
                return NotFound("User doesn't exists");
            }

            User user = new User();

            if (userDto.Name != null)
            {
                user.Name = userDto.Name;
            }

            if (userDto.AboutMe != null)
            {
                user.AboutMe = userDto.AboutMe;
            }

            await _context.SaveChangesAsync();

            return Created("The following user was edited with success", UserToDto(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto? userDto)
        {
            if (userDto?.Email == "" | userDto?.Name == "")
            {
                return BadRequest("Email or Name cannot be empty");
            }

            if (_context.User.Any(user => user.Email == userDto.Email))
            {
                return BadRequest("Email is already taken");
            }

            var user = new User();

            user.Name = userDto.Name;
            user.AboutMe = userDto.AboutMe;
            user.Email = userDto.Email;

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Created("The following user was created with success", UserToDto(user));
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<UserDto>> DeleteUser(long userId)
        {
            var user = await _context.User.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User was deleted");
        }

        private static UserDto UserToDto(User user) =>
            new UserDto
            {
                Name = user.Name,
                AboutMe = user.AboutMe,
                Email = user.Email
            };
    }
}