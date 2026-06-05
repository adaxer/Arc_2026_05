using Sparkasse.Application.Common.Models;

namespace Sparkasse.Application.BankAccounts.Commands;

public class BankAccountCommandHandler : 
    IRequestHandler<OpenAccountCommand, Result<Guid>>,
    IRequestHandler<CloseAccountCommand, Result<decimal>>,
    IRequestHandler<DepositCommand, Result<decimal>>,
    IRequestHandler<WithdrawCommand, Result<decimal>>
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountCommandHandler(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    public Task<Result<Guid>> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
    {
        return _bankAccountService.OpenAccountAsync(request.InitialBalance, cancellationToken);
    }

    public Task<Result<decimal>> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        return _bankAccountService.CloseAccountAsync(request.AccountId, cancellationToken);
    }

    public Task<Result<decimal>> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        return _bankAccountService.DepositAsync(request.AccountId, request.Amount, cancellationToken);
    }

    public Task<Result<decimal>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        return _bankAccountService.WithdrawAsync(request.AccountId, request.Amount, cancellationToken);
    }
}

