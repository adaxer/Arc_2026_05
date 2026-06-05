namespace Sparkasse.Client.Common.Services;

public interface ITokenStorageService
{
    string? GetAccessToken();
    void SetTokens(string accessToken, string refreshToken);
    void ClearTokens();
    bool IsAuthenticated { get; }
}

/// <summary>
/// In-memory token storage service.
/// For production, consider using platform-specific secure storage (e.g., Data Protection API on Windows).
/// </summary>
public class TokenStorageService : ITokenStorageService
{
    private string? _accessToken;
    private string? _refreshToken;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken);

    public string? GetAccessToken() => _accessToken;

    public void SetTokens(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;
    }

    public void ClearTokens()
    {
        _accessToken = null;
        _refreshToken = null;
    }
}
