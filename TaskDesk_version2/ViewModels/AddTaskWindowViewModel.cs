using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.ViewModels;

public class AddTaskWindowViewModel : INotifyPropertyChanged
{
    private string _taskTitle = string.Empty;
    private string _taskDescription = string.Empty;
    private DateOnly _dateOn = DateOnly.MinValue;
    private TaskState _taskState = TaskState.Pending;
    private List<string> _groupNames = new();

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

    public DateOnly DateOn
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

    public TaskState TaskState
    {
        get => _taskState;
        set
        {
            if (_taskState != value)
            {
                _taskState = value;
                OnPropertyChanged(nameof(TaskState));
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
    
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    
    public event Action? RequestClose;

    public AddTaskWindowViewModel()
    {
        SaveCommand = new RelayCommand(SaveTask);
        CancelCommand = new RelayCommand(() => RequestClose?.Invoke());
    }
    
    private void SaveTask()
    {
        var groupIds = GroupOperator.GetIdsFromNames(GroupNames, MainData.Groups);

        var newTask = new Task
        {
            Title = TaskTitle,
            Description = TaskDescription,
            DueDate = DateOn,
            State = TaskState,
            GroupIds = groupIds
        };
        
        MainData.Tasks.Add(newTask);
        
        RequestClose?.Invoke();
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}