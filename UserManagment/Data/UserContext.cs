using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace UsersService.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) :
    base(options) // Chama o construtor da classe "DbContext" com as configurações de contexto definidas
        {
        }

        public DbSet<User> User { get; set; } = null!;
    }
}
