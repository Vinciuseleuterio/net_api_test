using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Models
{
    public class Note
    {
        public long Id { get; init; }
        [NotNull][StringLength(50)] public string? Title { get; set; }
        [NotNull][StringLength(1000)] public string? Text { get; set; }
        public long UserId { get; set; } // chave estrangeira explícita
        [ForeignKey("UserId")] public User? User { get; init; } // navegação para o usuário
    }
}