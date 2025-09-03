using Application.Features.UserRequests;
using Domain.Entities;
using FluentValidation;
using MediatR;
using NotesApp.Domain.Interfaces;

namespace Application.Handlers.UserRequestHandlers
{
    public class CreateUserRequestHandler(
        IUserRepository repo,
        IValidator<CreateUserRequest> validator,
        User.UserBuilder builder) : IRequestHandler<CreateUserRequest, User>
    {
        private readonly IUserRepository _repo = repo;
        private readonly IValidator<CreateUserRequest> _validator = validator;
        private readonly User.UserBuilder _builder = builder;

        public async Task<User> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var result = _validator
                .Validate(request);

            if (!result.IsValid) throw new ValidationException(result.Errors);

            // Cria um novo objeto de usuário

            var user = _builder
                .SetName(request.Name)
                .SetEmail(request.Email)
                .SetAboutMe(request.AboutMe)
                .Build();

            user.SetCreatedAt();

            return await _repo
                .CreateUser(user);
        }
    }

}
