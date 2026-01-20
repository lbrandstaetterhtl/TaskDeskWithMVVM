using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml;
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
    private string _taskStateString = "";
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
            if (_taskStateString != value)
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
        var groupIds = GroupOperator.GetIdsFromNames(GroupNames, MainData.Groups);
        
        var userIds = UserOperator.GetIdsFromNames(UserNames, MainData.Users);
        
        var due = DateOnly.FromDateTime(DateOn?.DateTime ?? DateTime.Now);
        
        var taskState = new Task().GetTaskStateFromString(TaskStateString);

        var newTask = new Task(MainData.Tasks.Count, TaskTitle, TaskDescription, due, taskState, groupIds, userIds);
        
        await Dispatcher.UIThread.InvokeAsync(() => MainData.Tasks.Add(newTask));
        
        RequestClose?.Invoke();
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}