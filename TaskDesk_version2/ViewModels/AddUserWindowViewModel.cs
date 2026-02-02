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
    private string _fullname = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _roleString;
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
    
    public string RoleString
    {
        get => _roleString;
        set
        {
            if (_roleString != value)
            {
                _roleString = value;
                OnPropertyChanged(nameof(RoleString));
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
        try
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            var groupIds = GroupsOperator.GetIdsFromNames(GroupNames, MainData.Groups);

            if (Fullname == string.Empty || Email == string.Empty || Password == string.Empty)
            {
                var errorWindow = new Views.ErrorWindow("Please fill in all required fields.");
                await errorWindow.ShowDialog(desktop.MainWindow);
                return;
            }

            UserRole role;
            try
            {
                role = RoleConverter.StringToRole(RoleString);
            }
            catch (Exception)
            {
                var errorWindow = new Views.ErrorWindow("Invalid role selected.");
                await errorWindow.ShowDialog(desktop.MainWindow);
                return;
            }

            var newUser = new User(Fullname, Email, Password, role, groupIds);

            await Dispatcher.UIThread.InvokeAsync(() => MainData.Users.Add(newUser));

            foreach (var groupId in groupIds)
            {
                var group = GroupsOperator.GetGroupById(groupId);
                if (group != null)
                {
                    group.UserIds.Add(newUser.Id);
                }
            }
            
            AppLogger.Info("New user added: ID: " + newUser.Id);

            RequestClose?.Invoke();
        }
        catch (Exception e)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;
            
            var errorWindow = new Views.ErrorWindow($"An error occurred while saving the user: {e.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
            
            AppLogger.Error("Error while saving user: " + e.Message);
        }
    }
}