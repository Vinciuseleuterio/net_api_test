using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.UserRequests;
 
public class DeleteUserRequest : INotification
{
    [JsonIgnore]
    public long UserId { get; set; }
}
