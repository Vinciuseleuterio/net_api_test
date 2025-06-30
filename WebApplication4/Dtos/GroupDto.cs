using System.ComponentModel.DataAnnotations;

namespace NotesApp.Dto
{
    public class GroupDto
    {
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }
    }
}
