using System.ComponentModel.DataAnnotations;

namespace NotesApp.Dtos
{
    public class EditGroupDto
    {
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }
    }
}
