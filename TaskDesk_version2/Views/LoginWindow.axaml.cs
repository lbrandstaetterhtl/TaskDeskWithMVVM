using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class LoginWindow : Window
{
    private readonly LoginWindowViewModel _vm;
    private readonly bool _lastLoggedInUserFound = false;
    
    public LoginWindow()
    {
        InitializeComponent();

        new MainData();

        _vm = new LoginWindowViewModel();
        
        _vm.RequestClose += Close;
        _vm.PasswordVisibleCommand = new RelayCommand(PasswordVisibility);
        DataContext = _vm;
        
        foreach (var user in _vm.SavedUsers)
        {
            SavedUserComboBox.Items.Add(user.FullName);

            if (user.Id == MainData.Settings.LastLoggedInUserId)
            {
                SavedUserComboBox.SelectedItem = user.FullName;
                _lastLoggedInUserFound = true;
            }
        }
        
        SetPasswordWaterMark(true);
        
        Closing += OnClosing;
        Opened += OnOpened;
    }

    private void PasswordVisibility()
    {
        if (PasswordBox.PasswordChar == '*')
        {
            PasswordBox.PasswordChar = '\0';
            PasswordVisibilityToggleImage.Source = new Bitmap("../../../Assets/Visible.png");
            return;
        }
        
        PasswordBox.PasswordChar = '*';
        PasswordVisibilityToggleImage.Source = new Bitmap("../../../Assets/NotVisible.png");
    }

    private void SetPasswordWaterMark(bool isFirstLoad = false)
    {
        
        if (!_lastLoggedInUserFound && _vm.SavedUsers.Count > 0)
        {
            SavedUserComboBox.SelectedItem = SavedUserComboBox.Items[0];
            SetLoginBoxText(isFirstLoad);
            PasswordBox.Watermark = "Enter password for " + _vm.SavedUsers[0].FullName;
        }
        else if (_lastLoggedInUserFound && _vm.SavedUsers.Count > 0)
        {
            var user = _vm.SavedUsers.Find(u => u.Id == MainData.Settings.LastLoggedInUserId);;
            if (user != null)
            {
                SetLoginBoxText(isFirstLoad, user.Email);
                PasswordBox.Watermark = "Enter password for " + user.FullName;
            }
        }
    }

    private void SetLoginBoxText(bool isFirstLoad, string email = "")
    {
        if (isFirstLoad)
        {
            if (_lastLoggedInUserFound)
            {
                LoginTextBox.Text = _vm.SavedUsers[0].Email;
            }
            else
            {
                LoginTextBox.Text = email;
            }
        }
    }
    
    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (LoginTextBox.Text == string.Empty || PasswordBox.Text == string.Empty)
        {
            AppLogger.Info("------------- Login Window Closed -------------");
            AppLogger.Info("------------- Application Closed -----------------------------------------");
            
            return;
        }
        
        AppLogger.Info("------------- Login Window Closed -------------");
    }
    
    private void OnOpened(object? sender, EventArgs e)
    {
        AppLogger.Info("------------- Login Window Opened -------------");
    }
    
    private void SavedUserComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SavedUserComboBox.SelectedIndex < 0 || SavedUserComboBox.SelectedIndex >= _vm.SavedUsers.Count)
        {
            return;
        }

        var selectedUser = _vm.SavedUsers[SavedUserComboBox.SelectedIndex];
        LoginTextBox.Text = selectedUser.Email;
        PasswordBox.Watermark = "Enter password for " + selectedUser.FullName;
    }
    
    private void LoginTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (LoginTextBox.Text == "")
        {
            PasswordBox.Watermark = "Enter password";
            SavedUserComboBox.SelectedItem = "";
            return;
        }
        
        
    }
    
    private void PasswordBox_GotFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        PasswordBox.Watermark = "";
    }

    private void PasswordBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SetPasswordWaterMark();
    }
}