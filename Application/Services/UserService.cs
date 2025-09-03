using Application.Features.UserRequests;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IValidator<CreateUserRequest> _createValidator;
        private readonly IValidator<UpdateUserRequest> _editValidator;
        private readonly User.UserBuilder _userBuilder;

        public UserService(IUserRepository repo,
            IValidator<CreateUserRequest> createValidator,
            IValidator<UpdateUserRequest> ediValidator,
            User.UserBuilder userBuilder)
        {
            _repo = repo;
            _createValidator = createValidator;
            _editValidator = ediValidator;
            _userBuilder = userBuilder;
        }

        public async Task<User> CreateUser(CreateUserRequest request)
        {

            var result = _createValidator
                .Validate(request);

            if (!result.IsValid) throw new ValidationException(result.Errors);

            // Cria um novo objeto de usuário

            var user = _userBuilder
                .SetName(request.Name)
                .SetEmail(request.Email)
                .SetAboutMe(request.AboutMe)
                .Build();

            //user.SetCreatedAt();

            return await _repo
                .CreateUser(user);
        }

        public async Task<User> GetUserById(long userId)
        {
            return await _repo
                .GetUserById(userId);
        }

        public async Task<User> UpdateUser(UpdateUserRequest request, long userId)
        {
            var result = _editValidator
                .Validate(request);

            if (!result.IsValid) throw new ValidationException(result.Errors);

            var user = await _repo
            .ExistingUser(userId);

            var updatedUser = _userBuilder
                .SetName(request.Name)
                .SetAboutMe(request.AboutMe)
                .Update(user);

            // updatedUser.SetUpdatedAt();

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
