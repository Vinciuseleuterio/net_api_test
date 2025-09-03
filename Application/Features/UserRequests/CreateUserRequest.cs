using Domain.Entities;
using MediatR;

namespace Application.Features.UserRequests;

public class CreateUserRequest : IRequest<User>
{
    public required string Email { get; init; }
    public required string Name { get; set; }
    public string AboutMe { get; set; } = "";
}
