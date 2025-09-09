using Application.Features.GroupRequests;
using Application.Features.UserRequests.GroupRequests;
using Application.Interfaces;
using MediatR;
using NotesApp.Application.DTOs;
using NotesApp.Domain.Entities;

namespace Presentation.Requests.Groups
{
    public static class GroupEndpoints
    {
        public static IEndpointRouteBuilder MapGroupEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api")
                .WithTags("Group");

            group.MapPost("/users/{userId}/groups", async (long userId, CreateGroupRequest request, IMediator mediator) =>
            {
                request.UserId = userId;
                var group = await mediator.Send(request);
                return Results.Ok(GroupToDto(group));
            });

            group.MapGet("/users/{userId}/groups/{groupId}", async (long userId, long groupId, IMediator mediator) =>
            {
                var request = new GetGroupByIdRequest { UserId = userId, GroupId = groupId };
                var group = await mediator.Send(request);
                return Results.Ok(GroupToDto(group));
            });

            group.MapPatch("/users/{userId}/groups/{groupId}", async (long userId, long groupId, IMediator mediator, UpdateGroupRequest request) =>
            {
                request.UserId = userId;
                request.GroupId = groupId;
                var group = await mediator.Send(request);
                return Results.Ok(GroupToDto(group));
            });

            group.MapDelete("/users/{userId}/groups/{groupId}", async (long userId, long groupId, IMediator mediator) =>
            {
                var request = new DeleteGroupRequest { UserId = userId, GroupId = groupId };
                await mediator.Publish(request);
                return Results.Ok();
            });

            return app;
        }

        private static GroupDto GroupToDto(Group group) =>
            new GroupDto
            {
                Description = group.Description,
                Name = group.Name,
            };
    }
}
