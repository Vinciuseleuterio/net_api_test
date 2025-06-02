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
        // Realiza a injeção de dependência da "ApplicationContext" 
        private readonly ApplicationContext _context;

        public NoteController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAllNotesByUserId(long? userId)
        {
            if (userId == null | userId <= 0)
            {
                return BadRequest("UserId cannot be empty or null");
            }

            var notes = await _context.Note
                .Where(note => note.UserId == userId)
                .ToListAsync();

            if (notes.Count == 0)
            {
                return BadRequest("Note(s) does not exist");
            }

            return Ok(notes.Select(note => NotesToDto(note)));
        }

        [HttpPut("{userId}/{noteId}")]
        public async Task<ActionResult<NoteDto>> EditNoteFromUser(long userId, long noteId, NoteDto noteDto)
        {
            var note = _context.Note
                .FirstOrDefault(note => note.Id == noteId);

            if (note == null)
            {
                return NotFound("Note does not exist");
            }

            if (note.UserId != userId)
            {
                return Forbid("Note does not belong to user");
            }

            note.Text = noteDto.Text;
            note.Title = noteDto.Title;
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Created("The succeeding note was edited with success", NotesToDto(note));
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<NoteDto>> CreateNote(NoteDto noteDto, long userId)
        {
            var user = _context.User.FirstOrDefault(user => user.Id == userId);

            if (noteDto.Title == "" | noteDto.Text == "")
            {
                return BadRequest("Title and Text are required");
            }

            if (user == null)
            {
                return BadRequest("User does not exist");
            }

            Note note = new Note();
            note.Title = noteDto.Title;
            note.Text = noteDto.Text;
            note.UserId = userId; // Injeta o ID do usuário que vem da URL no campo "UserId"

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return Created("The succeeding note was created with success", NotesToDto(note));
        }

        [HttpDelete("{userId}/{noteId}")]
        public async Task<ActionResult<NoteDto>> DeleteNoteFromUser(long userId, long noteId)
        {
            var note = await _context.Note
                .Where(note => note.Id == noteId)
                .FirstOrDefaultAsync();

            if (note == null)
            {
                return NotFound("Note does not exist");
            }

            if (userId != note.UserId)
            {
                return Forbid("Note does not belong to user");
            }

            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return Ok("The note was deleted with success");
        }

        private bool NoteExists(long userId)
        {
            return _context.Note.Any(e => e.UserId == userId);
        }

        // Realiza a conversão da nota para o nosso DTO (Data Transfer Object)
        private static NoteDto NotesToDto(Note note) =>
            new NoteDto
            {
                Title = note.Title,
                Text = note.Text
            };
    }
}