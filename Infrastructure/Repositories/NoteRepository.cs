using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Interfaces;
using NotesApp.Infrastructure.Data;

namespace NotesApp.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationContext _context;

        public NoteRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Note> CreatePersonalNote(Note note, long userId)
        {
            if (await ExistingUser(userId) == null) throw new ArgumentException("User not found");

            var createdNote = _context.Note
                .Add(note);

            if (createdNote == null) throw new DbUpdateException("Error saving note in the database");

            await _context
                .SaveChangesAsync();

            return createdNote.Entity;
        }

        public async Task<Note> CreateGroupNote(Note note, long userId, long groupId)
        {
            var groupMembership = await _context.GroupMembership
                 .Where(gmp => gmp.UserId == userId & gmp.GroupId == groupId)
                 .FirstAsync();

            if (groupMembership == null) throw new ArgumentException("Group Membership not found");

            var createdNote = _context.Note
                .Add(note);

            if (createdNote == null) throw new DbUpdateException("Error saving note in the database");

            await _context
                .SaveChangesAsync();

            return createdNote.Entity;
        }

        public async Task<Note> GetNoteById(long userId, long noteId)
        {
            await ExistingUser(userId);

            var note = await _context.Note
                .FindAsync(noteId);

            if (note == null) throw new ArgumentException("Note dosn't exist");

            if (note.CreatorId != userId) throw new ArgumentException("Note doesn't belong to user");

            return note;
        }

        public async Task<IEnumerable<Note>> GetAllNotesFromUser(long userId)
        {
            await ExistingUser(userId);

            var notes = await _context.Note
                .Where(note => note.CreatorId == userId)
                .ToListAsync();

            if (notes.Count == 0) throw new ArgumentException("Note(s) doesn't exist");

            return notes;
        }

        public async Task<IEnumerable<Note>> GetAllNotesFromGroup(long userId, long groupId)
        {
            var groupMembership = _context.GroupMembership
                .Where(gmp => gmp.UserId == userId && gmp.GroupId == groupId)
                .ToListAsync();

            if (groupMembership == null) throw new ArgumentException("Group Membership not found");

            var notes = await _context.Note
                .Where(note => note.GroupId == groupId)
                .ToListAsync();

            if (notes == null) throw new ArgumentException("Note(s) doesn't exist");

            return notes;
        }

        public async Task<Note> UpdateNote(Note note, long userId, long noteId)
        {
            if (note.CreatorId != userId) throw new ArgumentException("Note doesn't belong to user");

            var createtNote = _context
                .Note
                .Update(note);

            if (createtNote == null) throw new DbUpdateException("Error saving in the database");

            await _context
                .SaveChangesAsync();

            return createtNote.Entity;
        }

        public async Task DeleteNote(long userId, long noteId)
        {
            var note = await ExistingNote(noteId);

            if (note == null) throw new ArgumentException("Note doesn't exist");

            if (note.CreatorId != userId) throw new ArgumentException("Note doesn't belong to user");

            note.SetIsDeleted();
            note.SetUpdatedAt();

            await _context
                .SaveChangesAsync();
        }

        public async Task<User> ExistingUser(long userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            var user = await _context.User
                .FindAsync(userId);

            if (user == null) throw new ArgumentException("User not found");

            return user;
        }

        public async Task<Note> ExistingNote(long noteId)
        {

            if (noteId <= 0) throw new ArgumentException("Invalid note ID", nameof(noteId));

            var note = await _context.Note
                .FindAsync(noteId);

            if (note == null) throw new ArgumentException("Note doesn't exist");

            return note;
        }
    }
}
