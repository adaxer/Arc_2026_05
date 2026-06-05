using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sparkasse.Infrastructure.Identity;

namespace Sparkasse.Web.Endpoints;

public class Users : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        // Standard Identity API endpoints (Cookie-based AND Bearer token for /login endpoint)
        // Angular client uses /login with useCookies=true
        // Avalonia client uses /login with useCookies=false (default) to get Bearer tokens
        groupBuilder.MapIdentityApi<ApplicationUser>();

        groupBuilder.MapPost(Logout, "logout").RequireAuthorization();
    }

    [EndpointSummary("Log out")]
    [EndpointDescription("Logs out the current user by clearing the authentication cookie.")]
    public static async Task<Results<Ok, UnauthorizedHttpResult>> Logout(SignInManager<ApplicationUser> signInManager, [FromBody] object empty)
    {
        if (empty != null)
        {
            await signInManager.SignOutAsync();
            return TypedResults.Ok();
        }

        return TypedResults.Unauthorized();
    }
}
