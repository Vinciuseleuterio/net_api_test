using Application.Interfaces;
using FluentValidation;
using NotesApp.Application.DTOs;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repo;
        private readonly IValidator<NoteDto> _noteDtoValidator;

        public NoteService(INoteRepository repo,
            IValidator<NoteDto> noteDtoValidator)
        {
            _repo = repo;
            _noteDtoValidator = noteDtoValidator;
        }

        public async Task<Note> CreatePersonalNote(NoteDto noteDto, long userId)
        {
            var result = _noteDtoValidator
                .Validate(noteDto);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var note = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                CreatorId = userId
            };

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

            Note note = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                CreatorId = userId,
                GroupId = groupId
            };

            note.SetCreatedAt();

            return await _repo
                .CreateGroupNote(note, userId, groupId);
        }

        public async Task<Note> GetNoteById(long userId, long noteId)
        {
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
            var note = await _repo
                .ExistingNote(noteId);

            note.Title = noteDto.Title;
            note.Content = noteDto.Content;

            note.SetUpdatedAt();

            return await _repo
                .UpdateNote(note, userId, noteId);

        }

        public async Task DeleteNote(long userId, long noteId)
        {
            await _repo
                .DeleteNote(userId, noteId);
        }
    }
}
