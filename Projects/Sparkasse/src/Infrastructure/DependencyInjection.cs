using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sparkasse.Application.Common.Interfaces;
using Sparkasse.Infrastructure.Data;
using Sparkasse.Infrastructure.Data.Interceptors;
using Sparkasse.Infrastructure.Identity;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(Services.Database);
        Guard.Against.Null(connectionString, message: $"Connection string '{Services.Database}' not found.");

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlite(connectionString);
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });


        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        var authBuilder = builder.Services.AddAuthentication(options =>
            {
                // Allow both Cookie and Bearer Token authentication
                // Set Application scheme as default for backward compatibility with Identity API
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            });

        authBuilder.AddIdentityCookies();
        authBuilder.AddBearerToken(IdentityConstants.BearerScheme);

        // Add named policy that accepts BOTH Cookie (Angular) AND Bearer Token (Avalonia)
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("CookieOrBearer", policy =>
            {
                policy.AddAuthenticationSchemes(
                    IdentityConstants.ApplicationScheme,  // Cookie-based (Angular)
                    IdentityConstants.BearerScheme         // Bearer Token (Avalonia)
                );
                policy.RequireAuthenticatedUser();
            });

        builder.Services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();
    }
}
