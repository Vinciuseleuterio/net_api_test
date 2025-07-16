using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotesApp.Domain.Entities;

namespace NotesApp.Infrastructure.Data.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("groups");

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnName("name");
            builder.Property(g => g.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            builder.Property(g => g.CreatorId)
                .HasColumnName("creator_id");


            builder.HasOne(g => g.Creator)
                .WithMany(u => u.Groups)
                .HasForeignKey(g => g.CreatorId);
        }
    }
}
