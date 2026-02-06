using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Task> _tasks = new ObservableCollection<Task>();

    public ObservableCollection<Task> Tasks
    {
        get => _tasks;
        set
        {
            if (_tasks != value)
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }
    }

    public MainWindowViewModel()
    {
        Tasks = MainData.Tasks;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public static async void OnAddTaskClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var addTaskWindow = new Views.AddTaskWindow();
            await addTaskWindow.ShowDialog(desktop.Windows[0]);
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error opening Add Task window: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while opening the Add Task window. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static void OnOpenTaskClick(Task task)
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        var taskWindow = new Views.OpenTaskWindow(task);
        taskWindow.Show();
        taskWindow.ShowInTaskbar = true;
    }

    public static async void OnAddUserClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var addUserWindow = new Views.AddUserWindow();
            await addUserWindow.ShowDialog(desktop.Windows[0]);
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error opening Add User window: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while opening the Add User window. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnAddGroupClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var addGroupWindow = new Views.AddGroupWindow();
            await addGroupWindow.ShowDialog(desktop.Windows[0]);
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error opening Add Group window." + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while opening the Add Group window. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnDeleteTaskClick(Task task)
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow = new Views.InfoWindow("Are you sure that you want to delete this task?", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result)
            {
                return;
            }

            var infoWindow = new Views.InfoWindow("Task has been deleted.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            MainData.Tasks.Remove(task);

            foreach (var userId in task.UserIds)
            {
                var user = UsersOperator.GetUserById(userId);
                if (user !=  null && user.TaskIds.Contains(task.Id))
                {
                    user.TaskIds.Remove(task.Id);
                }
            }

            foreach (var groupId in task.GroupIds)
            {
                var group = GroupsOperator.GetGroupById(groupId);
                if (group != null && group.TaskIds.Contains(task.Id))
                {
                    group.TaskIds.Remove(task.Id);
                }
            }
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Info("Error deleting task: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while deleting the task. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnManageUsersClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            var manageUsersWindow = new Views.ManageUsersWindow(MainData.Users[0]);
            await manageUsersWindow.ShowDialog(desktop.Windows[0]);
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error opening Manage Users window: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while opening the Manage Users window. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnClearAllTasksClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow = new Views.InfoWindow("Are you sure you want to clear all tasks? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result)
            {
                return;
            }

            var infoWindow = new Views.InfoWindow("All tasks have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            MainData.Tasks.Clear();

            foreach (var user in MainData.Users)
            {
                user.TaskIds.Clear();
            }

            foreach (var group in MainData.Groups)
            {
                group.TaskIds.Clear();
            }
            
            AppLogger.Info("All tasks have been cleared.");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error clearing tasks: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while clearing all tasks. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnClearAllUsersClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow = new Views.InfoWindow("Are you sure you want to clear all users? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result)
            {
                return;
            }

            var infoWindow = new Views.InfoWindow("All users have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            MainData.Users.Clear();

            foreach (var group in MainData.Groups)
            {
                group.UserIds.Clear();
            }

            foreach (var task in MainData.Tasks)
            {
                task.UserIds.Clear();
            }
            
            AppLogger.Info("All users have been cleared.");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error clearing users: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while clearing users. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnClearAllGroupsClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow = new Views.InfoWindow("Are you sure you want to clear all groups? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result)
            {
                return;
            }

            var infoWindow = new Views.InfoWindow("All groups have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            MainData.Groups.Clear();

            foreach (var user in MainData.Users)
            {
                user.GroupIds.Clear();
            }

            foreach (var task in MainData.Tasks)
            {
                task.GroupIds.Clear();
            }
            
            AppLogger.Info("All groups have been cleared.");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error clearing groups: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while clearing groups. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnClearCompletedTasksClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow = new Views.InfoWindow("Are you sure you want to clear all completed tasks? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result)
            {
                return;
            }

            var infoWindow = new Views.InfoWindow("All completed tasks have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            for (int i = MainData.Tasks.Count - 1; i >= 0; i--)
            {
                if (MainData.Tasks[i].State == TaskState.Completed)
                {
                    var task = MainData.Tasks[i];

                    foreach (var userId in task.UserIds)
                    {
                        var user = UsersOperator.GetUserById(userId);
                        if (user !=  null && user.TaskIds.Contains(task.Id))
                        {
                            user.TaskIds.Remove(task.Id);
                        }
                    }

                    foreach (var groupId in task.GroupIds)
                    {
                        var group = GroupsOperator.GetGroupById(groupId);
                        if (group != null && group.TaskIds.Contains(task.Id))
                        {
                            group.TaskIds.Remove(task.Id);
                        }
                    }

                    MainData.Tasks.RemoveAt(i);
                }
            }
            
            AppLogger.Info("All completed tasks cleared.");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error clearing completed tasks: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while clearing completed tasks. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnClearCancelledTasksClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow = new Views.InfoWindow("Are you sure you want to clear all cancelled tasks? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result)
            {
                return;
            }

            var infoWindow = new Views.InfoWindow("All cancelled tasks have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            for (int i = MainData.Tasks.Count - 1; i >= 0; i--)
            {
                if (MainData.Tasks[i].State == TaskState.Cancelled)
                {
                    var task = MainData.Tasks[i];

                    foreach (var userId in task.UserIds)
                    {
                        var user = UsersOperator.GetUserById(userId);
                        if (user !=  null && user.TaskIds.Contains(task.Id))
                        {
                            user.TaskIds.Remove(task.Id);
                        }
                    }

                    foreach (var groupId in task.GroupIds)
                    {
                        var group = GroupsOperator.GetGroupById(groupId);
                        if (group != null && group.TaskIds.Contains(task.Id))
                        {
                            group.TaskIds.Remove(task.Id);
                        }
                    }

                    MainData.Tasks.RemoveAt(i);
                }
            }
            
            AppLogger.Info("All cancelled task cleared");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error clearing cancelled tasks: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while clearing cancelled tasks. {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public static async void OnClearOverdueTasksClick()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var confirmWindow = new Views.InfoWindow("Are you sure you want to clear all overdue tasks? This action cannot be undone.", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result)
            {
                return;
            }

            var infoWindow = new Views.InfoWindow("All overdue tasks have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);

            for (int i = MainData.Tasks.Count - 1; i >= 0; i--)
            {
                if (MainData.Tasks[i].DueDate < DateOnly.FromDateTime(DateTime.Now) &&
                    MainData.Tasks[i].State != TaskState.Completed &&
                    MainData.Tasks[i].State != TaskState.Cancelled)
                {
                    var task = MainData.Tasks[i];

                    foreach (var userId in task.UserIds)
                    {
                        var user = UsersOperator.GetUserById(userId);
                        if (user !=  null && user.TaskIds.Contains(task.Id))
                        {
                            user.TaskIds.Remove(task.Id);
                        }
                    }

                    foreach (var groupId in task.GroupIds)
                    {
                        var group = GroupsOperator.GetGroupById(groupId);
                        if (group != null && group.TaskIds.Contains(task.Id))
                        {
                            group.TaskIds.Remove(task.Id);
                        }
                    }

                    MainData.Tasks.RemoveAt(i);
                }
            }
            
            AppLogger.Info("All overdue task cleared");
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error clearing overdue tasks: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while clearing overdue tasks. {ex.Message}");
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

    public static async void OnSaveCurrentUserClick()
    {
        
        try
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            if (!MainData.Settings.SavedUserIds.Contains(MainData.CurrentUser.Id))
            {
                MainData.Settings.SavedUserIds.Add(MainData.CurrentUser.Id);
            
                var infoWindow = new Views.InfoWindow("Current user has been saved.");
                await infoWindow.ShowDialog(desktop.Windows[0]);
                AppLogger.Info("Current user saved: " + MainData.CurrentUser.FullName);
            }
        }
        catch (Exception ex)
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            var errorWindow = new Views.ErrorWindow("Error saving current user: " + ex.Message);
            await errorWindow.ShowDialog(desktop.Windows[0]);
            AppLogger.Error("Error saving current user: " + ex.Message);
        }
    }
    
    public static async void OnClearSavedUsersClick()
    {
        try
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            MainData.Settings.SavedUserIds.Clear();
            var infoWindow = new Views.InfoWindow("All saved users have been cleared.");
            await infoWindow.ShowDialog(desktop.Windows[0]);
            AppLogger.Info("All saved users cleared.");
            
        }
        catch (Exception ex)
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            var errorWindow = new Views.ErrorWindow("Error clearing saved users: " + ex.Message);
            await errorWindow.ShowDialog(desktop.Windows[0]);
            AppLogger.Error("Error clearing saved users: " + ex.Message);
        }
    }
    
    public static async void OnLogoutClick()
    {
        try
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            var confirmWindow = new Views.InfoWindow("Are you sure you want to logout?", true);
            var result = await confirmWindow.ShowDialogAsync(desktop.Windows[0]);

            if (!result)
            {
                return;
            }
            
            var loginWindow = new Views.LoginWindow();
            loginWindow.Show();
            AppLogger.Info("User logged out: " + MainData.CurrentUser.FullName);
            MainData.CurrentUser = null;
            desktop.Windows[0].Close();
        }
        catch (Exception ex)
        {
            if (App.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            var errorWindow = new Views.ErrorWindow("Error during logout: " + ex.Message);
            await errorWindow.ShowDialog(desktop.Windows[0]);
            AppLogger.Error("Error during logout: " + ex.Message);
        }
    }
}
