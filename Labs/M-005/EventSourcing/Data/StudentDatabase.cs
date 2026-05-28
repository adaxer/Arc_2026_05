using EventSourcing.Events;

namespace EventSourcing.Data
{
    public class StudentDatabase
    {
        // 1. Events InMemory speichern
        private readonly Dictionary<Guid, SortedList<DateTime, Event>> _studentEvents = [];

        public void Append(Event e)
        {
            if (!_studentEvents.ContainsKey(e.StreamId))
            {
                _studentEvents[e.StreamId] = [];
            }

            e.CreatedAtUtc = DateTime.UtcNow;
            _studentEvents[e.StreamId].Add(e.CreatedAtUtc, e);

            // 3. Projektion (async oder sync)
            var studentId = e.StreamId;
            _studentProjection[studentId] = GetStudent(studentId);
        }

        // 2. Student aus EventSourcing materialisieren
        internal Student? GetStudent(Guid studentId)
        {
            if (!_studentEvents.ContainsKey(studentId))
            {
                return null;
            }

            return _studentEvents[studentId]
                .Aggregate(new Student(), (s, e) => s.Apply(e.Value));
        }

        // 3. Asynchrone Projektion // strong consistancy vs. eventual consistancy
        private readonly Dictionary<Guid, Student> _studentProjection = new();

        internal Student? GetStudentView(Guid studentId)
        {
            return _studentProjection.GetValueOrDefault(studentId);
        }
    }
}
