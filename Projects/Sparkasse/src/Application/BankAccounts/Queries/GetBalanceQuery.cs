using Sparkasse.Application.Common.Models;

namespace Sparkasse.Application.BankAccounts.Queries;

public record GetBalanceQuery(Guid AccountId) : IRequest<Result<decimal>>;
