using Sparkasse.Application.Common.Models;

namespace Sparkasse.Application.BankAccounts.Commands;

public record CloseAccountCommand(Guid AccountId) : IRequest<Result<decimal>>;
