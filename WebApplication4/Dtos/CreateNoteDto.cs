namespace NotesApp.Dtos
{
    public class CreateNoteDto
    {
        public required string Title { get; set; }
        public string? Content { get; set; }
    }
}