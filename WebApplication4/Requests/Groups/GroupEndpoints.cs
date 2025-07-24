using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;

namespace Presentation.Requests.Groups
{
    public static class GroupEndpoints
    {
        public static void MapGroupEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api")
                .WithTags("Group");

            group.MapPost("/users/{userId}/groups", async (long userId, GroupDto createGroupDto, GroupService groupService) =>
            {
                var group = await groupService.CreateGroup(createGroupDto, userId);
                return Results.Ok(GroupToDto(group));
            });

            group.MapGet("/users/{userId}/groups/{groupId}", async (long userId, long groupId, GroupService groupService) =>
            {
                var group = await groupService.GetGroupById(userId, groupId);
                return Results.Ok(GroupToDto(group));
            });

            group.MapPatch("/users/{userId}/groups/{groupId}", async (long userId, long groupId, GroupDto groupDto, GroupService groupService) =>
            {
                var group = await groupService.UpdateGroup(groupDto, userId, groupId);
                return Results.Ok(GroupToDto(group));
            });

            group.MapDelete("/users/{userId}/groups/{groupId}", async (long userId, long groupId, GroupService groupService) =>
            {
                await groupService.DeleteGroup(userId, groupId);
                return Results.Ok();
            });
        }

        private static GroupDto GroupToDto(Group group) =>
            new GroupDto
            {
                Description = group.Description,
                Name = group.Name,
            };
    }
}