using System.Net.Http.Json;

namespace Sparkasse.Client.Common.Services;

// Local DTO for API responses - mirrors backend BankAccountValue
public class BankAccountValue
{
    public DateTime Timestamp { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Balance { get; set; }
}

public interface IBankAccountService
{
    Guid? CurrentAccountId { get; }
    Task<Guid> OpenAccountAsync(decimal initialBalance);
    Task<decimal> DepositAsync(Guid accountId, decimal amount);
    Task<decimal> WithdrawAsync(Guid accountId, decimal amount);
    Task<decimal> CloseAccountAsync(Guid accountId);
    Task<decimal> GetBalanceAsync(Guid accountId);
    Task<IEnumerable<BankAccountTransactionDto>> GetStatementAsync(Guid accountId);
}

public class BankAccountService : IBankAccountService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private Guid? _currentAccountId;

    public Guid? CurrentAccountId => _currentAccountId;

    public BankAccountService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Guid> OpenAccountAsync(decimal initialBalance)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var command = new { InitialBalance = initialBalance };
        var response = await client.PostAsJsonAsync("/api/BankAccounts", command);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to open account: {response.StatusCode} - {errorContent}");
        }

        var accountId = await response.Content.ReadFromJsonAsync<Guid>();
        _currentAccountId = accountId; // Store the account ID
        return accountId;
    }

    public async Task<decimal> DepositAsync(Guid accountId, decimal amount)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var response = await client.PostAsync($"/api/BankAccounts/{accountId}/deposit?amount={amount}", null);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to deposit: {response.StatusCode} - {errorContent}");
        }

        return await response.Content.ReadFromJsonAsync<decimal>();
    }

    public async Task<decimal> WithdrawAsync(Guid accountId, decimal amount)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var response = await client.PostAsync($"/api/BankAccounts/{accountId}/withdraw?amount={amount}", null);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to withdraw: {response.StatusCode} - {errorContent}");
        }

        return await response.Content.ReadFromJsonAsync<decimal>();
    }

    public async Task<decimal> CloseAccountAsync(Guid accountId)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var response = await client.DeleteAsync($"/api/BankAccounts/{accountId}/close");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to close account: {response.StatusCode} - {errorContent}");
        }

        var finalBalance = await response.Content.ReadFromJsonAsync<decimal>();
        _currentAccountId = null; // Clear the stored account ID
        return finalBalance;
    }

    public async Task<decimal> GetBalanceAsync(Guid accountId)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var response = await client.GetAsync($"/api/BankAccounts/{accountId}/balance");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to get balance: {response.StatusCode} - {errorContent}");
        }

        return await response.Content.ReadFromJsonAsync<decimal>();
    }

    public async Task<IEnumerable<BankAccountTransactionDto>> GetStatementAsync(Guid accountId)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var response = await client.GetAsync($"/api/BankAccounts/{accountId}/statement");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to get statement: {response.StatusCode} - {errorContent}");
        }

        var statement = await response.Content.ReadFromJsonAsync<IEnumerable<BankAccountValue>>();

        // Convert BankAccountValue to BankAccountTransactionDto
        return statement?.Select(v => new BankAccountTransactionDto
        {
            Date = v.Timestamp,
            Description = v.Amount.HasValue ? (v.Amount.Value >= 0 ? "Einzahlung" : "Auszahlung") : "Unbekannt",
            Amount = v.Amount ?? 0,
            Balance = v.Balance ?? 0
        }) ?? Enumerable.Empty<BankAccountTransactionDto>();
    }
}

// DTO for displaying transactions in the UI
public class BankAccountTransactionDto
{
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
}
