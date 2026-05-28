using EventSourcing.Data;
using EventSourcing.Events;

namespace EventSourcing
{
    internal class Program
    {
        const string StundentId = "96A40BE5-7C7D-4385-8493-975873E959BE";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var studentId = Guid.Parse(StundentId);

            // 1. Events persistieren
            var db = new StudentDatabase();

            var registered = new StudentRegistered
            {
                StudentId = studentId,
                FullName = "Bugs 🐰",
                Email = "bugs@bunny.com",
                DateOfBirth = new DateTime(1940, 7, 27)
            };
            db.Append(registered);
            Console.WriteLine($"Student {registered.FullName} created!");


            var enrolled = new StudentEnrolled
            {
                StudentId = studentId,
                CourseName = "Moderne Architekturen"
            };
            db.Append(enrolled);
            Console.WriteLine($"Student enrolled to {enrolled.CourseName}!");


            var studentUpdated = new StudentEmailUpdated
            {
                StudentId = studentId,
                Email = "whats.up@doc.com",
            };
            db.Append(studentUpdated);
            Console.WriteLine($"Student email updated to {studentUpdated.Email}!");


            // 2. Student aus EventSourcing materialisieren
            var student = db.GetStudent(studentId);
            Console.WriteLine($"Student {student.FullName} wurde aus EventSourcing materialisiert");

            // 3. Student aus projezierter View
            var studentFromView = db.GetStudentView(studentId);
            Console.WriteLine($"Student {student.FullName} aus projezierten View");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
