using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Dtos;
using WebApplication4.Data;
using WebApplication4.Dto;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public NoteController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CreateNoteDto>>> GetAllNotesFromUser(long? userId)
        {
            var notes = await _context.Note
                .Where(note => note.CreatorId == userId)
                .ToListAsync();

            if (notes.Count == 0)
            {
                return NotFound();
            }

            return Ok(notes.Select(note => NotesToDto(note)));
        }

        [HttpGet("{userId}/{groupId}")]
        public async Task<ActionResult<IEnumerable<CreateNoteDto>>> GetAllNotesFromGroup(long? userId, long? groupId)
        {

            var group = await _context.GroupMembership
                .Where(userToGroup => userToGroup.GroupId == groupId && userToGroup.UserId == userId)
                .FirstAsync();

            if (group == null)
            {
                NotFound();
            }

            var notes = await _context.Note.
                Where(note => note.GroupId == groupId)
                .ToListAsync();

            return Ok(notes.Select(note => NotesToDto(note)));
        }

        [HttpPatch("{userId}/{noteId}")]
        public async Task<ActionResult<CreateNoteDto>> EditNoteFromUser(long userId, long noteId, EditNoteDto editNoteDto)
        {
            var note = _context.Note
                .FirstOrDefault(note => note.Id == noteId);

            if (note == null)
            {
                return NotFound();
            }

            if (note.CreatorId != userId)
            {
                return BadRequest("Nota não pertence ao usuário");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!string.IsNullOrEmpty(editNoteDto.Title))
            {
                note.Title = editNoteDto.Title;
            }

            if (!string.IsNullOrEmpty(editNoteDto.Content))
            {
                note.Content = editNoteDto.Content;
            }

            _context.Entry(note);
            note.Updated();
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<CreateNoteDto>> CreatePersonalNote(CreateNoteDto noteDto, long userId)
        {
            var user = await _context.User
            .FindAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Note note = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                CreatorId = userId
            };

            _context.Note.Add(note);
            note.Created();
            await _context.SaveChangesAsync();

            return Created("", NotesToDto(note));
        }

        [HttpPost("{userId}/{groupId}")]
        public async Task<ActionResult<CreateNoteDto>> CreateGroupNote(CreateNoteDto noteDto, long userId, long groupId)
        {

            var user = await _context.User
                .FindAsync(userId);

            if (user == null)
            {
                return NotFound();

            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Note note = new Note
            {
                CreatorId = userId,
                GroupId = groupId,
                Title = noteDto.Title,
                Content = noteDto.Content
            };

            _context.Note.Add(note);
            note.Created();
            await _context.SaveChangesAsync();

            return Created("", NotesToDto(note));
        }


        [HttpDelete("{userId}/{noteId}")]
        public async Task<ActionResult<CreateNoteDto>> DeleteNoteFromUser(long userId, long noteId)
        {
            var note = await _context.Note
                .Where(note => note.Id == noteId)
                .FirstOrDefaultAsync();

            if (note == null)
            {
                return NotFound();
            }

            if (userId != note.CreatorId)
            {
                return Forbid();
            }

            note.Delete();
            await _context.SaveChangesAsync();

            return Ok();
        }

        private static CreateNoteDto NotesToDto(Note note) =>
            new CreateNoteDto
            {
                Title = note.Title,
                Content = note.Content
            };
    }
}