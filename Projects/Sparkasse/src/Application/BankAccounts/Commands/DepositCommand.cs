using Sparkasse.Application.Common.Models;

namespace Sparkasse.Application.BankAccounts.Commands;

public record DepositCommand(Guid AccountId, decimal Amount) : IRequest<Result<decimal>>;
