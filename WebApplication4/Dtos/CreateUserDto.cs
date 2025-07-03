namespace NotesApp.Dtos
{
    public class CreateUserDto
    {
        public required string Name { get; set; }
        public required string Email { get; init; }
        public string? AboutMe { get; set; }
    }
}