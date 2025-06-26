using NotesApp.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication4.Models;

namespace NotesApp.Models
{
    [Table("group_membership")]
    public class GroupMembership : ISoftDelete
    {
        [Key]
        public long Id { get; private init; }

        [Column("user_id")]
        public required long UserId { get; init; }

        [ForeignKey(nameof(UserId))]
        public User User { get; private set; } = null!;

        [Column("group_id")]
        public required long GroupId { get; init; }

        [ForeignKey(nameof(GroupId))]
        public Group Group { get; private set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; private set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        public void Created()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public void Delete()
        {
            IsDeleted = true;

        }
    }
}
