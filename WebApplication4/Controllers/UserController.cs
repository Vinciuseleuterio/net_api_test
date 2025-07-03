using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Dtos;
using NotesApp.Models;
using NotesApp.Validations;

namespace NotesApp.Controllers
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
        public async Task<ActionResult<CreateUserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email
            };

            CreateUserDtoValidator validator = new CreateUserDtoValidator();
            var result = validator.Validate(createUserDto);


            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest("Property: " + error.PropertyName + "\nError was: " + error.ErrorMessage);
                }
            }

            // Pass the error message this way, so we can see the property that caused the error

            if (!string.IsNullOrEmpty(createUserDto.AboutMe))
            {
                user.AboutMe = createUserDto.AboutMe;
            }

            _context.User.Add(user);
            user.Created();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                return Conflict("Email already exists.");
            }

            return Created("", UserToDto(user));
        }

        // Method "createUser" was validated with success


        [HttpGet("{userId}")]
        public async Task<ActionResult<CreateUserDto>> GetUserById(long userId)
        {
            var user = await _context.User
                .FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(UserToDto(user));
        }

        // For this method to work as intended, we need to implement JWT Authentication since we don`t know witch user is logged in
        // Only users from the same group should be able to see each other

        [HttpPatch("{userId}")]
        public async Task<ActionResult<EditUserDto>> EditUser(long userId, EditUserDto editUserDto)
        {


            EditUserDtoValidator validator = new EditUserDtoValidator();
            var result = validator.Validate(editUserDto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest("Property: " + error.PropertyName + "\nError was: " + error.ErrorMessage);
                }
            }

            var user = await _context.User
                .FindAsync(userId);

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

        // For this method to work as intended, we need to implement JWT Authentication since we don`t know witch user is logged in

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(long userId)
        {
            var user = await _context.User
                .FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.Delete();
            user.Updated();
            await _context.SaveChangesAsync();

            return Ok();
        }

        // For this method to work as intended, we need to implement JWT Authentication since we don`t know witch user is logged in

        private static EditUserDto UserToDto(User user) =>
            new EditUserDto
            {
                Name = user.Name,
                AboutMe = user.AboutMe
            };

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