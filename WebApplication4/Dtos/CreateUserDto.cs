using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Dto
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(50)]
        public required string Name { get; init; }

        [Required]
        [EmailAddress]
        public required string Email { get; init; }

        [StringLength(250)]
        public string? AboutMe { get; set; }
    }
}