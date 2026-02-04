namespace Sky.Api.Domain.Entities
{
    public class Enrollment
    {
        #region Properties
        public int Id { get; }
        public Student Student { get; private set; }
        public Course Course { get; private set; }
        public DateTime EnrolledAt { get; private set; }

        #endregion

        #region Constructors
        private Enrollment() { }

        private Enrollment(Student student, Course course)
        {
            Student = student;
            Course = course;
            EnrolledAt = DateTime.UtcNow;
        }

        #endregion

        #region Factory
        public static Enrollment Create(Student student, Course course)
        {
            if (student is null)
                throw new ArgumentException("O estudante não pode ser nulo.");

            if (course is null)
                throw new ArgumentException("O curso não pode ser nulo.");

            return new Enrollment(student, course);
        }
        #endregion
    }
}
