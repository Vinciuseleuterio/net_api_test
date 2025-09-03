using System.Text.Json.Serialization;
using Domain.Entities;
using MediatR;

namespace Application.Features.UserRequests;

public class UpdateUserRequest : IRequest<User>
{
    public required string Name {get; set;}
    public string AboutMe {get; set;} = "";
    
    [JsonIgnore]
    public long UserId {get; set;}
}
