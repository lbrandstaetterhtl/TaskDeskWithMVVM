using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.ViewModels;

public class LoginWindowViewModel : INotifyPropertyChanged
{
    private string _email = string.Empty;
    private string _password = string.Empty;
    private bool _isValid = false;
    private List<User> _savedUsers = new List<User>();
    
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
    
    public bool IsValid
    {
        get => _isValid;
        set
        {
            if (_isValid != value)
            {
                _isValid = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }
    }

    public List<User> SavedUsers
    {
        get => _savedUsers;
        set
        {
            if (_savedUsers != value)
            {
                _savedUsers = value;
                OnPropertyChanged(nameof(SavedUsers));
            }
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public LoginWindowViewModel()
    {
        LoginCommand = new RelayCommand(Login);
        CloseCommand = new RelayCommand(() => RequestClose?.Invoke());

        foreach (var userId in MainData.Settings.SavedUserIds)
        {
            foreach (var user in MainData.Users)
            {
                if (user.Id == userId)
                {
                    SavedUsers.Add(user);
                }
            }
        }
    }

    public ICommand LoginCommand { get; set;  }
    public ICommand CloseCommand { get; set;  }
    public Action? RequestClose;
    
    public ICommand PasswordVisibleCommand { get; set; }

    private async void Login()
    {
        try
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                IsValid = false;
                var errorWindow = new Views.ErrorWindow("Email and Password cannot be empty.", "User Error: Invalid Input");
                await errorWindow.ShowDialog(App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow! : null);
                return;
            }

            foreach (var user in MainData.Users)
            {
                if (user.Email == Email && user.Password == Password)
                {
                    IsValid = true;
                    MainData.CurrentUser = user;
                    var mainWindow = new Views.MainWindow();
                    mainWindow.Show();
                    AppLogger.Info("New Login with user:" + Email);
                    if (MainData.Settings.LastLoggedInUserId != user.Id) AppLogger.Info("Set last logged in user id to: " + user.Id);
                    MainData.Settings.LastLoggedInUserId = user.Id;
                    RequestClose?.Invoke();
                    return;
                }
            }

            if (!IsValid)
            {
                var errorWindow = new Views.ErrorWindow("Invalid email or password.", "User Error: Invalid Input");
                await errorWindow.ShowDialog(App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow! : null);
            }
        }
        catch (Exception e)
        {
            var errorWindow = new Views.ErrorWindow($"An error occurred during login: {e.Message}");
            await errorWindow.ShowDialog(App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow! : null);
        }
    }
}