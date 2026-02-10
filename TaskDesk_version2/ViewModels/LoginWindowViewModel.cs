using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;
using TaskDesk_version2.Views;

namespace TaskDesk_version2.ViewModels;

public class LoginWindowViewModel : INotifyPropertyChanged
{
    private string _email = string.Empty;
    private bool _isValid;
    private string _password = string.Empty;
    private List<User> _savedUsers = new();
    public Action? RequestClose;

    public LoginWindowViewModel()
    {
        LoginCommand = new RelayCommand(Login);
        CloseCommand = new RelayCommand(() => RequestClose?.Invoke());

        foreach (var userId in MainData.Settings.SavedUserIds)
        foreach (var user in MainData.Users)
            if (user.Id == userId)
                SavedUsers.Add(user);
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

    public ICommand LoginCommand { get; set; }
    public ICommand CloseCommand { get; set; }

    public ICommand PasswordVisibleCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void Login()
    {
        try
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                IsValid = false;
                var errorWindow = new ErrorWindow("Email and Password cannot be empty.", "User Error: Invalid Input");
                await errorWindow.ShowDialog(
                    App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                        ? desktop.MainWindow!
                        : null);
                return;
            }

            foreach (var user in MainData.Users)
                if (user.Email == Email && user.Password == Password)
                {
                    IsValid = true;
                    MainData.CurrentUser = user;
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    AppLogger.Info("New Login with user:" + Email);
                    if (MainData.Settings.LastLoggedInUserId != user.Id)
                        AppLogger.Info("Set last logged in user id to: " + user.Id);
                    MainData.Settings.LastLoggedInUserId = user.Id;
                    RequestClose?.Invoke();
                    return;
                }

            if (!IsValid)
            {
                var errorWindow = new ErrorWindow("Invalid email or password.", "User Error: Invalid Input");
                await errorWindow.ShowDialog(
                    App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                        ? desktop.MainWindow!
                        : null);
            }
        }
        catch (Exception e)
        {
            var errorWindow = new ErrorWindow($"An error occurred during login: {e.Message}");
            await errorWindow.ShowDialog(
                App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                    ? desktop.MainWindow!
                    : null);
        }
    }
}