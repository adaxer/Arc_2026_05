using LabEventSourcing.Events;
using MediatR;

namespace LabCQRS.WebApi.CQRS;

public record BankAccountEvent(DateTime Timestamp, decimal? Amount);

public class BankAccountQueryHandler : 
    IRequestHandler<GetBalanceQuery, decimal>, 
    IRequestHandler<GetStatementQuery, IEnumerable<BankAccountEvent?>>
{
    private readonly BankAccountFactory _factory;

    private BankAccount Account 
        => _factory.CreateFromRouteParam();

    public BankAccountQueryHandler(BankAccountFactory factory) 
        => _factory = factory;

    public Task<decimal> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Account.GetBalance());
    }

    public Task<IEnumerable<BankAccountEvent?>> Handle(GetStatementQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Account.Select(ToBankAccountEvent));
    }

    private static BankAccountEvent? ToBankAccountEvent(Event e) => e switch
    {
        AccountOpenedEvent o => new BankAccountEvent(o.Timestamp, o.InitialBalance),
        MoneyDepositedEvent d => new BankAccountEvent(d.Timestamp, d.Amount),
        MoneyWithdrawnEvent w => new BankAccountEvent(w.Timestamp, -w.Amount),
        AccountClosedEvent c => new BankAccountEvent(c.Timestamp, null),
        _ => null
    };
}