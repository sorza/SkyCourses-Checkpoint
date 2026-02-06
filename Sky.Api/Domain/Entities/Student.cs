using Microsoft.AspNetCore.Identity;
using Sky.Api.Domain.Definitions;
using Sky.Api.Domain.ValueObjects;

namespace Sky.Api.Domain.Entities
{
    public class Student : Entity
    {
        #region Properties
        public string UserId { get; private set; }
        public Email Email { get; private set; }
        public DateTime RegistratedAt { get; set; }
        public IdentityUser User { get; private set; }
        public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();
        #endregion

        #region Constructors

        private Student() { }

        private Student(string userId, Email email)
        {
            UserId = userId;
            Email = email;
            RegistratedAt = DateTime.UtcNow;
        }

        #endregion

        #region Factory
        public static Student Create(string userId, string email)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("O UserId não pode ser nulo.");

            if (email is null)
                throw new ArgumentException("O email não pode ser nulo.");                                   
            
            return new Student(userId, Email.Create(email));
        }

        #endregion

    }
}
