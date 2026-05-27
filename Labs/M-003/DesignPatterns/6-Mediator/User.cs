
// 1. Mediator-Interface

// 2. Konkreter Mediator (Chatraum)

// 3. Kollege (Benutzer)
public class User
{
    public string Name { get; }
    private IChatRoom _chatRoom;

    public User(string name, IChatRoom chatRoom)
    {
        Name = name;
        _chatRoom = chatRoom;
    }

    public void Send(string message)
    {
        _chatRoom.SendMessage(this, message);
    }

    public void Receive(string sender, string message)
    {
        Console.WriteLine($"[{Name}] empfängt von {sender}: {message}");
    }
}
