namespace EventSourcing.Events
{
    public abstract class Event
    {
        // Events treten immer zu einem Zeitpunkt auf
        public DateTime CreatedAtUtc { get; set; }

        // Abfolge von Events als Streams eindeutig identifizieren
        public abstract Guid StreamId { get; }
    }

    // Wir verwenden keine "regulaere" Student class
    public class StudentRegistered : Event
    {
        public required Guid StudentId { get; init; }

        public required string FullName { get; init; }

        public required string Email { get; init; }

        public required DateTime DateOfBirth { get; init; }

        public override Guid StreamId => StudentId;
    }

    public class StudentEmailUpdated : Event
    {
        public required Guid StudentId { get; init; }

        public required string Email { get; init; }

        public override Guid StreamId => StudentId;
    }
    
    public class StudentEnrolled : Event
    {
        public required Guid StudentId { get; init; }

        public required string CourseName { get; set; }

        public override Guid StreamId => StudentId;
    }
    
    public class StudentDisenrolled : Event
    {
        public required Guid StudentId { get; init; }

        public required string CourseName { get; set; }

        public override Guid StreamId => StudentId;
    }
}
