using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using System.Reflection;

namespace NotesApp.Infrastructure.Data
{
    public class
        ApplicationContext : DbContext
    {

        private static readonly Assembly _assembly = typeof(ApplicationContext).Assembly;
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
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<Group>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Note>()
                .HasQueryFilter(n => !n.IsDeleted);

            modelBuilder.Entity<GroupMembership>()
                .HasQueryFilter(gm => !gm.IsDeleted);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.ApplyConfigurationsFromAssembly(_assembly);

            base.OnModelCreating(modelBuilder);
        }

        public static void CascadeSoftDelete(object entity)
        {
            if (entity is User user)
            {
                foreach (var note in user.Notes)
                {
                    if (!note.IsDeleted)
                    {
                        note.SetIsDeleted();
                        note.SetUpdatedAt();
                    }

                }
            }

            if (entity is User userG)
            {
                foreach (var groupU in userG.Groups)
                {
                    if (!groupU.IsDeleted)
                    {
                        groupU.SetIsDeleted();
                        groupU.SetUpdatedAt();
                    }
                }
            }

            if (entity is Group group)
            {
                foreach (var note in group.Notes)
                {
                    if (!note.IsDeleted)
                    {
                        note.SetIsDeleted();
                        note.SetUpdatedAt();
                    }

                }
            }

            if (entity is Group groupM)
            {
                foreach (var groupMembership in groupM.GroupMemberships)
                {
                    if (!groupMembership.IsDeleted)
                    {
                        groupMembership.SetIsDeleted();
                        groupMembership.SetUpdatedAt();
                    }

                }
            }
        }
    }
}
