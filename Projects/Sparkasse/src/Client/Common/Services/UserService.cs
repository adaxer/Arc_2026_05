using System.Net.Http.Json;
using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;
using Sparkasse.Client.Common.Messages;

namespace Sparkasse.Client.Common.Services;

public interface IUserService
{
    Task<LoginResult> LoginAsync(string username, string password);
    Task LogoutAsync();
}

/// <summary>
/// User authentication service that handles login/logout and token management.
/// Stores tokens in ITokenStorageService for use by AuthenticationDelegatingHandler.
/// Sends UserLoggedInMessage and UserLoggedOutMessage via WeakReferenceMessenger.
/// </summary>
public class UserService : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenStorageService _tokenStorage;
    private readonly IMessenger _messenger;
    private readonly JsonSerializerOptions _jsonOptions;

    public UserService(
        IHttpClientFactory httpClientFactory, 
        ITokenStorageService tokenStorage,
        IMessenger messenger)
    {
        _httpClientFactory = httpClientFactory;
        _tokenStorage = tokenStorage;
        _messenger = messenger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("WebApi");

            var loginRequest = new
            {
                email = username,
                password = password
            };

            var response = await httpClient.PostAsJsonAsync("/api/users/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                // Parse the token response
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(_jsonOptions);

                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    // Store tokens for future API calls
                    _tokenStorage.SetTokens(tokenResponse.AccessToken, tokenResponse.RefreshToken ?? string.Empty);

                    // Send login message
                    _messenger.Send(new UserLoggedInMessage(username));

                    return new LoginResult { Success = true };
                }

                return new LoginResult 
                { 
                    Success = false, 
                    ErrorMessage = "Login erfolgreich, aber keine Tokens erhalten." 
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return new LoginResult 
            { 
                Success = false, 
                ErrorMessage = $"Login fehlgeschlagen: {response.StatusCode}" 
            };
        }
        catch (Exception ex)
        {
            return new LoginResult 
            { 
                Success = false, 
                ErrorMessage = $"Verbindungsfehler: {ex.Message}" 
            };
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("WebApi");
            await httpClient.PostAsJsonAsync("/logout", new { });
        }
        catch
        {
            // Logout error handling
        }
        finally
        {
            // Always clear tokens on logout
            _tokenStorage.ClearTokens();

            // Send logout message
            _messenger.Send(new UserLoggedOutMessage());
        }
    }
}

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; } = "Bearer";
}

public class LoginResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}
