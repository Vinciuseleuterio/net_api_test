using NotesApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models
{
    [Table("users")]
    public class User : StandardModel
    {

        [Required]
        [Column("name")]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [Column("email")]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Column("about_me")]
        [StringLength(250)]
        public string? AboutMe { get; set; }
        public List<Group> Groups { get; private set; } = [];
        public List<GroupMembership> GroupMemberships { get; private set; } = [];
        public List<Note> Notes { get; private set; } = [];
    }
}