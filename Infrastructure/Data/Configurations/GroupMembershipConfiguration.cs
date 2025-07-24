using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotesApp.Domain.Entities;

namespace NotesApp.Infrastructure.Data.Configurations
{
    public class GroupMembershipConfiguration : IEntityTypeConfiguration<GroupMembership>
    {
        public void Configure(EntityTypeBuilder<GroupMembership> builder)
        {
            builder.ToTable("group_membership");

            builder.HasKey(gm => new { gm.UserId, gm.GroupId });

            builder.Property(gm => gm.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at");
            builder.Property(gm => gm.IsDeleted)
                .IsRequired()
                .HasColumnName("is_deleted");
            builder.Property(gm => gm.UpdatedAt)
                .HasColumnName("updated_at");
            builder.Property(gm => gm.UserId)
                .HasColumnName("user_id");
            builder.Property(gm => gm.GroupId)
                .HasColumnName("group_id");

            builder
                .HasOne(gm => gm.User)
                .WithMany(u => u.GroupMemberships)
                .HasForeignKey(gm => gm.UserId);
            builder
                .HasOne(gm => gm.Group)
                .WithMany(g => g.GroupMemberships)
                .HasForeignKey(gm => gm.GroupId);
        }
    }
}
