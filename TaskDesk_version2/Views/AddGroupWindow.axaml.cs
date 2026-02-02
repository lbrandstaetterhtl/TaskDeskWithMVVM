using System;
using Avalonia.Controls;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class AddGroupWindow : Window
{
    public AddGroupWindow()
    {
        InitializeComponent();
        
        var vm = new AddGroupWindowViewModel();
        vm.RequestClose += Close;
        DataContext = vm;
        
        SetUserList();
        
        Opened += OnOpened;
        Closing += OnClosing;
    }
    
    private void SetUserList()
    {
        if (MainData.Users.Count <= 0)
        {
            return;
        }

        var users = MainData.Users;

        foreach (var user in users)
        {
            UsersList.Items.Add(user.FullName);
        }
    }
    
    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        AppLogger.Info("------------- Add Group Window Closed -------------");
    }
    
    private void OnOpened(object? sender, EventArgs e)
    {
        AppLogger.Info("------------- Add Group Window Opened -------------");
    }
}