using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal;

namespace WebApplication4.Models
{
    public class User
    {
        public long Id { get; init; }
        [NotNull] [StringLength(50)] public string? Name { get; set; }
        [NotNull] [StringLength(100)] public string? Email { get; set; }
        [StringLength(250)] public string? AboutMe { get; set; }

        [DeleteBehavior(DeleteBehavior.Cascade)]
        public List<Note>? Note { get; init; }
    }
}