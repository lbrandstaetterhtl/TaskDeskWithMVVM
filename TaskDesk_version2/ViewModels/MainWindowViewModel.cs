using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using TaskDesk_version2.Models;
using TaskDesk_version2.Views;

namespace TaskDesk_version2.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Task> Tasks => MainData.Tasks;
    private ObservableCollection<Task> _filteredTasks = new();
    public List<string> StateOptions => StateConverter.GetAllStateStrings();
    public List<string> AllUsers => UsersOperator.GetAllUserFullNames();
    public List<string> AllGroups => GroupsOperator.GetAllGroupNames();
    private string _searchText = string.Empty;
    private ObservableCollection<string> _stateFilter = new();
    private ObservableCollection<string> _userFilter = new();
    private ObservableCollection<string> _groupFilter = new();
    private DateTimeOffset _searchDueDate = DateTimeOffset.Now;
    private bool _isDateFilterEnabled;

    public MainWindowViewModel()
    {
        FilteredTasks = new ObservableCollection<Task>(MainData.Tasks);
        LogoutCommand = new RelayCommand(OnLogoutClick);
        ApplyFiltersCommand = new RelayCommand(ApplyFilters);
        ClearFiltersCommand = new RelayCommand(ClearFilters);
        
        // Subscribe to MainData.Tasks CollectionChanged event
        MainData.Tasks.CollectionChanged += OnMainDataTasksChanged;
    }
    
    private void OnMainDataTasksChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Re-apply filters when MainData.Tasks changes
        RefreshFilteredTasks();
    }
    
    private void RefreshFilteredTasks()
    {
        // Check if any filters are active
        bool hasActiveFilters = !string.IsNullOrWhiteSpace(SearchText) ||
                                IsDateFilterEnabled ||
                                UserFilter.Count > 0 ||
                                GroupFilter.Count > 0 ||
                                StateFilter.Count > 0;
        
        if (hasActiveFilters)
        {
            // Re-apply current filters
            ApplyFilters();
        }
        else
        {
            // No filters active, show all tasks
            FilteredTasks = new ObservableCollection<Task>(MainData.Tasks);
        }
    }
    
    public ObservableCollection<Task> FilteredTasks
    {
        get => _filteredTasks;
        set
        {
            if (_filteredTasks != value)
            {
                _filteredTasks = value;
                OnPropertyChanged(nameof(FilteredTasks));
            }
        }
    }

    public DateTimeOffset SearchDueDate
    {
        get => _searchDueDate;
        set
        {
            if (_searchDueDate != value)
            {
                _searchDueDate = value;
                OnPropertyChanged(nameof(SearchDueDate));
            }
        }
    }

    public ObservableCollection<string> StateFilter
    {
        get => _stateFilter;
        set
        {
            if (_stateFilter != value)
            {
                _stateFilter = value;
                OnPropertyChanged(nameof(StateFilter));
            }
        }
    }
    
    public ObservableCollection<string> UserFilter
    {
        get => _userFilter;
        set
        {
            if (_userFilter != value)
            {
                _userFilter = value;
                OnPropertyChanged(nameof(UserFilter));
            }
        }
    }
    
    public ObservableCollection<string> GroupFilter
    {
        get => _groupFilter;
        set
        {
            if (_groupFilter != value)
            {
                _groupFilter = value;
                OnPropertyChanged(nameof(GroupFilter));
            }
        }
    }
    
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
    }

    public bool IsDateFilterEnabled
    {
        get => _isDateFilterEnabled;
        set
        {
            if (_isDateFilterEnabled != value)
            {
                _isDateFilterEnabled = value;
                OnPropertyChanged(nameof(IsDateFilterEnabled));
            }
        }
    }

    public ICommand LogoutCommand { get; set; }
    public ICommand ApplyFiltersCommand { get; set; }
    
    public ICommand ClearFiltersCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async void OnAddTaskClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var addTaskWindow = new AddTaskWindow();
            await addTaskWindow.ShowDialog(desktop.Windows[0]);
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error opening Add Task window: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while opening the Add Task window. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static void OnOpenTaskClick(Task task)
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        var taskWindow = new OpenTaskWindow(task);
        taskWindow.Show();
        taskWindow.ShowInTaskbar = true;
    }

    public async void OnAddUserClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var addUserWindow = new AddUserWindow();
            await addUserWindow.ShowDialog(desktop.Windows[0]);
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error opening Add User window: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while opening the Add User window. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public async void OnAddGroupClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var addGroupWindow = new AddGroupWindow();
            await addGroupWindow.ShowDialog(desktop.Windows[0]);
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error opening Add Group window." + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while opening the Add Group window. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnDeleteTaskClick(Task task)
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow = new InfoWindow("Are you sure that you want to delete this task?", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result) return;

            var infoWindow = new InfoWindow("Task has been deleted.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            MainData.Tasks.Remove(task);

            foreach (var userId in task.UserIds)
            {
                var user = UsersOperator.GetUserById(userId);
                if (user.TaskIds.Contains(task.Id)) user.TaskIds.Remove(task.Id);
            }

            foreach (var groupId in task.GroupIds)
            {
                var group = GroupsOperator.GetGroupById(groupId);
                if (group.TaskIds.Contains(task.Id)) group.TaskIds.Remove(task.Id);
            }
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Info("Error deleting task: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while deleting the task. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public async void OnManageUsersClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var manageUsersWindow = new ManageUsersWindow(MainData.Users[0]);
            await manageUsersWindow.ShowDialog(desktop.Windows[0]);
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error opening Manage Users window: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while opening the Manage Users window. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public async void OnClearAllTasksClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow =
                new InfoWindow("Are you sure you want to clear all tasks? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result) return;

            var infoWindow = new InfoWindow("All tasks have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            MainData.Tasks.Clear();

            foreach (var user in MainData.Users) user.TaskIds.Clear();

            foreach (var group in MainData.Groups) group.TaskIds.Clear();

            AppLogger.Info("All tasks have been cleared.");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error clearing tasks: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while clearing all tasks. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public async void OnClearAllUsersClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow =
                new InfoWindow("Are you sure you want to clear all users? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result) return;

            var infoWindow = new InfoWindow("All users have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            MainData.Users.Clear();

            foreach (var group in MainData.Groups) group.UserIds.Clear();

            foreach (var task in MainData.Tasks) task.UserIds.Clear();

            AppLogger.Info("All users have been cleared.");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error clearing users: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while clearing users. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public async void OnClearAllGroupsClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow =
                new InfoWindow("Are you sure you want to clear all groups? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result) return;

            var infoWindow = new InfoWindow("All groups have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            MainData.Groups.Clear();

            foreach (var user in MainData.Users) user.GroupIds.Clear();

            foreach (var task in MainData.Tasks) task.GroupIds.Clear();

            AppLogger.Info("All groups have been cleared.");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error clearing groups: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while clearing groups. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public async void OnClearCompletedTasksClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow =
                new InfoWindow("Are you sure you want to clear all completed tasks? This action cannot be undone.",
                    true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result) return;

            var infoWindow = new InfoWindow("All completed tasks have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            for (var i = MainData.Tasks.Count - 1; i >= 0; i--)
                if (MainData.Tasks[i].State == TaskState.Completed)
                {
                    var task = MainData.Tasks[i];

                    foreach (var userId in task.UserIds)
                    {
                        var user = UsersOperator.GetUserById(userId);
                        if (user.TaskIds.Contains(task.Id)) user.TaskIds.Remove(task.Id);
                    }

                    foreach (var groupId in task.GroupIds)
                    {
                        var group = GroupsOperator.GetGroupById(groupId);
                        if (group.TaskIds.Contains(task.Id)) group.TaskIds.Remove(task.Id);
                    }

                    MainData.Tasks.RemoveAt(i);
                }

            AppLogger.Info("All completed tasks cleared.");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error clearing completed tasks: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while clearing completed tasks. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public async void OnClearCancelledTasksClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow =
                new InfoWindow("Are you sure you want to clear all cancelled tasks? This action cannot be undone.",
                    true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result) return;

            var infoWindow = new InfoWindow("All cancelled tasks have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            for (var i = MainData.Tasks.Count - 1; i >= 0; i--)
                if (MainData.Tasks[i].State == TaskState.Cancelled)
                {
                    var task = MainData.Tasks[i];

                    foreach (var userId in task.UserIds)
                    {
                        var user = UsersOperator.GetUserById(userId);
                        if (user.TaskIds.Contains(task.Id)) user.TaskIds.Remove(task.Id);
                    }

                    foreach (var groupId in task.GroupIds)
                    {
                        var group = GroupsOperator.GetGroupById(groupId);
                        if (group.TaskIds.Contains(task.Id)) group.TaskIds.Remove(task.Id);
                    }

                    MainData.Tasks.RemoveAt(i);
                }

            AppLogger.Info("All cancelled task cleared");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error clearing cancelled tasks: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while clearing cancelled tasks. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public async void OnClearOverdueTasksClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow =
                new InfoWindow("Are you sure you want to clear all overdue tasks? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result) return;

            var infoWindow = new InfoWindow("All overdue tasks have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            for (var i = MainData.Tasks.Count - 1; i >= 0; i--)
                if (MainData.Tasks[i].DueDate < DateOnly.FromDateTime(DateTime.Now) &&
                    MainData.Tasks[i].State != TaskState.Completed &&
                    MainData.Tasks[i].State != TaskState.Cancelled)
                {
                    var task = MainData.Tasks[i];

                    foreach (var userId in task.UserIds)
                    {
                        var user = UsersOperator.GetUserById(userId);
                        if (user.TaskIds.Contains(task.Id)) user.TaskIds.Remove(task.Id);
                    }

                    foreach (var groupId in task.GroupIds)
                    {
                        var group = GroupsOperator.GetGroupById(groupId);
                        if (group.TaskIds.Contains(task.Id)) group.TaskIds.Remove(task.Id);
                    }

                    MainData.Tasks.RemoveAt(i);
                }

            AppLogger.Info("All overdue task cleared");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error clearing overdue tasks: " + ex.Message);

            var errorWindow = new ErrorWindow($"An error occurred while clearing overdue tasks. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static void OnChangeThemeClick()
    {
        if (App.Current == null)
            return;

        MainData.Settings.IsThemeDark = !MainData.Settings.IsThemeDark;

        (App.Current as App).SetTheme(MainData.Settings.IsThemeDark);
    }

    public async void OnSaveCurrentUserClick()
    {
        try
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            if (!MainData.Settings.SavedUserIds.Contains(MainData.CurrentUser.Id))
            {
                MainData.Settings.SavedUserIds.Add(MainData.CurrentUser.Id);

                var infoWindow = new InfoWindow("Current user has been saved.");
                await infoWindow.ShowDialog(desktop.Windows[0]);
                AppLogger.Info("Current user saved: " + MainData.CurrentUser.FullName);
            }
        }
        catch (Exception ex)
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var errorWindow = new ErrorWindow("Error saving current user: " + ex.Message);
            await errorWindow.ShowDialog(desktop.Windows[0]);
            AppLogger.Error("Error saving current user: " + ex.Message);
        }
    }

    public async void OnClearSavedUsersClick()
    {
        try
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            MainData.Settings.SavedUserIds.Clear();
            var infoWindow = new InfoWindow("All saved users have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);
            AppLogger.Info("All saved users cleared.");
        }
        catch (Exception ex)
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var errorWindow = new ErrorWindow("Error clearing saved users: " + ex.Message);
            await errorWindow.ShowDialog(desktop.Windows[0]);
            AppLogger.Error("Error clearing saved users: " + ex.Message);
        }
    }

    private static async void OnLogoutClick()
    {
        try
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow = new InfoWindow("Are you sure you want to logout?", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result) return;

            var loginWindow = new LoginWindow();
            loginWindow.Show();
            AppLogger.Info("User logged out: " + MainData.CurrentUser.FullName);
            MainData.CurrentUser = null;
            desktop.Windows[0].Close();
        }
        catch (Exception ex)
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var errorWindow = new ErrorWindow("Error during logout: " + ex.Message);
            await errorWindow.ShowDialog(desktop.Windows[0]);
            AppLogger.Error("Error during logout: " + ex.Message);
        }
    }
    
    private void ApplyFilters()
    {
        bool filtersApplied = false;
        FilteredTasks.Clear();
        
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredTasks.AddRange(TasksOperator.SearchInTasksForString(SearchText, MainData.Tasks));
            filtersApplied = true;
        }

        if (IsDateFilterEnabled)
        {
            FilteredTasks.AddRange(TasksOperator.SearchInTasksForDate(SearchDueDate, MainData.Tasks));
            filtersApplied = true;
        }

        if (UserFilter.Count > 0)
        {
            FilteredTasks.AddRange(TasksOperator.SearchInTasksForUsers(UserFilter, MainData.Tasks));
            filtersApplied = true;
        }

        if (GroupFilter.Count > 0)
        {
            FilteredTasks.AddRange(TasksOperator.SearchInTasksForGroups(GroupFilter, MainData.Tasks));
            filtersApplied = true;
        }

        if (StateFilter.Count > 0)
        {
            FilteredTasks.AddRange(TasksOperator.SearchInTasksForStates(StateFilter, MainData.Tasks));
            filtersApplied = true;
        }
        
        if (!filtersApplied)
        {
            FilteredTasks = new ObservableCollection<Task>(MainData.Tasks);
        }
    }
    
    private void ClearFilters()
    {
        SearchText = string.Empty;
        StateFilter = new ObservableCollection<string>();
        UserFilter = new ObservableCollection<string>();
        GroupFilter = new ObservableCollection<string>();
        IsDateFilterEnabled = false;
        SearchDueDate = DateTimeOffset.Now;
        FilteredTasks = new ObservableCollection<Task>(MainData.Tasks);
    }
}