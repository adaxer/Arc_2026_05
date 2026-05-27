
public class Mediator
{
    public static void RunChat()
    {
        // Verwendung
        var chatRoom = new ChatRoom();
        var alice = new User("Alice", chatRoom);
        var bob = new User("Bob", chatRoom);

        chatRoom.AddUser(alice);
        chatRoom.AddUser(bob);

        alice.Send("Hallo Bob!"); // Bob erhält die Nachricht
        bob.Send("Hi Alice!");    // Alice erhält die Nachricht

    }
}
