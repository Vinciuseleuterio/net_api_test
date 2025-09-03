using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using NotesApp.Application.DTOs;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repo;
        private readonly IValidator<NoteDto> _noteDtoValidator;
        private readonly Note.NoteBuilder _noteBuilder;

        public NoteService(INoteRepository repo,
            IValidator<NoteDto> noteDtoValidator,
            Note.NoteBuilder noteBuilder)
        {
            _repo = repo;
            _noteDtoValidator = noteDtoValidator;
            _noteBuilder = noteBuilder;
        }

        public async Task<Note> CreatePersonalNote(NoteDto noteDto, long userId)
        {
            var result = _noteDtoValidator
                .Validate(noteDto);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var note = _noteBuilder
                .SetTitle(noteDto.Title)
                .SetContent(noteDto.Content)
                .SetCreatorId(userId)
                .Build();

            note.SetCreatedAt();

            return await _repo
                .CreatePersonalNote(note, userId);
        }

        public async Task<Note> CreateGroupNote(NoteDto noteDto, long userId, long groupId)
        {
            var result = _noteDtoValidator
                .Validate(noteDto);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var note = _noteBuilder
                 .SetCreatorId(userId)
                 .SetGroupId(groupId)
                 .SetTitle(noteDto.Title)
                 .SetContent(noteDto.Content)
                 .Build();

            note.SetCreatedAt();

            return await _repo
                .CreateGroupNote(note, userId, groupId);
        }

        public async Task<Note> GetNoteById(long userId, long noteId)
        {
            if(userId <= 0) throw new InvalidCastException("user id must be set");

            if(noteId <= 0) throw new InvalidCastException("note id must be set");

            return await _repo
                .GetNoteById(userId, noteId);
        }


        public async Task<IEnumerable<Note>> GetAllNotesFromUser(long userId)
        {
            return await _repo
                .GetAllNotesFromUser(userId);
        }

        public async Task<IEnumerable<Note>> GetAllNotesFromGroup(long userId, long groupId)
        {
            return await _repo
                 .GetAllNotesFromGroup(userId, groupId);
        }

        public async Task<Note> UpdateNote(NoteDto noteDto, long userId, long noteId)
        {
            var result = _noteDtoValidator
                .Validate(noteDto);

            if(!result.IsValid) throw new ValidationException(result.Errors); 

            var note = await _repo
                .ExistingNote(noteId);

            var updatedNote = _noteBuilder
                .SetTitle(noteDto.Title)
                .SetContent(noteDto.Content)
                .Update(note);

            updatedNote.SetUpdatedAt();

            return await _repo
                .UpdateNote(note, userId, noteId);
        }

        public async Task DeleteNote(long userId, long noteId)
        {
            if (userId <= 0) throw new InvalidCastException("userId must be set");
            if (noteId <= 0) throw new InvalidCastException("noteId must be set");

            await _repo
                .DeleteNote(userId, noteId);
        }
    }
}
