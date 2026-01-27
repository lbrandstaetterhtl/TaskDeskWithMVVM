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

    public static async void OnAddTaskClick(object? sender, RoutedEventArgs e)
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        var addTaskWindow = new Views.AddTaskWindow();
        await addTaskWindow.ShowDialog(desktop.Windows[0]);
    }

    public static async void OnOpenTaskClick(Task task)
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        var taskWindow = new Views.OpenTaskWindow(task);
        taskWindow.Show();
        taskWindow.ShowInTaskbar = true;
    }
    
    public static async void OnAddUserClick(object? sender, RoutedEventArgs e)
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        var addUserWindow = new Views.AddUserWindow();
        await addUserWindow.ShowDialog(desktop.Windows[0]);
    }
    
    public static async void OnAddGroupClick(object? sender, RoutedEventArgs e)
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        var addGroupWindow = new Views.AddGroupWindow();
        await addGroupWindow.ShowDialog(desktop.Windows[0]);
    }
    
    public static async void OnDeleteTaskClick(Task task)
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
    
    public static async void OnManageUsersClick(object? sender, RoutedEventArgs e)
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        var user = MainData.Users[0];
        
        var manageUsersWindow = new Views.ManageUsersWindow(user);
        await manageUsersWindow.ShowDialog(desktop.Windows[0]);
    }
    
    public static async void OnClearAllTasksClick(object? sender, RoutedEventArgs e)
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
    }
    
    public static async void OnClearAllUsersClick(object? sender, RoutedEventArgs e)
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
    }
    
    public static async void OnClearAllGroupsClick(object? sender, RoutedEventArgs e)
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
    }
}