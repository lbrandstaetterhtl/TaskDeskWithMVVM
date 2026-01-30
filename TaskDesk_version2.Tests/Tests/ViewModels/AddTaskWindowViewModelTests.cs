using TaskDesk_version2.ViewModels;
using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.Generic;

namespace TaskDesk_version2.Tests.Tests.ViewModels;

/// <summary>
/// Unit-Tests für AddTaskWindowViewModel
/// </summary>
public class AddTaskWindowViewModelTests : IDisposable
{
    public AddTaskWindowViewModelTests()
    {
        // Vor jedem Test: Testdaten initialisieren
        MainData.Tasks.Clear();
        MainData.Users.Clear();
        MainData.Groups.Clear();
    }

    public void Dispose()
    {
        // Nach jedem Test: Aufräumen
        MainData.Tasks.Clear();
        MainData.Users.Clear();
        MainData.Groups.Clear();
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_InitializesDefaultValues()
    {
        // Act
        var viewModel = new AddTaskWindowViewModel();

        // Assert
        Assert.Equal(string.Empty, viewModel.TaskTitle);
        Assert.Equal(string.Empty, viewModel.TaskDescription);
        Assert.Equal(string.Empty, viewModel.TaskStateString);
        Assert.NotNull(viewModel.GroupNames);
        Assert.NotNull(viewModel.UserNames);
        Assert.Empty(viewModel.GroupNames);
        Assert.Empty(viewModel.UserNames);
    }

    [Fact]
    public void Constructor_InitializesCommands()
    {
        // Act
        var viewModel = new AddTaskWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.SaveCommand);
        Assert.NotNull(viewModel.CancelCommand);
    }

    [Fact]
    public void Constructor_InitializesDateOn_ToNow()
    {
        // Act
        var viewModel = new AddTaskWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.DateOn);
        Assert.True(viewModel.DateOn?.Date == DateTimeOffset.Now.Date);
    }

    #endregion

    #region TaskTitle Property Tests

    [Fact]
    public void TaskTitle_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.TaskTitle))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.TaskTitle = "New Task Title";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("New Task Title", viewModel.TaskTitle);
    }

    [Fact]
    public void TaskTitle_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        viewModel.TaskTitle = "Same Title";
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.TaskTitle))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.TaskTitle = "Same Title";

        // Assert
        Assert.False(propertyChangedRaised);
    }

    [Fact]
    public void TaskTitle_CanBeSetToEmptyString()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        viewModel.TaskTitle = "Some Title";

        // Act
        viewModel.TaskTitle = "";

        // Assert
        Assert.Equal("", viewModel.TaskTitle);
    }

    [Fact]
    public void TaskTitle_CanContainSpecialCharacters()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        var specialTitle = "Task: <Test> & \"Quotes\" äöü";

        // Act
        viewModel.TaskTitle = specialTitle;

        // Assert
        Assert.Equal(specialTitle, viewModel.TaskTitle);
    }

    #endregion

    #region TaskDescription Property Tests

    [Fact]
    public void TaskDescription_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.TaskDescription))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.TaskDescription = "New Description";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("New Description", viewModel.TaskDescription);
    }

    [Fact]
    public void TaskDescription_CanBeMultiLine()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        var multiLineDesc = "Line 1\nLine 2\nLine 3";

        // Act
        viewModel.TaskDescription = multiLineDesc;

        // Assert
        Assert.Equal(multiLineDesc, viewModel.TaskDescription);
    }

    #endregion

    #region DateOn Property Tests

    [Fact]
    public void DateOn_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.DateOn))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.DateOn = new DateTimeOffset(2026, 12, 31, 0, 0, 0, TimeSpan.Zero);

        // Assert
        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void DateOn_CanBeSetToNull()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();

        // Act
        viewModel.DateOn = null;

        // Assert
        Assert.Null(viewModel.DateOn);
    }

    [Fact]
    public void DateOn_CanBeSetToFutureDate()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        var futureDate = DateTimeOffset.Now.AddYears(5);

        // Act
        viewModel.DateOn = futureDate;

        // Assert
        Assert.Equal(futureDate, viewModel.DateOn);
    }

    [Fact]
    public void DateOn_CanBeSetToPastDate()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        var pastDate = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);

        // Act
        viewModel.DateOn = pastDate;

        // Assert
        Assert.Equal(pastDate, viewModel.DateOn);
    }

    #endregion

    #region TaskStateString Property Tests

    [Fact]
    public void TaskStateString_SetValidValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.TaskStateString))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.TaskStateString = "Pending...";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("Pending...", viewModel.TaskStateString);
    }

    [Fact]
    public void TaskStateString_SetEmptyString_DoesNotChange()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        viewModel.TaskStateString = "Pending...";

        // Act
        viewModel.TaskStateString = "";

        // Assert
        Assert.Equal("Pending...", viewModel.TaskStateString);
    }

    [Theory]
    [InlineData("Pending...")]
    [InlineData("In Progress...")]
    [InlineData("Completed!")]
    [InlineData("On Hold...")]
    [InlineData("Cancelled!")]
    public void TaskStateString_CanBeSetToAllValidStates(string state)
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();

        // Act
        viewModel.TaskStateString = state;

        // Assert
        Assert.Equal(state, viewModel.TaskStateString);
    }

    #endregion

    #region GroupNames Property Tests

    [Fact]
    public void GroupNames_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.GroupNames))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.GroupNames = new List<string> { "Group 1", "Group 2" };

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal(2, viewModel.GroupNames.Count);
    }

    [Fact]
    public void GroupNames_CanBeSetToEmptyList()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        viewModel.GroupNames = new List<string> { "Group 1" };

        // Act
        viewModel.GroupNames = new List<string>();

        // Assert
        Assert.Empty(viewModel.GroupNames);
    }

    #endregion

    #region UserNames Property Tests

    [Fact]
    public void UserNames_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.UserNames))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.UserNames = new List<string> { "User 1", "User 2" };

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal(2, viewModel.UserNames.Count);
    }

    #endregion

    #region RequestClose Event Tests

    [Fact]
    public void CancelCommand_InvokesRequestClose()
    {
        // Arrange
        var viewModel = new AddTaskWindowViewModel();
        bool requestCloseInvoked = false;
        viewModel.RequestClose += () => requestCloseInvoked = true;

        // Act
        viewModel.CancelCommand.Execute(null);

        // Assert
        Assert.True(requestCloseInvoked);
    }

    #endregion
}
