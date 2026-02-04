using System;
using System.ComponentModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();

        new MainData();
        
        var vm = new ViewModels.LoginWindowViewModel();
        vm.RequestClose += Close;
        vm.PasswordVisibleCommand = new RelayCommand(PasswordVisibility);
        DataContext = vm;

        var lastLoggedInUserFound = false;
        foreach (var user in vm.SavedUsers)
        {
            SavedUserComboBox.Items.Add(user.FullName);

            if (user.Id == MainData.Settings.LastLoggedInUserId)
            {
                SavedUserComboBox.SelectedItem = user.FullName;
                lastLoggedInUserFound = true;
            }
        }

        if (!lastLoggedInUserFound && vm.SavedUsers.Count > 0)
        {
            SavedUserComboBox.SelectedItem = SavedUserComboBox.Items[0];
            LoginTextBox.Text = vm.SavedUsers[0].Email;
            PasswordBox.Watermark = "Enter password for " + vm.SavedUsers[0].FullName;
        }
        else if (lastLoggedInUserFound && vm.SavedUsers.Count > 0)
        {
            var user = vm.SavedUsers.Find(u => u.Id == MainData.Settings.LastLoggedInUserId);;
            if (user != null)
            {
                LoginTextBox.Text = user.Email;
                PasswordBox.Watermark = "Enter password for " + user.FullName;
            }
        }
        
        Closing += OnClosing;
        Opened += OnOpened;
    }

    private void PasswordVisibility()
    {
        if (PasswordBox.PasswordChar == '*')
        {
            PasswordBox.PasswordChar = '\0';
            return;
        }
        
        PasswordBox.PasswordChar = '*';
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
        if (DataContext is not ViewModels.LoginWindowViewModel vm)
        {
            return;
        }
        
        if (SavedUserComboBox.SelectedIndex < 0 || SavedUserComboBox.SelectedIndex >= vm.SavedUsers.Count)
        {
            return;
        }

        var selectedUser = vm.SavedUsers[SavedUserComboBox.SelectedIndex];
        LoginTextBox.Text = selectedUser.Email;
        PasswordBox.Watermark = "Enter password for " + selectedUser.FullName;
    }
    
    private void LoginTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (DataContext is not ViewModels.LoginWindowViewModel vm)
        {
            return;
        }

        if (LoginTextBox.Text == "")
        {
            PasswordBox.Watermark = "Enter password";
            SavedUserComboBox.SelectedItem = "";
        }
    }
}