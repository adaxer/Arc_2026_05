using System.Collections.ObjectModel;
using ADaxer.MvvmNav.Abstractions.Navigation;
using ADaxer.MvvmNav.Core.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Sparkasse.Client.Common.ViewModels;

public partial class TodoListDto : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _colour = "#1976d2";

    public ObservableCollection<TodoItemDto> Items { get; } = [];
}

public partial class TodoItemDto : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _listId;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private bool _done;

    [ObservableProperty]
    private int _priority;

    [ObservableProperty]
    private string? _note;

    public Action<TodoItemDto>? OnDelete { get; set; }

    [RelayCommand]
    private void Delete()
    {
        OnDelete?.Invoke(this);
    }
}

/// <summary>
/// Todos module. Manages todo lists and items.
/// </summary>
public partial class TodosViewModel : ViewModelBase, INavigationAware
{
    [ObservableProperty]
    private string _newItemTitle = string.Empty;

    [ObservableProperty]
    private TodoListDto? _selectedList;

    [ObservableProperty]
    private bool _isLoading;

    public ObservableCollection<TodoListDto> Lists { get; } = [];

    public TodosViewModel()
    {
        Title = "Todo-Listen";
    }

    public async Task OnNavigatedToAsync(NavigationParameters context)
    {
        await LoadTodoLists();
    }

    [RelayCommand]
    private async Task LoadTodoLists()
    {
        IsLoading = true;
        try
        {
            // TODO: Load todos from API
            await Task.Delay(300); // Simulate API call

            Lists.Clear();

            var list1 = new TodoListDto { Id = 1, Title = "Persönlich", Colour = "#2196f3" };
            var item1 = new TodoItemDto { Id = 1, ListId = 1, Title = "Einkaufen gehen", Done = false };
            item1.OnDelete = async (item) => await DeleteItem(item);
            list1.Items.Add(item1);

            var item2 = new TodoItemDto { Id = 2, ListId = 1, Title = "Code Review", Done = true };
            item2.OnDelete = async (item) => await DeleteItem(item);
            list1.Items.Add(item2);

            var list2 = new TodoListDto { Id = 2, Title = "Arbeit", Colour = "#4caf50" };
            var item3 = new TodoItemDto { Id = 3, ListId = 2, Title = "Meeting vorbereiten", Done = false };
            item3.OnDelete = async (item) => await DeleteItem(item);
            list2.Items.Add(item3);

            Lists.Add(list1);
            Lists.Add(list2);

            SelectedList = Lists.FirstOrDefault();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task AddItem()
    {
        if (!string.IsNullOrWhiteSpace(NewItemTitle) && SelectedList != null)
        {
            IsLoading = true;
            try
            {
                // TODO: Call API to create item
                await Task.Delay(100);

                var newItem = new TodoItemDto 
                { 
                    Id = SelectedList.Items.Count + 1,
                    ListId = SelectedList.Id,
                    Title = NewItemTitle, 
                    Done = false 
                };
                newItem.OnDelete = async (item) => await DeleteItem(item);

                SelectedList.Items.Add(newItem);
                NewItemTitle = string.Empty;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    [RelayCommand]
    private async Task ToggleItem(TodoItemDto item)
    {
        // TODO: Call API to update item
        await Task.Delay(100);
    }

    [RelayCommand]
    private async Task DeleteItem(TodoItemDto item)
    {
        if (SelectedList != null)
        {
            IsLoading = true;
            try
            {
                // TODO: Call API to delete item
                await Task.Delay(100);

                SelectedList.Items.Remove(item);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    [RelayCommand]
    private void SelectList(TodoListDto list)
    {
        SelectedList = list;
    }
}
