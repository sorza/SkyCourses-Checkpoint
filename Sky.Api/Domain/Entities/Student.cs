using Sky.Api.Domain.ValueObjects;

namespace Sky.Api.Domain.Entities
{
    public class Student
    {
        #region Properties
        public int Id { get; }
        public Email Email { get; private set; }
        public DateTime RegistratedAt { get; set; }
        #endregion

        #region Constructor

        private Student(Email email)
        {
            Email = email;
            RegistratedAt = DateTime.UtcNow;
        }

        #endregion

        #region Factory
        public static Student Create(string email)
        {
            if (email is null)
                throw new ArgumentException("O email não pode ser nulo.");                                   
            
            return new Student(Email.Create(email));
        }

        #endregion

    }
}
