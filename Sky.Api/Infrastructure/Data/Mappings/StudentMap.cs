using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sky.Api.Domain.Entities;
using Sky.Api.Domain.ValueObjects;

namespace Sky.Api.Infrastructure.Data.Mappings
{
    public class StudentMap : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(s => s.RegistratedAt)
                .IsRequired();

            builder.OwnsOne(s => s.Email, email =>
            {
                email.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasMaxLength(Email.MaxLength);
            });           
           
            builder.HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
           
            builder.HasMany(s => s.Enrollments)
                .WithOne(e => e.Student)
                .OnDelete(DeleteBehavior.Cascade);
          
            builder.HasIndex(s => s.UserId).IsUnique();
        }
    }
}
