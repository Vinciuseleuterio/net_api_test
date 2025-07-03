namespace NotesApp.Models
{
    public class Group : StandardModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required long CreatorId { get; init; }
        public User Creator { get; private set; } = null!;
        public List<Note> Notes { get; private set; } = [];
        public List<GroupMembership> GroupMemberships { get; private set; } = [];
    }
}
