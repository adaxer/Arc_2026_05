
using LabEventSourcing.Events;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Routing.Patterns;

namespace LabCQRS.WebApi;

public class BankAccountFactory
{
    private readonly EventStore _store;
    private readonly HttpContext? _context;

    public BankAccountFactory(EventStore store, IHttpContextAccessor accessor)
    {
        _store = store;
        _context = accessor.HttpContext;
    }

    public BankAccount CreateNew(Guid accountId)
    {
        return new BankAccount(_store, accountId);
    }

    public BankAccount CreateFromRouteParam(string key = "accountId")
    {
        // Extract Account ID from route data or query string
        var accountIdStr = _context.Request.RouteValues[key]?.ToString()
            ?? _context.Request.Query[key].ToString();

        if (Guid.TryParse(accountIdStr, out var accountId))
            return new BankAccount(_store, accountId);

        throw new KeyNotFoundException($"{key} not found in route data or query string.");
    }
}
