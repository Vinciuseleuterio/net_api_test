using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Data
{
    public class
        ApplicationContext : DbContext // Superclasse herdada pelo nosso conexto que permite manipulação do banco
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :
            base(options) // Chama o construtor da classe "DbContext" com as configurações de contexto definidas
        {
        }

        public DbSet<Note> Note { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        // Representação das entidades no contexto específico. O "null!" é um tratamento de nulabilidade, já que o compilador verifica as propriedades não anuláveis durante a execução

        // Resumidamente, esse arquivo serve como "ponte" entre a aplicação e o banco de dados
    }
}