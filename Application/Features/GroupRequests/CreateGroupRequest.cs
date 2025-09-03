using System.Text.Json.Serialization;
using MediatR;
using NotesApp.Domain.Entities;

namespace Application.Features.UserRequests.GroupRequests;

public class CreateGroupRequest : IRequest<Group>
{
    public required string Name { get; set; }
    public string Description { get; set; } = "";

    [JsonIgnore]
    public long UserId { get; set; }
}
