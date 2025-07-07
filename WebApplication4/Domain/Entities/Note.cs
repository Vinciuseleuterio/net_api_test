namespace NotesApp.Domain.Models
{
    public class Note : StandardModel
    {
        public required string Title { get; set; }
        public string? Content { get; set; }
        public required long CreatorId { get; init; }
        public User User { get; private set; } = null!;
        public long? GroupId { get; init; }
        public Group Group { get; private set; } = null!;
    }
}