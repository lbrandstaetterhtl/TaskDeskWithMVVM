using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class MainWindow : Window
{
    private ObservableCollection<Task>? _currentFilteredTasks;
    
    public MainWindow()
    {
        InitializeComponent();

        var vm = new MainWindowViewModel();

        DataContext = vm;

        if (MainData.Settings.IsThemeDark)
            ChangeThemeMenuItem.Header += " (Current: Dark)";
        else
            ChangeThemeMenuItem.Header += " (Current: Light)";
        
        // Subscribe to initial FilteredTasks changes
        _currentFilteredTasks = vm.FilteredTasks;
        if (_currentFilteredTasks != null)
            _currentFilteredTasks.CollectionChanged += Tasks_CollectionChanged;
        
        // Listen for FilteredTasks property replacement (after ApplyFilters/ClearFilters)
        vm.PropertyChanged += ViewModelOnPropertyChanged;

        Closing += OnClosing;
        Opened += OnOpened;
    }

    private void ViewModelOnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainWindowViewModel.FilteredTasks) && sender is MainWindowViewModel vm)
        {
            // Unsubscribe from old FilteredTasks
            if (_currentFilteredTasks != null)
                _currentFilteredTasks.CollectionChanged -= Tasks_CollectionChanged;
            
            // Subscribe to new FilteredTasks
            _currentFilteredTasks = vm.FilteredTasks;
            if (_currentFilteredTasks != null)
                _currentFilteredTasks.CollectionChanged += Tasks_CollectionChanged;
            
            // After FilteredTasks was replaced (ApplyFilters or ClearFilters), update row backgrounds
            Dispatcher.UIThread.Post(SetBackgroundColorOfBorder, DispatcherPriority.Loaded);
        }
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (Design.IsDesignMode) return;

        if (DataContext is MainWindowViewModel vm)
        {
            vm.Tasks.CollectionChanged -= Tasks_CollectionChanged;
            vm.PropertyChanged -= ViewModelOnPropertyChanged;
            if (_currentFilteredTasks != null)
                _currentFilteredTasks.CollectionChanged -= Tasks_CollectionChanged;
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
        if (Design.IsDesignMode) return;

        if (DataContext is MainWindowViewModel vm)
        {
            vm.Tasks.CollectionChanged += Tasks_CollectionChanged;
            // Ensure PropertyChanged is subscribed (already added in ctor, but keep consistent if DataContext changes)
            vm.PropertyChanged -= ViewModelOnPropertyChanged;
            vm.PropertyChanged += ViewModelOnPropertyChanged;
        }

        Dispatcher.UIThread.Post(SetBackgroundColorOfBorder, DispatcherPriority.Loaded);

        AppLogger.Info("------------- Main Window Opened -------------");
    }

    private void TaskDoubleClick(object? sender, TappedEventArgs e)
    {
        if (sender is Border { DataContext: Task task }) MainWindowViewModel.OnOpenTaskClick(task);
    }

    private void OnTaskOpenClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: Task task }) MainWindowViewModel.OnOpenTaskClick(task);
    }

    private new void PointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is Border border) border.Opacity = 0.8;
    }

    private new void PointerExited(object? sender, PointerEventArgs e)
    {
        if (sender is Border border) border.Opacity = 1.0;
    }

    private void OnDeleteTaskClick(object? sender, RoutedEventArgs e)
    {
        if (sender is MenuItem { DataContext: Task task }) MainWindowViewModel.OnDeleteTaskClick(task);
    }

    private void OnChangeThemeClick(object? s, RoutedEventArgs e)
    {
        MainWindowViewModel.OnChangeThemeClick();

        if (MainData.Settings.IsThemeDark)
            ChangeThemeMenuItem.Header = "Change Theme (Current: Dark)";
        else
            ChangeThemeMenuItem.Header = "Change Theme (Current: Light)";
    }

    private void Tasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.UIThread.Post(SetBackgroundColorOfBorder, DispatcherPriority.Loaded);
    }

    private void OnStateBorderDataContextChanged(object? sender, EventArgs e)
    {
        if (sender is not Border border)
            return;

        if (border.DataContext is Task task)
        {
            border.Classes.Clear();
            border.Classes.Add(task.State.ToString());
        }
    }

    private void SetBackgroundColorOfBorder()
    {
        var itemsControl = this.FindControl<ItemsControl>("TasksGrid");
        if (itemsControl?.Presenter?.Panel == null)
            return;

        var index = 0;
        foreach (var child in itemsControl.Presenter.Panel.Children)
            if (child is ContentPresenter contentPresenter &&
                contentPresenter.Child is Border border &&
                border.Classes.Contains("TaskRowBorder"))
            {
                border.Background = index % 2 == 0
                    ? Brushes.DimGray
                    : Brushes.DarkGray;
                index++;
            }
    }
}