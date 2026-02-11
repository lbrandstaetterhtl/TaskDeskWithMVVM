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
        
        IdBlock.Text = "ID: " + _task.Id;

        Opened += OnOpened;
        Closing += OnClosing;
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        AppLogger.Info("------------- Task Window Closed -------------");
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        AppLogger.Info("------------- Task Window Opened -------------");
    }
}