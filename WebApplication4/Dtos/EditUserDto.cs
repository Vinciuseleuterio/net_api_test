using System.ComponentModel.DataAnnotations;

namespace NotesApp.Dto
{
    public class EditUserDto
    {
        [StringLength(50)]
        public string? Name { get; init; }

        [StringLength(250)]
        public string? AboutMe { get; set; }
    }
}
