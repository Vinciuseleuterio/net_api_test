using Domain.Entities;

namespace NotesApp.Domain.Interfaces
{
    public interface INoteRepository
    {
        public Task<Note> CreatePersonalNote(Note note, long userId);
        public Task<Note> CreateGroupNote(Note note, long noteId, long groupId);
        public Task<Note> GetNoteById(long userId, long noteId);
        public Task<IEnumerable<Note>> GetAllNotesFromUser(long userId);
        public Task<IEnumerable<Note>> GetAllNotesFromGroup(long userId, long groupId);
        public Task<Note> UpdateNote(Note note, long userId, long noteId);
        public Task DeleteNote(long userId, long noteId);
        public Task<Note> ExistingNote(long noteId);
        public Task<User> ExistingUser(long userId);
    }
}
