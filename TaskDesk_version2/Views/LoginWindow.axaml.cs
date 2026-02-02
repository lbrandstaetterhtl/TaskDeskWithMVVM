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
            AppLogger.Info("------------- Application Closed ----------------------------------------");
            
            return;
        }
        
        AppLogger.Info("------------- Login Window Closed -------------");
    }
    
    private void OnOpened(object? sender, EventArgs e)
    {
        AppLogger.Info("------------- Login Window Opened -------------");
    }
}