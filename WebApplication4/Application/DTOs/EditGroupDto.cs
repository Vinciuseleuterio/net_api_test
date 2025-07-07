using System.ComponentModel.DataAnnotations;

namespace NotesApp.Application.DTOs
{
    public class EditGroupDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
