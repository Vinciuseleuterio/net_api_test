using Application.Features.UserRequests;
using Domain.Entities;
using FluentValidation;
using MediatR;
using NotesApp.Domain.Interfaces;

namespace Application.Handlers.UserRequestHandlers;

public class UpdateUserRequestHandler(
    IUserRepository repo,
    IValidator<UpdateUserRequest> validator,
    User.UserBuilder builder) : IRequestHandler<UpdateUserRequest, User>
{
    private readonly IUserRepository _repo = repo;
    private readonly IValidator<UpdateUserRequest> _validator = validator;
    private readonly User.UserBuilder _builder = builder;

    public async Task<User> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = _validator
            .Validate(request);

        if (!result.IsValid) throw new ValidationException(result.Errors);

        var user = await _repo
        .ExistingUser(request.UserId);

        var updatedUser = _builder
            .SetName(request.Name)
            .SetAboutMe(request.AboutMe)
            .Update(user);

        updatedUser.SetUpdatedAt();

        return await _repo
            .UpdateUser(user, request.UserId);
    }

}
