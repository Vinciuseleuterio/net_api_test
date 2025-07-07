using System.ComponentModel.DataAnnotations;

namespace NotesApp.Application.DTOs
{
    public class CreateGroupDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
