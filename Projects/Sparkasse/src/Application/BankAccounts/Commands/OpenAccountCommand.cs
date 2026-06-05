using Sparkasse.Application.Common.Models;

namespace Sparkasse.Application.BankAccounts.Commands;

public record OpenAccountCommand(decimal InitialBalance) : IRequest<Result<Guid>>;
