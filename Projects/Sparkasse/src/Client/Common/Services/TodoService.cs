using System.Net.Http.Json;

namespace Sparkasse.Client.Common.Services;

public interface ITodoService
{
    Task<TodosVm> GetTodosAsync();
    Task<int> CreateTodoListAsync(string title, string? colour = null);
    Task UpdateTodoListAsync(int id, string title, string? colour = null);
    Task DeleteTodoListAsync(int id);
    Task<int> CreateTodoItemAsync(int listId, string title);
    Task UpdateTodoItemAsync(int id, string title, bool done);
    Task DeleteTodoItemAsync(int id);
}

public class TodoService : ITodoService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TodoService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TodosVm> GetTodosAsync()
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var response = await client.GetAsync("/api/TodoLists");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodosVm>() 
            ?? new TodosVm { Lists = [], PriorityLevels = [], Colours = [] };
    }

    public async Task<int> CreateTodoListAsync(string title, string? colour = null)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var command = new CreateTodoListCommand { Title = title, Colour = colour };
        var response = await client.PostAsJsonAsync("/api/TodoLists", command);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task UpdateTodoListAsync(int id, string title, string? colour = null)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var command = new UpdateTodoListCommand { Id = id, Title = title, Colour = colour };
        var response = await client.PutAsJsonAsync($"/api/TodoLists/{id}", command);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteTodoListAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var response = await client.DeleteAsync($"/api/TodoLists/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<int> CreateTodoItemAsync(int listId, string title)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var command = new CreateTodoItemCommand { ListId = listId, Title = title };
        var response = await client.PostAsJsonAsync("/api/TodoItems", command);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task UpdateTodoItemAsync(int id, string title, bool done)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var command = new UpdateTodoItemCommand { Id = id, Title = title, Done = done };
        var response = await client.PutAsJsonAsync($"/api/TodoItems/{id}", command);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteTodoItemAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("WebApi");
        var response = await client.DeleteAsync($"/api/TodoItems/{id}");
        response.EnsureSuccessStatusCode();
    }
}

// DTOs matching the API contracts
public class TodosVm
{
    public List<TodoListDto> Lists { get; set; } = [];
    public List<LookupDto> PriorityLevels { get; set; } = [];
    public List<ColourDto> Colours { get; set; } = [];
}

public class TodoListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Colour { get; set; }
    public List<TodoItemDto> Items { get; set; } = [];
}

public class TodoItemDto
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Done { get; set; }
    public int Priority { get; set; }
    public string? Note { get; set; }
}

public class LookupDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class ColourDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

// Command DTOs for API calls
public class CreateTodoListCommand
{
    public string Title { get; set; } = string.Empty;
    public string? Colour { get; set; }
}

public class UpdateTodoListCommand
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Colour { get; set; }
}

public class CreateTodoItemCommand
{
    public int ListId { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class UpdateTodoItemCommand
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Done { get; set; }
}
