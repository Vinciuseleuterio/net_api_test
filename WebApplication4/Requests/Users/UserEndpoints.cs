using Application.Features.UserRequests;
using Domain.Entities;
using MediatR;
using NotesApp.Application.DTOs;

namespace Presentation.Requests.Users
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var user = app.MapGroup("/api")
                .WithTags("User");

            user.MapPost("/users", async (IMediator mediator, CreateUserRequest request) =>
            {
                var user = await mediator.Send(request);
                return Results.Created($"/users", UserToDto(user));
            });

            user.MapGet("/users/{userId}", async (IMediator mediator, long userId) =>
            {
                var request = new GetUserByIdRequest { UserId = userId };
                var user = await mediator.Send(request);
                return Results.Ok(UserToDto(user));
            });

            user.MapPatch("/users{userId}", async (IMediator mediator, UpdateUserRequest request, long userId) =>
            {
                request.UserId = userId;
                var user = await mediator.Send(request);
                return Results.Ok(UserToDto(user));
            });

            user.MapDelete("/users/{userId}", async (IMediator mediator, long userId) =>
            {
                var request = new DeleteUserRequest { UserId = userId };
                await mediator.Publish(request);
                return Results.Ok("User Deleted");
            });

            return app;
        }

        private static EditUserDto UserToDto(User user) =>
            new()
            {
                Name = user.Name,
                AboutMe = user.AboutMe
            };
    }
}
