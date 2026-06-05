using Sparkasse.Application.Common.Models;

namespace Sparkasse.Application.BankAccounts.Commands;

public record WithdrawCommand(Guid AccountId, decimal Amount) : IRequest<Result<decimal>>;
