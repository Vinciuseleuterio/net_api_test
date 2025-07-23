using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;

namespace Presentation.Requests.Groups
{
    public static class GroupEndpoints
    {
        public static void MapGroupEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/group")
                .WithTags("group");

            group.MapPost("/api/users/{userId}/groups", async (long userId, GroupDto createGroupDto, GroupService groupService) =>
            {
                var group = await groupService.CreateGroup(createGroupDto, userId);
                return Results.Ok(GroupToDto(group));
            });

            group.MapGet("/api/users/{userId}/groups", async (long userId, GroupService groupService) =>
            {
                var groups = await groupService.GetGroupsFromUser(userId);
                return Results.Ok(groups.Select(g => GroupToDto(g)));
            });

            group.MapGet("/api/users/{userId}/groups/{groupId}", async (long userId, long groupId, GroupService groupService) =>
            {
                var group = await groupService.GetGroupById(userId, groupId);
                return Results.Ok(GroupToDto(group));
            });

            group.MapPatch("/api/users/{userId}/groups/{groupId}", async (long userId, long groupId, GroupDto groupDto, GroupService groupService) =>
            {
                var group = await groupService.UpdateGroup(groupDto, userId, groupId);
                return Results.Ok(GroupToDto(group));
            });

            group.MapDelete("/api/users/{userId}/groups/{groupId}", async (long userId, long groupId, GroupService groupService) =>
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