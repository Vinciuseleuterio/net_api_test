using System.ComponentModel.DataAnnotations;

namespace NotesApp.Dtos
{
    public class EditNoteDto
    {
        [StringLength(50)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Content { get; set; }
    }
}
