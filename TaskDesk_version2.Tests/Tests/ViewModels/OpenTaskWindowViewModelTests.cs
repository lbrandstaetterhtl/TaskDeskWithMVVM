using System;
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
}
