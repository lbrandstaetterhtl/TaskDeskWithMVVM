using System;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Controls;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class ManageUsersWindow : Window
{
    private readonly User _originalUser;
    
    public ManageUsersWindow(User user)
    {
        InitializeComponent();
        
        var vm = new ManageUsersWindowViewModel(user);
        DataContext = vm;
        _originalUser = user;
        
        IdBox.Text = "ID: " + user.Id;
        
        vm.RequestClose += Close;
        
        SetRoleCombo();

        UserList.SelectionChanged += (_, _) =>
        {
            if (UserList.SelectedItem is User selectedUser)
            {
                vm.SelectedUser = selectedUser;
                vm.UpdateData();
                IdBox.Text = "ID: " + selectedUser.Id;
            }
        };
        
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
}