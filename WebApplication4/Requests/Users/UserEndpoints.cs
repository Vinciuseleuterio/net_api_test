using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;

namespace Presentation.Requests.Users
{
    public static class UserEndpoints
    {
        //public static RouteGroupBuilder MapGroupWithTag(this WebApplication app, string prefix, string tagName, Action<RouteGroupBuilder> configure)
        //{
        //    var group = app.MapGroup(prefix).WithTags(tagName);
        //    configure(group);
        //    return group;
        //}
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var user = app.MapGroup("/user")
                .WithTags("user");

            user.MapPost("/api/users", async (CreateUserDto createUserDto, UserService userService) =>
            {
                var user = await userService.CreateUser(createUserDto);
                return Results.Created($"/api/users/{user.Id}", UserToDto(user));
            });

            user.MapGet("/api/users/{userId}", async (long userId, UserService userService) =>
            {
                var user = await userService.GetUserById(userId);
                return Results.Ok(UserToDto(user));
            });

            user.MapPatch("/api/users/{userId}", async (long userId, EditUserDto editUserDto, UserService userService) =>
            {
                var user = await userService.UpdateUser(editUserDto, userId);
                return Results.Ok(UserToDto(user));
            });

            user.MapDelete("/api/users/{userId}", async (long userId, UserService userService) =>
            {
                await userService.DeleteUserAsync(userId);
                return Results.Ok("User Deleted");
            });
        }

        private static EditUserDto UserToDto(User user) =>
            new EditUserDto
            {
                Name = user.Name,
                AboutMe = user.AboutMe
            };
    }
}