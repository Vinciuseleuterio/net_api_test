using System.ComponentModel.DataAnnotations;

namespace NotesApp.Dto
{
    public class GroupDto
    {
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [StringLength(250)]
        public required string Description { get; set; }
    }
}
