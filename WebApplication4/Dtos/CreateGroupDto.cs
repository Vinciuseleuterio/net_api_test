using System.ComponentModel.DataAnnotations;

namespace NotesApp.Dtos
{
    public class CreateGroupDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
