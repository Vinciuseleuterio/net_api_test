using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NotesApp.Infrastructure.Data.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("notes");

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnName("title");
            builder.Property(n => n.Content)
                .IsRequired()
                .HasColumnName("content");
            builder.Property(n => n.CreatorId)
                .IsRequired()
                .HasColumnName("creator_id");
            builder.Property(n => n.GroupId)
                .HasColumnName("groupId");

            builder.HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.CreatorId);
        }
    }
}
