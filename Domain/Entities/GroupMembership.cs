using Domain.Entities;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Domain.Entities
{
    public class GroupMembership : ISoftDelete
    {
        public long UserId { get; init; }
        public User User { get; private set; } = null!;
        public long GroupId { get; init; }
        public Group Group { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsDeleted { get; set; }

        public void SetCreatedAt()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
        public void SetIsDeleted()
        {
            IsDeleted = true;

        }
    }
}
