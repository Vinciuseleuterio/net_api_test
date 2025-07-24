using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
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

        public void HandleSoftDelete()
        {
            var softDeletables = ChangeTracker.Entries()
                .Where(e => e.Entity is ISoftDelete)
                .ToList();

            foreach (var entry in softDeletables)
            {
                CascadeSoftDelete(entry.Entity);
            }
        }

        public void CascadeSoftDelete(object entity)
        {
            if (entity is User userN)
            {
                foreach (var note in userN.Notes)
                {
                    if (!note.IsDeleted) 
                    {
                        note.IsDeleted = true;
                        note.SetUpdatedAt();
                    }
                        
                }
            }

            if (entity is Group groupN)
            {
                foreach (var noteGroup in groupN.Notes)
                {
                    if (!noteGroup.IsDeleted) 
                    {
                        noteGroup.IsDeleted = true;
                        noteGroup.SetUpdatedAt();
                    }
                        
                }
            }

            if (entity is Group groupM)
            {
                foreach (var groupMembershipGroup in groupM.GroupMemberships)
                {
                    if (!groupMembershipGroup.IsDeleted) 
                    {
                        groupMembershipGroup.IsDeleted = true;
                    }
                        
                }
            }
        }
    }
}