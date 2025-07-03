using System.ComponentModel.DataAnnotations;

namespace NotesApp.Dtos
{
    public class EditGroupDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
