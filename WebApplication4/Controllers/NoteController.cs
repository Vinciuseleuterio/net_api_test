using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAllNotesFromUser(long? userId)
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
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAllNotesFromGroup(long? userId, long? groupId)
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
        public async Task<ActionResult<NoteDto>> EditNoteFromUser(long userId, long noteId, NoteDto noteDto)
        {
            var note = _context.Note
                .FirstOrDefault(note => note.Id == noteId);

            if (note == null)
            {
                return NotFound();
            }

            if (note.CreatorId != userId)
            {
                return Forbid();
            }

            note.Content = noteDto.Text;
            note.Title = noteDto.Title;

            _context.Entry(note);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<NoteDto>> CreatePersonalNote(NoteDto noteDto, long userId)
        {
            var user = _context.User.FirstOrDefaultAsync(user => user.Id == userId);

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
                Content = noteDto.Text,
                CreatorId = userId
            };

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return Created("", NotesToDto(note));
        }

        [HttpPost("{userId}/{groupId}")]

        public async Task<ActionResult<NoteDto>> CreateGroupNote(NoteDto noteDto, long userId, long groupId)
        {

            var user = _context.User.FirstOrDefaultAsync(user => user.Id == userId);

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
                Title = noteDto.Title,
                Content = noteDto.Text,
                GroupId = groupId
            };

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return Created("", NotesToDto(note));
        }


        [HttpDelete("{userId}/{noteId}")]
        public async Task<ActionResult<NoteDto>> DeleteNoteFromUser(long userId, long noteId)
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

        private static NoteDto NotesToDto(Note note) =>
            new NoteDto
            {
                Title = note.Title,
                Text = note.Content
            };
    }
}