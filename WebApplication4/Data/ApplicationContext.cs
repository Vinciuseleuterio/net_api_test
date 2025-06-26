using Microsoft.EntityFrameworkCore;
using NotesApp.Interfaces;
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
                .Entity<GroupMembership>()
                .HasQueryFilter(gm => !gm.IsDeleted);

            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            HandleSoftDelete();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleSoftDelete();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void HandleSoftDelete()
        {
            var softDeletables = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDelete)
                .ToList();

            foreach (var entry in softDeletables)
            {
                entry.State = EntityState.Modified;
                ((ISoftDelete)entry.Entity).IsDeleted = true;

                CascadeSoftDelete(entry.Entity);
            }
        }

        private void CascadeSoftDelete(object entity)
        {
            if (entity is User userN)
            {
                foreach (var noteUser in userN.Notes)
                {
                    if (!noteUser.IsDeleted)
                        noteUser.IsDeleted = true;
                }
            }

            if (entity is Group groupN)
            {
                foreach (var noteGroup in groupN.Notes)
                {
                    if (!noteGroup.IsDeleted)
                        noteGroup.IsDeleted = true;
                }
            }

            if (entity is Group groupM)
            {
                foreach (var groupMembershipGroup in groupM.GroupMemberships)
                {
                    if (!groupMembershipGroup.IsDeleted)
                        groupMembershipGroup.IsDeleted = true;
                }
            }
        }


    }
}