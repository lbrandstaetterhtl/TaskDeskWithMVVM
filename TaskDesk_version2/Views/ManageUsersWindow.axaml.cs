using System;
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