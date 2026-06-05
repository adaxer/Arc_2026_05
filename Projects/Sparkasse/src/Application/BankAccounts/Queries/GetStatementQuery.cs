using Sparkasse.Application.BankAccounts.Queries;
using Sparkasse.Application.Common.Models;

namespace Sparkasse.Application.BankAccounts.Queries;

public record GetStatementQuery(Guid AccountId) : IRequest<Result<IEnumerable<BankAccountValue?>>>;
