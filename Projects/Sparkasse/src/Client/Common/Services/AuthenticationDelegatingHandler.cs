using System.Net.Http.Headers;

namespace Sparkasse.Client.Common.Services;

/// <summary>
/// Delegating handler that automatically adds Bearer token authentication to HTTP requests.
/// This handler is added to the HttpClient pipeline and transparently handles authentication
/// for all API calls without requiring changes to individual services.
/// </summary>
public class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly ITokenStorageService _tokenStorage;

    public AuthenticationDelegatingHandler(ITokenStorageService tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        // Get the access token from storage
        var token = _tokenStorage.GetAccessToken();

        // If we have a token, add it to the Authorization header
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Continue with the request
        return await base.SendAsync(request, cancellationToken);
    }
}
