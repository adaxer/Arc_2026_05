using Microsoft.AspNetCore.Http.HttpResults;
using Sparkasse.Application.BankAccounts.Commands;
using Sparkasse.Application.BankAccounts.Queries;

namespace Sparkasse.Web.Endpoints;

public class BankAccounts : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization("CookieOrBearer");

        groupBuilder.MapPost(OpenBankAccount);
        groupBuilder.MapPost("{accountId}/deposit", Deposit);
        groupBuilder.MapPost("{accountId}/withdraw", Withdraw);
        groupBuilder.MapDelete("{accountId}/close", CloseAccount);
        groupBuilder.MapGet("{accountId}/balance", GetBalance);
        groupBuilder.MapGet("{accountId}/statement", GetStatement);
    }

    [EndpointSummary("Open a new Bank Account")]
    [EndpointDescription("Opens a new bank account using the provided details and returns the ID of the created account.")]
    public static async Task<Results<Created<Guid>, BadRequest<string[]>>> OpenBankAccount(
        ISender sender, 
        OpenAccountCommand command)
    {
        var result = await sender.Send(command);

        if (!result.Succeeded)
            return TypedResults.BadRequest(result.Errors);

        return TypedResults.Created($"/{nameof(BankAccounts)}/{result.Value}", result.Value);
    }

    [EndpointSummary("Deposit money into account")]
    [EndpointDescription("Deposits the specified amount into the bank account and returns the new balance.")]
    public static async Task<Results<Ok<decimal>, BadRequest<string[]>>> Deposit(
        ISender sender, 
        Guid accountId,
        decimal amount)
    {
        var command = new DepositCommand(accountId, amount);
        var result = await sender.Send(command);

        if (!result.Succeeded)
            return TypedResults.BadRequest(result.Errors);

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("Withdraw money from account")]
    [EndpointDescription("Withdraws the specified amount from the bank account and returns the new balance.")]
    public static async Task<Results<Ok<decimal>, BadRequest<string[]>>> Withdraw(
        ISender sender, 
        Guid accountId,
        decimal amount)
    {
        var command = new WithdrawCommand(accountId, amount);
        var result = await sender.Send(command);

        if (!result.Succeeded)
            return TypedResults.BadRequest(result.Errors);

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("Close bank account")]
    [EndpointDescription("Closes the bank account and returns the final balance.")]
    public static async Task<Results<Ok<decimal>, BadRequest<string[]>>> CloseAccount(
        ISender sender, 
        Guid accountId)
    {
        var command = new CloseAccountCommand(accountId);
        var result = await sender.Send(command);

        if (!result.Succeeded)
            return TypedResults.BadRequest(result.Errors);

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("Get account balance")]
    [EndpointDescription("Retrieves the current balance of the specified bank account.")]
    public static async Task<Results<Ok<decimal>, BadRequest<string[]>>> GetBalance(
        ISender sender, 
        Guid accountId)
    {
        var query = new GetBalanceQuery(accountId);
        var result = await sender.Send(query);

        if (!result.Succeeded)
            return TypedResults.BadRequest(result.Errors);

        return TypedResults.Ok(result.Value);
    }

    [EndpointSummary("Get account statement")]
    [EndpointDescription("Retrieves the transaction history for the specified bank account.")]
    public static async Task<Results<Ok<IEnumerable<BankAccountValue?>>, BadRequest<string[]>>> GetStatement(
        ISender sender, 
        Guid accountId)
    {
        var query = new GetStatementQuery(accountId);
        var result = await sender.Send(query);

        if (!result.Succeeded)
            return TypedResults.BadRequest(result.Errors);

        return TypedResults.Ok(result.Value);
    }
}
