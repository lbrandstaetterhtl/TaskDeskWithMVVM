using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        
        if (MainData.Settings.IsThemeDark)
        {
            ChangeThemeMenuItem.Header += " (Current: Dark)";
        }
        else
        {
            ChangeThemeMenuItem.Header += " (Current: Light)";
        }

        Closing += OnClosing;
        Opened += OnOpened;
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (Avalonia.Controls.Design.IsDesignMode)
        {
            return;
        }
        
        TasksOperator.SaveTasksToJson(MainData.Tasks);

        UsersOperator.SaveUsersToJson(MainData.Users);

        GroupsOperator.SaveGroupsToJson(MainData.Groups);
        
        SettingsOperator.SaveSettingsToJson(MainData.Settings);
        
        AppLogger.Info("------------- Main Window Closed -------------");
        
        AppLogger.Info("------------- Application Closed ----------------------------------------");
    }
    
    private void OnOpened(object? sender, EventArgs e)
    {
        if (Avalonia.Controls.Design.IsDesignMode)
        {
            return;
        }

        AppLogger.Info("------------- Main Window Opened -------------");
    }

    private void TaskDoubleClick(object? sender, TappedEventArgs e)
    {
        if (sender is Border { DataContext: Task task })
        {
            MainWindowViewModel.OnOpenTaskClick(task);
        }
    }

    private void OnTaskOpenClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: Task task })
        {
            MainWindowViewModel.OnOpenTaskClick(task);
        }
    }

    private new void PointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is Border border)
        {
            border.Opacity = 0.8;
        }
    }

    private new void PointerExited(object? sender, PointerEventArgs e)
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
    
    private void OnChangeThemeClick(object? s, RoutedEventArgs e)
    {
        MainWindowViewModel.OnChangeThemeClick();
        
        if (MainData.Settings.IsThemeDark)
        {
            ChangeThemeMenuItem.Header = "Change Theme (Current: Dark)";
        }
        else
        {
            ChangeThemeMenuItem.Header = "Change Theme (Current: Light)";
        }
    }

    private void OnLogoutClick(object? s, RoutedEventArgs e)
    {
        
    }
}