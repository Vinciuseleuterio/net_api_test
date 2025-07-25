using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;

namespace Presentation.Requests.Notes
{
    public static class NoteEndpoints
    {
        public static IEndpointRouteBuilder MapNoteEndpoints(this IEndpointRouteBuilder app)
        {
            var note = app.MapGroup("/api")
                .WithTags("Note");

            note.MapPost("/users/{userId}/notes", async (NoteDto noteDto, long userId, NoteService noteService) =>
            {
                var note = await noteService.CreatePersonalNote(noteDto, userId);
                return Results.Created($"/users/{userId}/notes/{note.Id}", NoteToDto(note));
            });

            note.MapPost("/users/{userId}/groups/{groupId}/notes", async (NoteDto noteDto, long userId, long groupId, NoteService noteService) =>
            {
                var note = await noteService.CreateGroupNote(noteDto, userId, groupId);
                return Results.Created($"/users/{userId}/groups/{groupId}/notes/{note.Id}", NoteToDto(note));
            });

            note.MapGet("/users/{userId}/notes/{noteId}", async (long userId, long noteId, NoteService noteService) =>
            {
                var note = await noteService.GetNoteById(userId, noteId);
                return Results.Ok(NoteToDto(note));
            });

            note.MapGet("/users/{userId}/notes", async (long userId, NoteService noteService) =>
            {
                var notes = await noteService.GetAllNotesFromUser(userId);
                return Results.Ok(notes.Select(n => NoteToDto(n)));
            });

            note.MapGet("/users/{userId}/groups/{groupId}/notes", async (long userId, long groupId, NoteService noteService) =>
            {
                var notes = await noteService.GetAllNotesFromGroup(userId, groupId);
                return Results.Ok(notes.Select(n => NoteToDto(n)));
            });

            note.MapPatch("/users/{userId}/notes/{noteId}", async (NoteDto noteDto, long userId, long noteId, NoteService noteService) =>
            {
                var note = await noteService.UpdateNote(noteDto, userId, noteId);
                return Results.Ok(NoteToDto(note));
            });

            note.MapDelete("/users/{userId}/notes/{noteId}", async (long userId, long noteId, NoteService noteService) =>
            {
                await noteService.DeleteNote(userId, noteId);
                return Results.Ok("Note was deleted");
            });

            return app;
        }

        private static NoteDto NoteToDto(Note note) =>
            new NoteDto
            {
                Title = note.Title,
                Content = note.Content
            };
    }
}