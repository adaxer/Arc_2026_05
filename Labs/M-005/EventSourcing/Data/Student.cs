using EventSourcing.Events;

namespace EventSourcing
{
    // 2. Student representiert die Daten der Events
    public class Student
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<string> EnrolledCourses { get; set; } = [];

        #region Apply
        private Student Apply(StudentRegistered student)
        {
            Id = student.StudentId;
            FullName = student.FullName;
            Email = student.Email;
            DateOfBirth = student.DateOfBirth;
            return this;
        }

        private Student Apply(StudentEmailUpdated student)
        {
            Email = student.Email;
            return this;
        }

        private Student Apply(StudentEnrolled student)
        {
            if (!EnrolledCourses.Contains(student.CourseName))
            {
                EnrolledCourses.Add(student.CourseName);
            }
            return this;
        }

        private Student Apply(StudentDisenrolled student)
        {
            if (EnrolledCourses.Contains(student.CourseName))
            {
                EnrolledCourses.Remove(student.CourseName);
            }
            return this;
        } 

        public Student Apply(Event e) => e switch
        {
            StudentRegistered student => Apply(student),
            StudentEmailUpdated student => Apply(student),
            StudentEnrolled student => Apply(student),
            StudentDisenrolled student => Apply(student),
            _ => this,
        };

        #endregion
    }
}
