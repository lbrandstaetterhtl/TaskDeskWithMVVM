using System;
using System.Collections.Generic;
using System.ComponentModel;
using TaskDesk_version2.Models;
using TaskDesk_version2.Views;

namespace TaskDesk_version2.ViewModels;

public class OpenTaskWindowViewModel : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private DateTimeOffset? _dueDate = DateTimeOffset.Now;
    private string _state = string.Empty;
    private List<User> _assignedUsers = new();
    private List<Group> _assignedGroups = new();
    
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
    
    public List<User> AssignedUsers
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
    
    public List<Group> AssignedGroups
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
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public OpenTaskWindowViewModel(Task task)
    {
        Title = task.Title;
        Description = task.Description;
        DueDate = new DateTimeOffset(task.DueDate.ToDateTime(TimeOnly.MinValue));
        State = task.GetTaskStateAsString();
        
        AssignedGroups = GroupsOperator.GetListFromIds(task.GroupIds, MainData.Groups);
        AssignedUsers = UsersOperator.GetListFromIds(task.UserIds, MainData.Users);
    }

    private void SaveTask()
    {
        
    }
}