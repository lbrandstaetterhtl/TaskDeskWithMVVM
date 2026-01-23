using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;
using TaskDesk_version2.Views;

namespace TaskDesk_version2.ViewModels;

public class OpenTaskWindowViewModel : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private DateTimeOffset? _dueDate = DateTimeOffset.Now;
    private string _state = string.Empty;
    private ObservableCollection<User> _assignedUsers = new();
    private ObservableCollection<Group> _assignedGroups = new();
    private readonly Task _originalTask;
    private ObservableCollection<User> _allUsers = MainData.Users;
    private ObservableCollection<Group> _allGroups = MainData.Groups;
    
    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }
    
    public ObservableCollection<User> AllUsers
    {
        get => _allUsers;
        set
        {
            if (_allUsers != value)
            {
                _allUsers = value;
                OnPropertyChanged(nameof(AllUsers));
            }
        }
    }
    
    public ObservableCollection<Group> AllGroups
    {
        get => _allGroups;
        set
        {
            if (_allGroups != value)
            {
                _allGroups = value;
                OnPropertyChanged(nameof(AllGroups));
            }
        }
    }
    
    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }
    
    public DateTimeOffset? DueDate
    {
        get => _dueDate;
        set
        {
            if (_dueDate != value)
            {
                _dueDate = value;
                OnPropertyChanged(nameof(DueDate));
            }
        }
    }
    
    public string State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }
    }
    
    public ObservableCollection<User> AssignedUsers
    {
        get => _assignedUsers;
        set
        {
            if (_assignedUsers != value)
            {
                _assignedUsers = value;
                OnPropertyChanged(nameof(AssignedUsers));
            }
        }
    }
    
    public ObservableCollection<Group> AssignedGroups
    {
        get => _assignedGroups;
        set
        {
            if (_assignedGroups != value)
            {
                _assignedGroups = value;
                OnPropertyChanged(nameof(AssignedGroups));
            }
        }
    }
    
    public ICommand SaveTaskCommand { get; }
    public ICommand CloseCommand { get; }
    public Action? RequestClose;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public OpenTaskWindowViewModel(Task task)
    {
        SaveTaskCommand = new RelayCommand(SaveTask);
        CloseCommand = new RelayCommand(() => RequestClose?.Invoke());
        
        Title = task.Title;
        Description = task.Description;
        DueDate = new DateTimeOffset(task.DueDate.ToDateTime(TimeOnly.MinValue));
        State = task.GetTaskStateAsString();
        
        AssignedGroups = GroupsOperator.GetListFromIds(task.GroupIds, MainData.Groups);
        AssignedUsers = UsersOperator.GetListFromIds(task.UserIds, MainData.Users);
        
        _originalTask = task;
    }

    private async void SaveTask()
    {
        if (App.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            return;
        }
        
        if (Title == string.Empty || Description == string.Empty || DueDate == null || State == string.Empty)
        {
            var errorWindow = new ErrorWindow("All fields must be filled out.");
            await errorWindow.ShowDialog(desktop.MainWindow!);
            return;
        }

        TaskState taskState;
        try
        {
            taskState = StateConverter.StringToState(State);
        }
        catch (Exception)
        {
            var errorWindow = new ErrorWindow("Invalid Task State provided.");
            await errorWindow.ShowDialog(desktop.MainWindow!);
            return;
        }
        
        var assignedUserIds = UsersOperator.GetIdsFromList(AssignedUsers, AllUsers);
        var assignedGroupIds = GroupsOperator.GetIdsFromList(AssignedGroups, AllGroups);
        
        var due = DateOnly.FromDateTime(DueDate?.DateTime ?? DateTime.Now);

        var updatedTask = new Task(_originalTask.Id, Title, Description, due, taskState, assignedGroupIds, assignedUserIds);
        
        for (int i = 0; i < MainData.Tasks.Count; i++)
        {
            if (MainData.Tasks[i].Id == _originalTask.Id)
            {
                MainData.Tasks.RemoveAt(i);
                MainData.Tasks.Insert(i, updatedTask);
                break;
            }
        }
        
        for (int i = 0; i < MainData.Users.Count; i++)
        {
            if (assignedUserIds.Contains(MainData.Users[i].Id))
            {
                if (!MainData.Users[i].TaskIds.Contains(_originalTask.Id))
                {
                    MainData.Users[i].TaskIds.Add(_originalTask.Id);
                }
            }
            else
            {
                if (MainData.Users[i].TaskIds.Contains(_originalTask.Id))
                {
                    MainData.Users[i].TaskIds.Remove(_originalTask.Id);
                }
            }
        }
        
        foreach (var group in MainData.Groups)
        {
            if (assignedGroupIds.Contains(group.Id))
            {
                if (!group.TaskIds.Contains(_originalTask.Id))
                {
                    group.TaskIds.Add(_originalTask.Id);
                }
            }
            else
            {
                if (group.TaskIds.Contains(_originalTask.Id))
                {
                    group.TaskIds.Remove(_originalTask.Id);
                }
            }
        }
        
        RequestClose?.Invoke();
    }
}