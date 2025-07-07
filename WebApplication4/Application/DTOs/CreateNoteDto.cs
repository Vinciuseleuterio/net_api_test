namespace NotesApp.Application.DTOs
{
    public class CreateNoteDto
    {
        public required string Title { get; set; }
        public string? Content { get; set; }
    }
}