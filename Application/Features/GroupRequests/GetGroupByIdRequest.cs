using System.Text.Json.Serialization;
using MediatR;
using NotesApp.Domain.Entities;

namespace Application.Features.GroupRequests;

public class GetGroupByIdRequest : IRequest<Group>
{
    [JsonIgnore]
    public long UserId { get; set; }

    [JsonIgnore]
    public long GroupId { get; set; }
}
