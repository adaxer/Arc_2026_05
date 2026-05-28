using MediatR;

namespace LabCQRS.WebApi.CQRS;

public record OpenAccountCommand(decimal InitialBalance) : IRequest<Guid>;

public record CloseAccountCommand(Guid AccountId) : IRequest<decimal>;

public record DepositCommand(Guid AccountId, decimal Amount) : IRequest<decimal>;

public record WithdrawCommand(Guid AccountId, decimal Amount) : IRequest<decimal>;
