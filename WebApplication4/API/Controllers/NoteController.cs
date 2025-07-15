using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Models;

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
            try
            {
                var note = await _service
                    .CreatePersonalNote(noteDto, userId);

                return Created("", NoteToDto(note));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Errors });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error saving in the database: " + ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpPost("{userId}/{groupId}")]
        public async Task<ActionResult> CreateGroupNote(NoteDto noteDto, long userId, long groupId)
        {
            try
            {
                var note = await _service
                    .CreateGroupNote(noteDto, userId, groupId);

                return Created("", NoteToDto(note));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Errors });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error saving in the database: " + ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpGet("user/{userId}/{noteId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNoteById(long userId, long noteId)
        {
            try
            {
                var note = await _service
                    .GetNoteById(userId, noteId);

                return Ok(note);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAllNotesFromUser(long userId)
        {
            try
            {
                var notes = await _service
                    .GetAllNotesFromUser(userId);

                return Ok(notes
                    .Select(notes => NoteToDto(notes)));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpGet("{userId}/{groupId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAllNotesFromGroup(long userId, long groupId)
        {
            try
            {
                var notes = await _service
                    .GetAllNotesFromGroup(userId, groupId);

                return Ok(notes
                    .Select(notes => NoteToDto(notes)));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpPatch("{userId}/{noteId}")]
        public async Task<ActionResult<NoteDto>> UpdateNote(NoteDto noteDto, long userId, long noteId)
        {
            try
            {
                var note = await _service
                    .UpdateNote(noteDto, userId, noteId);

                return Ok(NoteToDto(note));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Errors });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error saving in the database: " + ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        [HttpDelete("{userId}/{noteId}")]
        public async Task<ActionResult> DeleteNote(long userId, long noteId)
        {
            try
            {
                await _service
                    .DeleteNote(userId, noteId);

                return Ok("Note was deleted");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Error");
            }
        }

        private static NoteDto NoteToDto(Note note) =>
            new NoteDto
            {
                Title = note.Title,
                Content = note.Content
            };
    }
}