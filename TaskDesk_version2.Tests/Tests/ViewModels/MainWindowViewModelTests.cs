using System;
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
}
