using System.Collections.ObjectModel;
using System.Dynamic;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.ViewModels;

public class MainWindowViewModel
{
    public ObservableCollection<Task> Tasks { get; }
    
    public MainWindowViewModel()
    {
        Tasks = MainData.Tasks;
    }

    public static async void OnAddTaskClick(object? sender, RoutedEventArgs e)
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        var addTaskWindow = new Views.AddTaskWindow();
        await addTaskWindow.ShowDialog(desktop.MainWindow!);
    }
}