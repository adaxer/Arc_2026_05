using Scalar.AspNetCore;
using Sparkasse.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// CORS configuration for Cookie-based authentication
// AllowCredentials() is required for cookies to work cross-origin
// Cannot use AllowAnyOrigin() with AllowCredentials()
app.UseCors(builder =>
{
    if (app.Environment.IsDevelopment())
    {
        // In development, allow local Angular dev server
        builder
            .WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Required for cookies
    }
    else
    {
        // In production, use the same origin (cookies work by default)
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    }
});

app.UseFileServer();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseExceptionHandler(options => { });

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapEndpoints(typeof(Program).Assembly);

app.MapFallbackToFile("index.html");

app.Run();
