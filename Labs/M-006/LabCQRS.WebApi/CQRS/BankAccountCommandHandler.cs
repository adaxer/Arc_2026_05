using LabEventSourcing.Events;
using MediatR;
using Microsoft.AspNetCore.Routing.Patterns;

namespace LabCQRS.WebApi.CQRS;

public class BankAccountCommandHandler : 
    IRequestHandler<OpenAccountCommand, Guid>,
    IRequestHandler<CloseAccountCommand, decimal>,
    IRequestHandler<DepositCommand, decimal>,
    IRequestHandler<WithdrawCommand, decimal>
{
    private readonly BankAccountFactory _factory;

    public BankAccountCommandHandler(BankAccountFactory factory) 
        => _factory = factory;

    public Task<Guid> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
    {
        BankAccount account;

#if DEBUG
        try
        {
            // Zu Testzwecken festgelegte Guid verwenden
            account = _factory.CreateFromRouteParam();
        }
        catch (KeyNotFoundException)
        {
            account = _factory.CreateNew(Guid.NewGuid());
        }
#else
        account = _factory.CreateNew(Guid.NewGuid());
#endif
        account.Open(request.InitialBalance);
        return Task.FromResult(account.AccountId);
    }

    public Task<decimal> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        var account = _factory.CreateFromRouteParam();
        account.Close();
        return Task.FromResult(account.GetBalance());
    }

    public Task<decimal> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var account = _factory.CreateFromRouteParam();
        account.Deposit(request.Amount);
        return Task.FromResult(account.GetBalance());
    }

    public Task<decimal> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var account = _factory.CreateFromRouteParam();
        account.Withdraw(request.Amount);
        return Task.FromResult(account.GetBalance());
    }
}

