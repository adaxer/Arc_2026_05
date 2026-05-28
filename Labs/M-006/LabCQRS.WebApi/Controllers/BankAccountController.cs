using LabCQRS.WebApi.CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LabCQRS.WebApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class BankAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> OpenAccount([FromBody] OpenAccountCommand command)
    {
        try
        {
            var accountId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBalance), new { accountId }, new { AccountId = accountId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{accountId:guid}/deposit")]
    public async Task<IActionResult> Deposit(Guid accountId, [FromBody] DepositCommand command)
    {
        if (command.AccountId != accountId)
            return BadRequest("Account ID in the URL and body must match.");

        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{accountId:guid}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid accountId, [FromBody] WithdrawCommand command)
    {
        if (command.AccountId != accountId)
            return BadRequest("Account ID in the URL and body must match.");
        try
        {

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{accountId:guid}/close")]
    public async Task<IActionResult> CloseAccount(Guid accountId)
    {
        var result = await _mediator.Send(new CloseAccountCommand(accountId));
        return Ok(result);
    }

    [HttpGet("{accountId:guid}/balance")]
    public async Task<ActionResult<decimal>> GetBalance(Guid accountId)
    {
        var result = await _mediator.Send(new GetBalanceQuery(accountId));
        return Ok(result);
    }

    [HttpGet("{accountId:guid}/statement")]
    public async Task<ActionResult<List<BankAccountEvent>>> GetStatement(Guid accountId)
    {
        var result = await _mediator.Send(new GetStatementQuery(accountId));
        return Ok(result);
    }
}
