using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.ViewModels;

public sealed class AddTaskWindowViewModel : INotifyPropertyChanged
{
    private string _taskTitle = string.Empty;
    private string _taskDescription = string.Empty;
    private DateTimeOffset? _dateOn = DateTimeOffset.Now;
    private string _taskStateString = string.Empty;
    private List<string> _groupNames = new();
    private List<string> _userNames = new();

    public string TaskTitle
    {
        get => _taskTitle;
        set
        {
            if (_taskTitle != value)
            {
                _taskTitle = value;
                OnPropertyChanged(nameof(TaskTitle));
            }
        }
    }

    public string TaskDescription
    {
        get => _taskDescription;
        set
        {
            if (_taskDescription != value)
            {
                _taskDescription = value;
                OnPropertyChanged(nameof(TaskDescription));
            }
        }
    }

    public DateTimeOffset? DateOn
    {
        get => _dateOn;
        set
        {
            if (_dateOn != value)
            {
                _dateOn = value;
                OnPropertyChanged(nameof(DateOn));
            }
        }
    }

    public string TaskStateString
    {
        get => _taskStateString;
        set
        {
            if (_taskStateString != value && value != "" && value != string.Empty)
            {
                _taskStateString = value;
                OnPropertyChanged(nameof(TaskStateString));
            }
        }
    }

    public List<string> GroupNames
    {
        get => _groupNames;
        set
        {
            if (_groupNames != value)
            {
                _groupNames = value;
                OnPropertyChanged(nameof(GroupNames));
            }
        }
    }
    
    public List<string> UserNames
    {
        get => _userNames;
        set
        {
            if (_userNames != value)
            {
                _userNames = value;
                OnPropertyChanged(nameof(UserNames));
            }
        }
    }
    
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    
    public event Action? RequestClose;

    public AddTaskWindowViewModel()
    {
        SaveCommand = new RelayCommand(SaveTask);
        CancelCommand = new RelayCommand(() => RequestClose?.Invoke());
    }
    
    private async void SaveTask()
    {
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var groupIds = GroupsOperator.GetIdsFromNames(GroupNames, MainData.Groups);

            var userIds = UsersOperator.GetIdsFromNames(UserNames, MainData.Users);

            var due = DateOnly.FromDateTime(DateOn?.DateTime ?? DateTime.Now);

            var taskState = TaskState.Pending;
            try
            {
                taskState = StateConverter.StringToState(TaskStateString);
            }
            catch (Exception)
            {
                var errorWindow = new Views.ErrorWindow("Invalid Task State provided.");
                await errorWindow.ShowDialog(desktop.MainWindow!);
                return;
            }

            var id = TasksOperator.GetNextTaskId();

            if (string.IsNullOrEmpty(TaskTitle) || string.IsNullOrEmpty(TaskDescription))
            {
                var errorWindow = new Views.ErrorWindow("Title and Description cannot be empty.");
                await errorWindow.ShowDialog(desktop.MainWindow!);
                return;
            }

            if (id < 1)
            {
                var errorWindow = new Views.ErrorWindow("Failed to generate a valid Task ID.");
                await errorWindow.ShowDialog(desktop.MainWindow!);
                return;
            }

            var newTask = new Task(id, TaskTitle, TaskDescription, due, taskState, groupIds, userIds);

            await Dispatcher.UIThread.InvokeAsync(() => MainData.Tasks.Add(newTask));

            foreach (var userId in userIds)
            {
                var user = UsersOperator.GetUserById(userId);
                if (user != null)
                {
                    await Dispatcher.UIThread.InvokeAsync(() => user.TaskIds.Add(id));
                }
            }

            foreach (var groupId in groupIds)
            {
                var group = GroupsOperator.GetGroupById(groupId);
                if (group != null)
                {
                    await Dispatcher.UIThread.InvokeAsync(() => group.TaskIds.Add(id));
                }
            }
            
            AppLogger.Info("New task added: ID: " + newTask.Id);

            RequestClose?.Invoke();
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            AppLogger.Error("Error adding new task: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while saving the task: {ex.Message}");
            await errorWindow.ShowDialog(desktop.MainWindow!);
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}