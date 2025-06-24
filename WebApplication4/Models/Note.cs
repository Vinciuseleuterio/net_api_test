using NotesApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models
{
    [Table("notes")]
    public class Note : StandardModel
    {
        [Required]
        [Column("title")]
        [StringLength(50)]
        public required string Title { get; set; }

        [Required]
        [Column("content")]
        [StringLength(1000)]
        public required string Content { get; set; }

        [Column("creator_id")]
        public required long CreatorId { get; init; }

        [ForeignKey(nameof(CreatorId))]
        public User User { get; private set; } = null!;

        [Column("group_id")]
        public long GroupId { get; init; }

        [ForeignKey(nameof(GroupId))]
        public Group Group { get; private set; } = null!;
    }
}