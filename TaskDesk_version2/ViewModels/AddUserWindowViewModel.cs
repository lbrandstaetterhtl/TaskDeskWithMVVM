using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.ViewModels;

public class AddUserWindowViewModel : INotifyPropertyChanged
{
    private string _fullname;
    private string _email;
    private string _password;
    private UserRole _role;
    private List<string> _groupnames = new();
    
    public string Fullname
    {
        get => _fullname;
        set
        {
            if (_fullname != value)
            {
                _fullname = value;
                OnPropertyChanged(nameof(Fullname));
            }
        }
    }
    
    public string Email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }
    
    public string Password
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }
    
    public UserRole Role
    {
        get => _role;
        set
        {
            if (_role != value)
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
    }
    
    public List<string> GroupNames
    {
        get => _groupnames;
        set
        {
            if (_groupnames != value)
            {
                _groupnames = value;
                OnPropertyChanged(nameof(GroupNames));
            }
        }
    }
    
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    
    public event Action? RequestClose;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public AddUserWindowViewModel()
    {
        SaveCommand = new RelayCommand(SaveUser);
        CancelCommand = new RelayCommand(() => RequestClose?.Invoke());
    }
    
    private async void SaveUser()
    {
        var groupIds = GroupOperator.GetIdsFromNames(GroupNames, MainData.Groups);
        
        var newUser = new User(MainData.Users.Count, Fullname, Email, Password, Role, groupIds);
        
        await Dispatcher.UIThread.InvokeAsync(() => MainData.Users.Add(newUser));
        
        Console.WriteLine($"{MainData.Users.Count} users added");
        
        RequestClose?.Invoke();
    }
}