﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;
using Xunit;
using Task = TaskDesk_version2.Models.Task;

namespace TaskDesk_version2.Tests.Tests.ViewModels;

/// <summary>
/// Unit-Tests fuer OpenTaskWindowViewModel
/// </summary>
public class OpenTaskWindowViewModelTests : IDisposable
{
    public OpenTaskWindowViewModelTests()
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
    public void Constructor_InitializesFromTask()
    {
        // Arrange
        var user = new User { Id = 1, FullName = "User One" };
        var group = new Group { Id = 2, Name = "Group A" };
        MainData.Users.Add(user);
        MainData.Groups.Add(group);
        var task = new Task(5, "Title", "Desc", new DateOnly(2026, 12, 31), TaskState.InProgress,
            new() { 2 }, new() { 1 });

        // Act
        var viewModel = new OpenTaskWindowViewModel(task);

        // Assert
        Assert.Equal("Title", viewModel.Title);
        Assert.Equal("Desc", viewModel.Description);
        Assert.Equal("In Progress...", viewModel.State);
        Assert.Equal(task.DueDate.ToDateTime(TimeOnly.MinValue).Date, viewModel.DueDate?.Date);
        Assert.Single(viewModel.AssignedUsers);
        Assert.Single(viewModel.AssignedGroups);
        Assert.Equal(1, viewModel.AssignedUsers[0].Id);
        Assert.Equal(2, viewModel.AssignedGroups[0].Id);
    }

    [Fact]
    public void Constructor_InitializesCommands()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());

        // Act
        var viewModel = new OpenTaskWindowViewModel(task);

        // Assert
        Assert.NotNull(viewModel.SaveTaskCommand);
        Assert.NotNull(viewModel.CloseCommand);
    }

    [Fact]
    public void Constructor_WithNoAssignedUsers_CreatesEmptyList()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());

        // Act
        var viewModel = new OpenTaskWindowViewModel(task);

        // Assert
        Assert.NotNull(viewModel.AssignedUsers);
        Assert.Empty(viewModel.AssignedUsers);
    }

    [Fact]
    public void Constructor_WithNoAssignedGroups_CreatesEmptyList()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());

        // Act
        var viewModel = new OpenTaskWindowViewModel(task);

        // Assert
        Assert.NotNull(viewModel.AssignedGroups);
        Assert.Empty(viewModel.AssignedGroups);
    }

    #endregion

    #region Title Property Tests

    [Fact]
    public void Title_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Title))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Title = "New Title";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("New Title", viewModel.Title);
    }

    [Fact]
    public void Title_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Title))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Title = "Title";

        // Assert
        Assert.False(propertyChangedRaised);
    }

    #endregion

    #region Description Property Tests

    [Fact]
    public void Description_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Description))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Description = "New Description";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("New Description", viewModel.Description);
    }

    [Fact]
    public void Description_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Description))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Description = "Desc";

        // Assert
        Assert.False(propertyChangedRaised);
    }

    #endregion

    #region DueDate Property Tests

    [Fact]
    public void DueDate_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.DueDate))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.DueDate = new DateTimeOffset(2027, 6, 15, 0, 0, 0, TimeSpan.Zero);

        // Assert
        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void DueDate_CanBeSetToNull()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);

        // Act
        viewModel.DueDate = null;

        // Assert
        Assert.Null(viewModel.DueDate);
    }

    #endregion

    #region State Property Tests

    [Fact]
    public void State_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.State))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.State = "Completed!";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("Completed!", viewModel.State);
    }

    [Theory]
    [InlineData("Pending...")]
    [InlineData("In Progress...")]
    [InlineData("Completed!")]
    [InlineData("On Hold...")]
    [InlineData("Cancelled!")]
    public void State_CanBeSetToAllValidStates(string state)
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);

        // Act
        viewModel.State = state;

        // Assert
        Assert.Equal(state, viewModel.State);
    }

    #endregion

    #region AssignedUsers Property Tests

    [Fact]
    public void AssignedUsers_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.AssignedUsers))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.AssignedUsers = new ObservableCollection<User> { new User { Id = 1, FullName = "Test" } };

        // Assert
        Assert.True(propertyChangedRaised);
    }

    #endregion

    #region AssignedGroups Property Tests

    [Fact]
    public void AssignedGroups_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.AssignedGroups))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.AssignedGroups = new ObservableCollection<Group> { new Group { Id = 1, Name = "Test" } };

        // Assert
        Assert.True(propertyChangedRaised);
    }

    #endregion

    #region AllUsers Property Tests

    [Fact]
    public void AllUsers_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.AllUsers))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.AllUsers = new ObservableCollection<User>();

        // Assert
        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void AllUsers_InitializedWithMainDataUsers()
    {
        // Arrange
        var user = new User { Id = 1, FullName = "Test User" };
        MainData.Users.Add(user);
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());

        // Act
        var viewModel = new OpenTaskWindowViewModel(task);

        // Assert
        Assert.Same(MainData.Users, viewModel.AllUsers);
    }

    #endregion

    #region AllGroups Property Tests

    [Fact]
    public void AllGroups_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.AllGroups))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.AllGroups = new ObservableCollection<Group>();

        // Assert
        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void AllGroups_InitializedWithMainDataGroups()
    {
        // Arrange
        var group = new Group { Id = 1, Name = "Test Group" };
        MainData.Groups.Add(group);
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());

        // Act
        var viewModel = new OpenTaskWindowViewModel(task);

        // Assert
        Assert.Same(MainData.Groups, viewModel.AllGroups);
    }

    #endregion

    #region CloseCommand Tests

    [Fact]
    public void CloseCommand_InvokesRequestClose()
    {
        // Arrange
        var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());
        var viewModel = new OpenTaskWindowViewModel(task);
        var closed = false;
        viewModel.RequestClose += () => closed = true;

        // Act
        viewModel.CloseCommand.Execute(null);

        // Assert
        Assert.True(closed);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithAllTaskStates_InitializesCorrectly()
    {
        // Arrange & Act & Assert
        foreach (TaskState state in Enum.GetValues(typeof(TaskState)))
        {
            var task = new Task(1, "Title", "Desc", new DateOnly(2026, 1, 1), state, new(), new());
            var viewModel = new OpenTaskWindowViewModel(task);
            Assert.Equal(StateConverter.StateToString(state), viewModel.State);
        }
    }

    [Fact]
    public void Constructor_WithSpecialCharactersInTitle_HandlesCorrectly()
    {
        // Arrange
        var specialTitle = "Task: <Test> & \"Quotes\" äöü";
        var task = new Task(1, specialTitle, "Desc", new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());

        // Act
        var viewModel = new OpenTaskWindowViewModel(task);

        // Assert
        Assert.Equal(specialTitle, viewModel.Title);
    }

    [Fact]
    public void Constructor_WithMultilineDescription_HandlesCorrectly()
    {
        // Arrange
        var multilineDesc = "Line 1\nLine 2\nLine 3";
        var task = new Task(1, "Title", multilineDesc, new DateOnly(2026, 1, 1), TaskState.Pending, new(), new());

        // Act
        var viewModel = new OpenTaskWindowViewModel(task);

        // Assert
        Assert.Equal(multilineDesc, viewModel.Description);
    }

    #endregion
}
