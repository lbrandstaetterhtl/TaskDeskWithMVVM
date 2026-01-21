using System;
using Avalonia.Controls;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class OpenTaskWindow : Window
{
    private readonly Task _task;
    
    public OpenTaskWindow(Task task)
    {
        InitializeComponent();
        
        var vm = new OpenTaskWindowViewModel(task);
        DataContext = vm;
        vm.RequestClose += Close;
        
        _task = task;
        
        SetUsersList();
        
        SetStateCombo();
        
        IdBox.Text = "ID: " + _task.Id;
    }
    
    private void SetUsersList()
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

        if (DataContext is not OpenTaskWindowViewModel vm)
        {
            return;
        }

        foreach (var user in vm.AssignedUsers)
        {
            UsersList.SelectedItems.Add(user.FullName);
        }
    }

    private void SetGroupList()
    {
        if (MainData.Users.Count <= 0)
        {
            return;
        }
        
        var groups = MainData.Groups;
        
        foreach (var group in groups)
        {
            GroupsList.Items.Add(group.Name);
        }

        if (DataContext is not OpenTaskWindowViewModel vm)
        {
            return;
        }

        foreach (var group in vm.AssignedGroups)
        {
            GroupsList.SelectedItems.Add(group.Name);
        }
    }
    
    private void SetStateCombo()
    {
        foreach (var enumValue in Enum.GetValues(typeof(TaskState)))
        {
            TaskState value = (TaskState)enumValue;
            StateCombo.Items.Add(StateConverter.StateToString(value));
        }

        StateCombo.SelectedItem = StateConverter.StateToString(_task.State);
    }
}