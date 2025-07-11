namespace NotesApp.Application.DTOs
{
    public class CreateUserDto
    {
        public required string Email { get; init; }
        public required string Name { get; set; }
        public string? AboutMe { get; set; }
    }
}