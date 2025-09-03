using System.Text.Json.Serialization;
using Domain.Entities;
using MediatR;

namespace Application.Features.UserRequests;

public class GetUserByIdRequest : IRequest<User>
{
    [JsonIgnore]
    public required long UserId { get; set; }
}
