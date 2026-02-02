using System;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Controls;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class ManageUsersWindow : Window
{
    private User _originalUser;
    
    public ManageUsersWindow(User user)
    {
        InitializeComponent();
        
        var vm = new ManageUsersWindowViewModel(user);
        DataContext = vm;
        _originalUser = user;
        
        IdBox.Text = "ID: " + user.Id;
        
        vm.RequestClose += Close;
        
        vm.UpdateData();
        
        SetRoleCombo();

        UserList.SelectionChanged += (_, _) =>
        {
            if (UserList.SelectedItem is User selectedUser)
            {
                vm.SelectedUser = selectedUser;
                _originalUser = selectedUser;
                vm.UpdateData();
                IdBox.Text = "ID: " + selectedUser.Id;
            }
        };
        
        UserList.SelectedItem = _originalUser;
        
        SearchBar.TextChanged += (_, _) =>
        {
            vm.SearchInput = SearchBar.Text ?? string.Empty;
            if (vm.SearchInput.Length > 0)
            {
                vm.SearchUpdate();
                if (!vm.AllUsers.Contains(vm.SelectedUser!))
                {
                    vm.ClearData();
                    UserList.SelectedItem = null;
                    IdBox.Text = "ID: ";
                }
            }
            else
            {
                vm.ClearData();
                vm.ClearSearch();
            }
        };
        
        Closing += OnClosing;
        Opened += OnOpened;
    }
    
    private void SetRoleCombo()
    {
        foreach (var enumValue in Enum.GetValues(typeof(UserRole)))
        {
            UserRole value = (UserRole)enumValue;
            RoleComboBox.Items.Add(RoleConverter.RoleToString(value));
        }
        
        RoleComboBox.SelectedItem = RoleConverter.RoleToString(_originalUser.Role);
    }

    private void OnClosing(object? s, WindowClosingEventArgs e)
    {
        AppLogger.Info("------------ Manage Users Window Closed -------------");
    }
    
    private void OnOpened(object? s, EventArgs e)
    {
        AppLogger.Info("------------ Manage Users Window Opened -------------");
    }
}