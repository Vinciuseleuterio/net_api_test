using System.Text.Json.Serialization;
using MediatR;
using NotesApp.Domain.Entities;

namespace Application.Features.GroupRequests;

public class GetGroupsFromUserRequest : IRequest<Group>
{
    [JsonIgnore]
    public long UserId { get; set; }
}
