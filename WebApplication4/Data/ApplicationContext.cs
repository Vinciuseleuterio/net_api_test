using Microsoft.EntityFrameworkCore;
using NotesApp.Models;
using WebApplication4.Models;

namespace WebApplication4.Data
{
    public class
        ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :
            base(options)
        {
        }

        public DbSet<Note> Note { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<Group> Group { get; set; } = null!;
        public DbSet<GroupMembership> GroupMembership { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder
                .Entity<Group>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder
                .Entity<Note>()
                .HasQueryFilter(n => !n.IsDeleted);

            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Deleções em cascata na entidade "User"

            modelBuilder.Entity<Group>()
                .HasOne(g => g.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(g => g.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMembership>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.GroupMemberships)
                .HasForeignKey(gm => gm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Deleções em cascata na entidade "Group"

            modelBuilder.Entity<Note>()
                .HasOne(n => n.Group)
                .WithMany(g => g.Notes)
                .HasForeignKey(n => n.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMembership>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.GroupMemberships)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}