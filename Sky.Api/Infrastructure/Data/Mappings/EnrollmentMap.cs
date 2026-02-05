using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sky.Api.Domain.Entities;

namespace Sky.Api.Infrastructure.Data.Mappings
{
    public class EnrollmentMap : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("Enrollments");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.EnrolledAt)
                .IsRequired();
          
            builder.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey("StudentId")
                .OnDelete(DeleteBehavior.Restrict);
           
            builder.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey("CourseId")
                .OnDelete(DeleteBehavior.Restrict);
         
            builder.HasIndex("StudentId", "CourseId")
                .IsUnique()
                .HasDatabaseName("IX_Enrollments_Student_Course");
        }
    }
}
