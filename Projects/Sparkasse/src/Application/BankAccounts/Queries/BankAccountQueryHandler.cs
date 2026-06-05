using Sparkasse.Application.Common.Models;
using Sparkasse.Domain.Common;
using Sparkasse.Domain.Events;

namespace Sparkasse.Application.BankAccounts.Queries;

public record BankAccountValue(DateTime Timestamp, decimal? Amount);

public class BankAccountQueryHandler : 
    IRequestHandler<GetBalanceQuery, Result<decimal>>, 
    IRequestHandler<GetStatementQuery, Result<IEnumerable<BankAccountValue?>>>
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountQueryHandler(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    public Task<Result<decimal>> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
    {
        return _bankAccountService.GetBalanceAsync(request.AccountId, cancellationToken);
    }

    public async Task<Result<IEnumerable<BankAccountValue?>>> Handle(GetStatementQuery request, CancellationToken cancellationToken)
    {
        var result = await _bankAccountService.GetStatementAsync(request.AccountId, cancellationToken);

        if (!result.Succeeded || result.Value == null)
        {
            return Result<IEnumerable<BankAccountValue?>>.Failure(result.Errors);
        }

        var events = result.Value.Cast<BankAccountValue?>();
        return Result<IEnumerable<BankAccountValue?>>.Success(events);
    }

    private static BankAccountValue? ToBankAccountEvent(BaseEvent e) => e switch
    {
        AccountOpenedEvent o => new BankAccountValue(o.Timestamp, o.InitialBalance),
        MoneyDepositedEvent d => new BankAccountValue(d.Timestamp, d.Amount),
        MoneyWithdrawnEvent w => new BankAccountValue(w.Timestamp, -w.Amount),
        AccountClosedEvent c => new BankAccountValue(c.Timestamp, null),
        _ => null
    };
}
