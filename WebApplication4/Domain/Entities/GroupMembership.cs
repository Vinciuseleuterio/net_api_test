using NotesApp.Domain.Interfaces;

namespace NotesApp.Domain.Models
{
    public class GroupMembership : ISoftDelete
    {
        public required long UserId { get; init; }
        public User User { get; private set; } = null!;
        public required long GroupId { get; init; }
        public Group Group { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; set; }

        public void SetCreatedAt()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public void SetDeletedAt()
        {
            IsDeleted = true;

        }
    }
}
