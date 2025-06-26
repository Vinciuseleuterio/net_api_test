using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dto
{
    public class CreateNote
    {
        [Required]
        [StringLength(50)]
        public required string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public required string Content { get; set; }
    }
}