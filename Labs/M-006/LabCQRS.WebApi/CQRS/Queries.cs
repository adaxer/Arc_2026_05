using MediatR;

namespace LabCQRS.WebApi.CQRS;

public record GetBalanceQuery(Guid AccountId) : IRequest<decimal>;

public record GetStatementQuery(Guid AccountId) : IRequest<IEnumerable<BankAccountEvent?>>;
