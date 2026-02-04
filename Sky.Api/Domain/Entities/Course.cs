namespace Sky.Api.Domain.Entities
{
    public class Course
    {

        #region Properties
        public int Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public int Workload { get; private set; }
        public DateTime CreatedAt { get; private set; }
        #endregion

        #region Constructors

        private Course() { }

        private Course(string title, string description, string category, int workload) 
        {
            Title = title;
            Description = description;
            Category = category;
            Workload = workload;
            CreatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Factory

        public static Course Create(string title, string description, string category, int workload)
        {
            if(string.IsNullOrEmpty(title))
                throw new ArgumentException("O título não pode ser nulo.");

            if(string.IsNullOrEmpty(description))
                throw new ArgumentException("A descrição não pode ser nula.");

            if(string.IsNullOrEmpty(category))
                throw new ArgumentException("A categoria não pode ser nula.");

            if(workload <= 0)
                throw new ArgumentException("A carga horária deve ser maior que zero.");

            return new Course(title, description, category, workload);
        }

        #endregion


    }
}
