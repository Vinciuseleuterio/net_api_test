using Microsoft.AspNetCore.Mvc;
using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;

namespace NotesApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NoteService _service;

        public NoteController(
            NoteService service)
        {
            _service = service;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> CreatePersonalNote(NoteDto noteDto, long userId)
        {
            var note = await _service
                .CreatePersonalNote(noteDto, userId);

            return Created("", NoteToDto(note));
        }

        [HttpPost("{userId}/{groupId}")]
        public async Task<ActionResult> CreateGroupNote(NoteDto noteDto, long userId, long groupId)
        {
            var note = await _service
                .CreateGroupNote(noteDto, userId, groupId);

            return Created("", NoteToDto(note));
        }

        [HttpGet("user/{userId}/{noteId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNoteById(long userId, long noteId)
        {
            var note = await _service
                .GetNoteById(userId, noteId);

            return Ok(note);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAllNotesFromUser(long userId)
        {
            var notes = await _service
                .GetAllNotesFromUser(userId);

            return Ok(notes
                .Select(notes => NoteToDto(notes)));
        }

        [HttpGet("{userId}/{groupId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAllNotesFromGroup(long userId, long groupId)
        {
            var notes = await _service
                .GetAllNotesFromGroup(userId, groupId);

            return Ok(notes
                .Select(notes => NoteToDto(notes)));
        }

        [HttpPatch("{userId}/{noteId}")]
        public async Task<ActionResult<NoteDto>> UpdateNote(NoteDto noteDto, long userId, long noteId)
        {
            var note = await _service
                .UpdateNote(noteDto, userId, noteId);

            return Ok(NoteToDto(note));
        }

        [HttpDelete("{userId}/{noteId}")]
        public async Task<ActionResult> DeleteNote(long userId, long noteId)
        {
            await _service
                .DeleteNote(userId, noteId);

            return Ok("Note was deleted");
        }

        private static NoteDto NoteToDto(Note note) =>
            new NoteDto
            {
                Title = note.Title,
                Content = note.Content
            };
    }
}