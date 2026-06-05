using System.Collections.ObjectModel;
using ADaxer.MvvmNav.Abstractions.Navigation;
using ADaxer.MvvmNav.Core.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sparkasse.Client.Common.Services;

namespace Sparkasse.Client.Common.ViewModels;

// ViewModel-specific wrapper for TodoListDto with ObservableCollection
public partial class TodoListViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _colour = "#1976d2";

    public ObservableCollection<TodoItemViewModel> Items { get; } = [];

    public static TodoListViewModel FromDto(TodoListDto dto)
    {
        var vm = new TodoListViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Colour = dto.Colour ?? "#1976d2"
        };

        foreach (var item in dto.Items)
        {
            vm.Items.Add(TodoItemViewModel.FromDto(item));
        }

        return vm;
    }
}

// ViewModel-specific wrapper for TodoItemDto
public partial class TodoItemViewModel : ObservableObject
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

    public Action<TodoItemViewModel>? OnDelete { get; set; }

    [RelayCommand]
    private void Delete()
    {
        OnDelete?.Invoke(this);
    }

    public static TodoItemViewModel FromDto(TodoItemDto dto)
    {
        return new TodoItemViewModel
        {
            Id = dto.Id,
            ListId = dto.ListId,
            Title = dto.Title,
            Done = dto.Done,
            Priority = dto.Priority,
            Note = dto.Note
        };
    }
}

/// <summary>
/// Todos module. Manages todo lists and items via WebApi.
/// </summary>
public partial class TodosViewModel : ViewModelBase, INavigationAware
{
    private readonly ITodoService _todoService;

    [ObservableProperty]
    private string _newItemTitle = string.Empty;

    [ObservableProperty]
    private TodoListViewModel? _selectedList;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    public ObservableCollection<TodoListViewModel> Lists { get; } = [];

    public TodosViewModel(ITodoService todoService)
    {
        _todoService = todoService;
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
        ErrorMessage = null;
        try
        {
            var todosVm = await _todoService.GetTodosAsync();

            Lists.Clear();
            foreach (var list in todosVm.Lists)
            {
                var listVm = TodoListViewModel.FromDto(list);

                // Wire up delete handler for each item
                foreach (var item in listVm.Items)
                {
                    item.OnDelete = async (i) => await DeleteItem(i);
                }

                Lists.Add(listVm);
            }

            SelectedList = Lists.FirstOrDefault();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fehler beim Laden der Todo-Listen: {ex.Message}";
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
            ErrorMessage = null;
            try
            {
                var newItemId = await _todoService.CreateTodoItemAsync(SelectedList.Id, NewItemTitle);

                var newItem = new TodoItemViewModel
                {
                    Id = newItemId,
                    ListId = SelectedList.Id,
                    Title = NewItemTitle,
                    Done = false
                };
                newItem.OnDelete = async (item) => await DeleteItem(item);

                SelectedList.Items.Add(newItem);
                NewItemTitle = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler beim Hinzufügen: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    [RelayCommand]
    private async Task ToggleItem(TodoItemViewModel item)
    {
        ErrorMessage = null;
        try
        {
            // Toggle the Done state
            item.Done = !item.Done;

            // Update via API
            await _todoService.UpdateTodoItemAsync(item.Id, item.Title, item.Done);
        }
        catch (Exception ex)
        {
            // Revert on error
            item.Done = !item.Done;
            ErrorMessage = $"Fehler beim Aktualisieren: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task DeleteItem(TodoItemViewModel item)
    {
        if (SelectedList != null)
        {
            IsLoading = true;
            ErrorMessage = null;
            try
            {
                await _todoService.DeleteTodoItemAsync(item.Id);
                SelectedList.Items.Remove(item);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler beim Löschen: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    [RelayCommand]
    private void SelectList(TodoListViewModel list)
    {
        SelectedList = list;
    }
}
