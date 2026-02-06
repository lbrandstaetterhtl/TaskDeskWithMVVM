using System;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Controls;
using Avalonia.Threading;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class ManageUsersWindow : Window
{
    private bool _init;
    
    public ManageUsersWindow(User user)
    {
        InitializeComponent();

        _init = true;

        var vm = new ManageUsersWindowViewModel(user);
        SetRoleCombo(vm.OriginalUser.Role);
        DataContext = vm;

        IdBox.Text = "ID: " + vm.OriginalUser.Id;

        vm.RequestClose += Close;
        UserList.SelectedItem = vm.SelectedUser;

        UserList.SelectionChanged += (_, _) =>
        {
            IdBox.Text = "ID: " + vm.SelectedUser?.Id;
        };

        SearchBar.TextChanged += (_, _) =>
        {
            if (_init) return;

            vm.SearchInput = SearchBar.Text ?? string.Empty;

            if (vm.SearchInput.Length > 0)
            {
                vm.SearchUpdate();

                if (vm.SelectedUser != null && !vm.AllUsers.Contains(vm.SelectedUser))
                {
                    vm.ClearData();
                    UserList.SelectedItem = null;
                    IdBox.Text = "ID: ";
                }
            }
            else
            {
                vm.SearchUpdate();
                if (vm.SelectedUser != null)
                    vm.UpdateData();
            }
        };
        
        Closing += OnClosing;
        Opened += OnOpened;
        
        _init = false;
    }
    
    private void SetRoleCombo(UserRole role)
    {
        foreach (var enumValue in Enum.GetValues(typeof(UserRole)))
        {
            UserRole value = (UserRole)enumValue;
            RoleComboBox.Items.Add(RoleConverter.RoleToString(value));
        }
        
        RoleComboBox.SelectedItem = RoleConverter.RoleToString(role);
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