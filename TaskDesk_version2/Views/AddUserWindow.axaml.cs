using System;
using System.ComponentModel;
using Avalonia.Controls;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class AddUserWindow : Window
{
    public AddUserWindow()
    {
        InitializeComponent();
        
        SetGroupList();
        
        SetRoleCombo();
        
        var vm = new AddUserWindowViewModel();
        vm.RequestClose += Close;
        DataContext = vm;
    }
    
    private void SetGroupList()
    {
        if (MainData.Groups.Count <= 0)
        {
            return;
        }

        var groups = MainData.Groups;

        foreach (var group in groups)
        {
            GroupList.Items.Add(group.Name);
        }
    }
    
    private void SetRoleCombo()
    {
        foreach (var enumValue in Enum.GetValues(typeof(UserRole)))
        {
            UserRole value = (UserRole)enumValue;
            RoleComboBox.Items.Add(RoleConverter.RoleToString(value));
        }
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        AppLogger.Info("------------- Add User Window Closed -------------");
    }
    
    private void OnOpened(object? sender, EventArgs e)
    {
        AppLogger.Info("------------- Add User Window Opened -------------");
    }
}