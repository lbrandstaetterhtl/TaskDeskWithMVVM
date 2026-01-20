using System;
using Avalonia.Controls;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class AddTaskWindow : Window
{
    public AddTaskWindow()
    {
        InitializeComponent();
        
        SetGroupList();
        
        SetStateCombo();
        
        SetUserList();
        
        var vm = new AddTaskWindowViewModel();
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
    
    private void SetUserList()
    {
        if (MainData.Users.Count <= 0)
        {
            return;
        }

        var users = MainData.Users;

        foreach (var user in users)
        {
            UserList.Items.Add(user.FullName);
        }
    }

    private void SetStateCombo()
    {
        foreach (var enumValue in Enum.GetValues(typeof(TaskState)))
        {
            TaskState value = (TaskState)enumValue;

            if (value != null)
            {
                StateCombo.Items.Add(StateConverter.StateToString(value));
            }
        }
    }
}