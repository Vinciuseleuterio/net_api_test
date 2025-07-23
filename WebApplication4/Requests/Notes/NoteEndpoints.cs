using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;

namespace Presentation.Requests.Notes
{
    public static class NoteEndpoints
    {
        public static void MapNoteEndpoints(this IEndpointRouteBuilder app)
        {
            var note = app.MapGroup("/note")
                .WithTags("note");

            note.MapPost("/api/users/{userId}/notes", async (NoteDto noteDto, long userId, NoteService noteService) =>
            {
                var note = await noteService.CreatePersonalNote(noteDto, userId);
                return Results.Created($"/api/users/{userId}/notes/{note.Id}", NoteToDto(note));
            });

            note.MapPost("/api/users/{userId}/groups/{groupId}/notes", async (NoteDto noteDto, long userId, long groupId, NoteService noteService) =>
            {
                var note = await noteService.CreateGroupNote(noteDto, userId, groupId);
                return Results.Created($"/api/users/{userId}/groups/{groupId}/notes/{note.Id}", NoteToDto(note));
            });

            note.MapGet("/api/users/{userId}/notes/{noteId}", async (long userId, long noteId, NoteService noteService) =>
            {
                var note = await noteService.GetNoteById(userId, noteId);
                return Results.Ok(NoteToDto(note));
            });

            note.MapGet("/api/users/{userId}/notes", async (long userId, NoteService noteService) =>
            {
                var notes = await noteService.GetAllNotesFromUser(userId);
                return Results.Ok(notes.Select(n => NoteToDto(n)));
            });

            note.MapGet("/api/users/{userId}/groups/{groupId}/notes", async (long userId, long groupId, NoteService noteService) =>
            {
                var notes = await noteService.GetAllNotesFromGroup(userId, groupId);
                return Results.Ok(notes.Select(n => NoteToDto(n)));
            });

            note.MapPatch("/api/users/{userId}/notes/{noteId}", async (NoteDto noteDto, long userId, long noteId, NoteService noteService) =>
            {
                var note = await noteService.UpdateNote(noteDto, userId, noteId);
                return Results.Ok(NoteToDto(note));
            });

            note.MapDelete("/api/users/{userId}/notes/{noteId}", async (long userId, long noteId, NoteService noteService) =>
            {
                await noteService.DeleteNote(userId, noteId);
                return Results.Ok("Note was deleted");
            });
        }

        private static NoteDto NoteToDto(Note note) =>
            new NoteDto
            {
                Title = note.Title,
                Content = note.Content
            };
    }
}