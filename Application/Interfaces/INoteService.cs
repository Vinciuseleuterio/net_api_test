using Domain.Entities;
using NotesApp.Application.DTOs;

namespace Application.Interfaces
{
    public interface INoteService
    {
        Task<Note> CreateGroupNote(NoteDto noteDto, long userId, long groupId);
        Task<Note> CreatePersonalNote(NoteDto noteDto, long userId);
        Task DeleteNote(long userId, long noteId);
        Task<IEnumerable<Note>> GetAllNotesFromGroup(long userId, long groupId);
        Task<IEnumerable<Note>> GetAllNotesFromUser(long userId);
        Task<Note> GetNoteById(long userId, long noteId);
        Task<Note> UpdateNote(NoteDto noteDto, long userId, long noteId);
    }
}
