using Sparkasse.Application.Common.Models;

namespace Sparkasse.Application.BankAccounts;

public interface IBankAccountService
{
    Task<Result<Guid>> OpenAccountAsync(decimal initialBalance, CancellationToken cancellationToken = default);
    Task<Result<decimal>> CloseAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<Result<decimal>> DepositAsync(Guid accountId, decimal amount, CancellationToken cancellationToken = default);
    Task<Result<decimal>> WithdrawAsync(Guid accountId, decimal amount, CancellationToken cancellationToken = default);
    Task<Result<decimal>> GetBalanceAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Queries.BankAccountValue>>> GetStatementAsync(Guid accountId, CancellationToken cancellationToken = default);
}
