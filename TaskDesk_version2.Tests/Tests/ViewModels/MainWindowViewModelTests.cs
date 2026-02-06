﻿using System;
using System.Collections.ObjectModel;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;
using Xunit;
using Task = TaskDesk_version2.Models.Task;

namespace TaskDesk_version2.Tests.Tests.ViewModels;

/// <summary>
/// Unit-Tests fuer MainWindowViewModel
/// </summary>
public class MainWindowViewModelTests : IDisposable
{
    public MainWindowViewModelTests()
    {
        MainData.Tasks.Clear();
        MainData.Users.Clear();
        MainData.Groups.Clear();
    }

    public void Dispose()
    {
        MainData.Tasks.Clear();
        MainData.Users.Clear();
        MainData.Groups.Clear();
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_UsesMainDataTasks()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Same(MainData.Tasks, viewModel.Tasks);
    }

    [Fact]
    public void Constructor_WithEmptyTasks_InitializesEmpty()
    {
        // Arrange
        MainData.Tasks.Clear();

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.Tasks);
        Assert.Empty(viewModel.Tasks);
    }

    [Fact]
    public void Constructor_WithMultipleTasks_ContainsAllTasks()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1" });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2" });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Task 3" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal(3, viewModel.Tasks.Count);
    }

    #endregion

    #region Tasks Property Tests

    [Fact]
    public void Tasks_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var newTasks = new ObservableCollection<Task> { new Task { Id = 2, Title = "Task 2" } };
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Tasks))
            {
                propertyRaised = true;
            }
        };

        // Act
        viewModel.Tasks = newTasks;

        // Assert
        Assert.True(propertyRaised);
        Assert.Same(newTasks, viewModel.Tasks);
    }

    [Fact]
    public void Tasks_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Tasks))
            {
                propertyRaised = true;
            }
        };

        // Act
        viewModel.Tasks = viewModel.Tasks;

        // Assert
        Assert.False(propertyRaised);
    }

    [Fact]
    public void Tasks_SetNull_SetsToNull()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();

        // Act
        viewModel.Tasks = null!;

        // Assert
        Assert.Null(viewModel.Tasks);
    }

    [Fact]
    public void Tasks_SetNewCollection_UpdatesValue()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var newTasks = new ObservableCollection<Task>
        {
            new Task { Id = 10, Title = "New Task 1" },
            new Task { Id = 11, Title = "New Task 2" }
        };

        // Act
        viewModel.Tasks = newTasks;

        // Assert
        Assert.Equal(2, viewModel.Tasks.Count);
        Assert.Equal(10, viewModel.Tasks[0].Id);
    }

    #endregion

    #region Tasks Collection Synchronization Tests

    [Fact]
    public void Tasks_ReflectsMainDataChanges()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();

        // Act
        MainData.Tasks.Add(new Task { Id = 5, Title = "Added Task" });

        // Assert
        Assert.Single(viewModel.Tasks);
        Assert.Equal("Added Task", viewModel.Tasks[0].Title);
    }

    [Fact]
    public void Tasks_ReflectsMainDataRemovals()
    {
        // Arrange
        var task = new Task { Id = 1, Title = "Task to Remove" };
        MainData.Tasks.Add(task);
        var viewModel = new MainWindowViewModel();

        // Act
        MainData.Tasks.Remove(task);

        // Assert
        Assert.Empty(viewModel.Tasks);
    }

    [Fact]
    public void Tasks_ReflectsMainDataClear()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1" });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2" });
        var viewModel = new MainWindowViewModel();

        // Act
        MainData.Tasks.Clear();

        // Assert
        Assert.Empty(viewModel.Tasks);
    }

    #endregion

    #region PropertyChanged Event Tests

    [Fact]
    public void PropertyChanged_EventNotNullByDefault()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();

        // Act & Assert
        var exception = Record.Exception(() =>
        {
            viewModel.PropertyChanged += (_, _) => { };
        });
        Assert.Null(exception);
    }

    [Fact]
    public void PropertyChanged_MultipleSubscribers_AllNotified()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        int notificationCount = 0;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Tasks))
                notificationCount++;
        };
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Tasks))
                notificationCount++;
        };

        // Act
        viewModel.Tasks = new ObservableCollection<Task>();

        // Assert
        Assert.Equal(2, notificationCount);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_TasksWithDifferentStates_AllIncluded()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Pending", State = TaskState.Pending });
        MainData.Tasks.Add(new Task { Id = 2, Title = "InProgress", State = TaskState.InProgress });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Completed", State = TaskState.Completed });
        MainData.Tasks.Add(new Task { Id = 4, Title = "OnHold", State = TaskState.OnHold });
        MainData.Tasks.Add(new Task { Id = 5, Title = "Cancelled", State = TaskState.Cancelled });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal(5, viewModel.Tasks.Count);
    }

    [Fact]
    public void Tasks_WithSpecialCharactersInTitles_HandledCorrectly()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task: <Test> & \"Quotes\" äöü" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal("Task: <Test> & \"Quotes\" äöü", viewModel.Tasks[0].Title);
    }

    [Fact]
    public void Tasks_WithLargeNumber_HandledCorrectly()
    {
        // Arrange
        for (int i = 0; i < 100; i++)
        {
            MainData.Tasks.Add(new Task { Id = i, Title = $"Task {i}" });
        }

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal(100, viewModel.Tasks.Count);
    }

    #endregion
}
