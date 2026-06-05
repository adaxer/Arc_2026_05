namespace Sparkasse.Client.Common.Messages;

/// <summary>
/// Message sent when a user successfully logs in.
/// </summary>
public class UserLoggedInMessage
{
    public string Username { get; }

    public UserLoggedInMessage(string username)
    {
        Username = username;
    }
}

/// <summary>
/// Message sent when a user logs out.
/// </summary>
public class UserLoggedOutMessage
{
}
