using Application.Interfaces;
using FluentValidation;
using NotesApp.Application.DTOs;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Application.Services
{
    public class UserService : IUserService
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

        public async Task<User> CreateUser(CreateUserDto createUserDto)
        {

            var result = _createValidator
                .Validate(createUserDto);

            if (!result.IsValid) throw new ValidationException(result.Errors);

            User user = new User

            {
                Email = createUserDto.Email,
                Name = createUserDto.Name
            };

            if (!string.IsNullOrEmpty(createUserDto.AboutMe))
            {
                user.AboutMe = createUserDto.AboutMe;
            }

            user.SetCreatedAt();

            return await _repo
                .CreateUser(user);
        }

        public async Task<User> GetUserById(long userId)
        {
            return await _repo
                .GetUserById(userId);
        }

        public async Task<User> UpdateUser(EditUserDto editUserDto, long userId)
        {
            var result = _editValidator
                .Validate(editUserDto);

            if (!result.IsValid) throw new ValidationException(result.Errors);

            var user = await _repo
                .ExistingUser(userId);

            user.Name = editUserDto.Name;
            user.AboutMe = editUserDto.AboutMe;

            user.SetUpdatedAt();

            return await _repo
                .UpdateUser(user, userId);
        }

        public async Task DeleteUserAsync(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            await _repo
                .DeleteUserAsync(userId);
        }
    }
}
