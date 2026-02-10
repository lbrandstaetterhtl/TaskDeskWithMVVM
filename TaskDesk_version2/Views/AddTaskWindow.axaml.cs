using System;
using Avalonia.Controls;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class AddTaskWindow : Window
{
    public AddTaskWindow()
    {
        InitializeComponent();

        var vm = new AddTaskWindowViewModel();
        vm.RequestClose += Close;
        DataContext = vm;

        Opened += OnOpened;
        Closing += OnClosing;
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        AppLogger.Info("------------- Add Task Window Closed -------------");
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        AppLogger.Info("------------- Add Task Window Opened -------------");
    }
}