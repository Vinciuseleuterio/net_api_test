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
        private readonly User.UserBuilder _userBuilder;

        public UserService(IUserRepository repo,
            IValidator<CreateUserDto> createValidator,
            IValidator<EditUserDto> ediValidator,
            User.UserBuilder userBuilder)
        {
            _repo = repo;
            _createValidator = createValidator;
            _editValidator = ediValidator;
            _userBuilder = userBuilder;
        }

        public async Task<User> CreateUser(CreateUserDto userDto)
        {

            var result = _createValidator
                .Validate(userDto);

            if (!result.IsValid) throw new ValidationException(result.Errors);

            // Cria um novo objeto de usuário

            var user = _userBuilder
                .SetName(userDto.Name)
                .SetEmail(userDto.Email)
                .SetAboutMe(userDto.AboutMe)
                .Build();


            user.SetCreatedAt();

            return await _repo
                .CreateUser(user);
        }

        public async Task<User> GetUserById(long userId)
        {
            return await _repo
                .GetUserById(userId);
        }

        public async Task<User> UpdateUser(EditUserDto userDto, long userId)
        {
            var result = _editValidator
                .Validate(userDto);

            if (!result.IsValid) throw new ValidationException(result.Errors);

            var user = await _repo
                .ExistingUser(userId);

            var updatedUser = _userBuilder
                .SetName(userDto.Name)
                .SetAboutMe(userDto.AboutMe)
                .Update(user);

            updatedUser.SetUpdatedAt();

            return await _repo
                .UpdateUser(user, userId);
        }

        public async Task DeleteUserAsync(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            var user = await _repo
                .ExistingUser(userId);

            await _repo
                .DeleteUserAsync(user);
        }
    }
}
