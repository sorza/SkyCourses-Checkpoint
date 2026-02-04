using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sky.Api.Domain.Entities;

namespace Sky.Api.Infra.Data.Mappings
{
    public class CourseMap : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Workload)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();
           
            builder.HasMany(c => c.Enrollments)
                .WithOne(e => e.Course)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
