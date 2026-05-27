
// 1. Mediator-Interface

// 2. Konkreter Mediator (Chatraum)
public class ChatRoom : IChatRoom
{
    private List<User> _users = new List<User>();

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public void SendMessage(User sender, string message)
    {
        // Leitet Nachricht an alle anderen Benutzer weiter
        foreach (var user in _users.Where(u => u != sender))
        {
            user.Receive(sender.Name, message);
        }
    }
}
