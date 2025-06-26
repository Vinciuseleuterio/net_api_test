using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication4.Models;

namespace NotesApp.Models
{
    [Table("groups")]
    public class Group : StandardModel
    {
        [Required]
        [Column("name")]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [Column("description")]
        [StringLength(250)]
        public required string Description { get; set; }

        [Column("creator_id")]
        public required long CreatorId { get; init; }

        [ForeignKey(nameof(CreatorId))]
        public User Creator { get; private set; } = null!;

        [NotMapped]
        public List<Note> Notes { get; private set; } = [];

        [NotMapped]
        public List<GroupMembership> GroupMemberships { get; private set; } = [];
    }
}
