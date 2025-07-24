using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;

namespace Presentation.Requests.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var user = app.MapGroup("/api")
                .WithTags("User");

            user.MapPost("/users", async (CreateUserDto createUserDto, UserService userService) =>
            {
                var user = await userService.CreateUser(createUserDto);
                return Results.Created($"/users/{user.Id}", UserToDto(user));
            });

            user.MapGet("/users/{userId}", async (long userId, UserService userService) =>
            {
                var user = await userService.GetUserById(userId);
                return Results.Ok(UserToDto(user));
            });

            user.MapPatch("/users/{userId}", async (long userId, EditUserDto editUserDto, UserService userService) =>
            {
                var user = await userService.UpdateUser(editUserDto, userId);
                return Results.Ok(UserToDto(user));
            });

            user.MapDelete("/users/{userId}", async (long userId, UserService userService) =>
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