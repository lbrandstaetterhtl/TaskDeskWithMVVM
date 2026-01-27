using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var vm = new MainWindowViewModel();

        DataContext = vm;

        AddTaskMenuItem.Click += MainWindowViewModel.OnAddTaskClick;

        AddUserMenuItem.Click += MainWindowViewModel.OnAddUserClick;
        
        AddGroupMenuItem.Click += MainWindowViewModel.OnAddGroupClick;
        
        ManageUsersMenuItem.Click += MainWindowViewModel.OnManageUsersClick;
        
        ClearAllTasksMenuItem.Click += MainWindowViewModel.OnClearAllTasksClick;
        
        ClearAllGroupsMenuItem.Click += MainWindowViewModel.OnClearAllGroupsClick;
        
        ClearAllUsersMenuItem.Click += MainWindowViewModel.OnClearAllUsersClick;

        Closing += OnClosing;
    }

    private void OnClosing(object sender, WindowClosingEventArgs e)
    {
        TasksOperator.SaveTasksToJson(MainData.Tasks);

        UsersOperator.SaveUsersToJson(MainData.Users);

        GroupsOperator.SaveGroupsToJson(MainData.Groups);
    }

    private void TaskDoubleClick(object? sender, TappedEventArgs e)
    {
        if (sender is Border { DataContext: Task task })
        {
            MainWindowViewModel.OnOpenTaskClick(task);
        }
    }

    private void TaskOpenClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: Task task })
        {
            MainWindowViewModel.OnOpenTaskClick(task);
        }
    }

    private void PointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is Border border)
        {
            border.Opacity = 0.8;
        }
    }

    private void PointerExited(object? sender, PointerEventArgs e)
    {
        if (sender is Border border)
        {
            border.Opacity = 1.0;
        }
    }
    
    private void OnDeleteTaskClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: Task task })
        {
            MainWindowViewModel.OnDeleteTaskClick(task);
        }
    }
}