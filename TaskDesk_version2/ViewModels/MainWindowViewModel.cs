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
        await addTaskWindow.ShowDialog(desktop.MainWindow!);
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
        await addUserWindow.ShowDialog(desktop.MainWindow!);
    }
    
    public static async void OnAddGroupClick(object? sender, RoutedEventArgs e)
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        var addGroupWindow = new Views.AddGroupWindow();
        await addGroupWindow.ShowDialog(desktop.MainWindow!);
    }
}