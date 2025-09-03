using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NotesApp.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnName("name");
            builder.Property(u => u.Email)
                .IsRequired()
                .HasColumnName("email");
            builder.Property(u => u.AboutMe)
                .HasMaxLength(250)
                .HasColumnName("about_me");
        }
    }
}
