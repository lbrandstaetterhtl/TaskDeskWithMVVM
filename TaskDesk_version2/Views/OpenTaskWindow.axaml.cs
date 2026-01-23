using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
        
        SetStateCombo();
        
        IdBox.Text = "ID: " + _task.Id;
      
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