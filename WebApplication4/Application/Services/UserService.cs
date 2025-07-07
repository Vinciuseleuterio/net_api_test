using FluentValidation;
using NotesApp.Application.DTOs;
using NotesApp.Domain.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        private readonly IValidator<CreateUserDto> _createValidator;
        private readonly IValidator<EditUserDto> _editValidator;

        public UserService(IUserRepository repo,
            IValidator<CreateUserDto> createValidator,
            IValidator<EditUserDto> ediValidator)
        {
            _repo = repo;
            _createValidator = createValidator;
            _editValidator = ediValidator;
        }

        public async Task<User> AddUserAsync(CreateUserDto createUserDto)
        {

            var result = _createValidator.Validate(createUserDto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine("Property: " + error.PropertyName + "\nError was: " + error.ErrorMessage);
                }
            }

            User user = new User

            {
                Email = createUserDto.Email,
                Name = createUserDto.Name
            };

            if (!string.IsNullOrEmpty(createUserDto.AboutMe))
            {
                user.AboutMe = createUserDto.AboutMe;
            }   

            await _repo.AddUserAsync(user);

            return user;
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            var user = await _repo.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }

        public async Task<User> UpdateUserAsync(long userId, EditUserDto editUserDto)
        {
            if (editUserDto == null) throw new ArgumentNullException(nameof(editUserDto));

            var result =  _editValidator.Validate(editUserDto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine("Property: " + error.PropertyName + "\nError was: " + error.ErrorMessage);
                }
            }

            var user = _repo.UpdateUserAsync(userId, editUserDto);

            if (user == null)
            {
                Console.WriteLine("User not found.");
            }

            return await user;
        }

        public async Task DeleteUserAsync(long userId)
        {
            if (userId <= 0 ) throw new ArgumentException("Invalid user ID", nameof(userId));

            await _repo.DeleteUserAsync(userId);
        }

        public async Task<bool> UserExistsAsync(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));
            return await _repo.UserExistsAsync(userId);
        }
    }
}
