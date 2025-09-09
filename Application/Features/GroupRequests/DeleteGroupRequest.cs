using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.GroupRequests;

public class DeleteGroupRequest : INotification
{
    [JsonIgnore]
    public long UserId { get; set; }

    [JsonIgnore]
    public long GroupId { get; set; }
}
