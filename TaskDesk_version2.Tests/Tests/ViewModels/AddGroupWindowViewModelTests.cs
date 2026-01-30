using TaskDesk_version2.ViewModels;
using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.Generic;

namespace TaskDesk_version2.Tests.Tests.ViewModels;

/// <summary>
/// Unit-Tests für AddGroupWindowViewModel
/// </summary>
public class AddGroupWindowViewModelTests : IDisposable
{
    public AddGroupWindowViewModelTests()
    {
        // Vor jedem Test: Testdaten initialisieren
        MainData.Groups.Clear();
        MainData.Users.Clear();
        MainData.Tasks.Clear();
    }

    public void Dispose()
    {
        // Nach jedem Test: Aufräumen
        MainData.Groups.Clear();
        MainData.Users.Clear();
        MainData.Tasks.Clear();
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_InitializesDefaultValues()
    {
        // Act
        var viewModel = new AddGroupWindowViewModel();

        // Assert
        Assert.Equal(string.Empty, viewModel.Name);
        Assert.Equal(string.Empty, viewModel.Description);
        Assert.NotNull(viewModel.UserNames);
        Assert.Empty(viewModel.UserNames);
    }

    [Fact]
    public void Constructor_InitializesCommands()
    {
        // Act
        var viewModel = new AddGroupWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.SaveCommand);
        Assert.NotNull(viewModel.CancelCommand);
    }

    #endregion

    #region Name Property Tests

    [Fact]
    public void Name_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Name))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Name = "Development Team";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("Development Team", viewModel.Name);
    }

    [Fact]
    public void Name_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        viewModel.Name = "Development Team";
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Name))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Name = "Development Team";

        // Assert
        Assert.False(propertyChangedRaised);
    }

    [Fact]
    public void Name_CanContainSpecialCharacters()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        var specialName = "Team: Entwicklung & Test (2026)";

        // Act
        viewModel.Name = specialName;

        // Assert
        Assert.Equal(specialName, viewModel.Name);
    }

    [Fact]
    public void Name_CanBeEmpty()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        viewModel.Name = "Some Name";

        // Act
        viewModel.Name = "";

        // Assert
        Assert.Equal("", viewModel.Name);
    }

    [Fact]
    public void Name_CanBeLong()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        var longName = new string('A', 500);

        // Act
        viewModel.Name = longName;

        // Assert
        Assert.Equal(longName, viewModel.Name);
    }

    #endregion

    #region Description Property Tests

    [Fact]
    public void Description_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Description))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Description = "This is a test group description.";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("This is a test group description.", viewModel.Description);
    }

    [Fact]
    public void Description_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        viewModel.Description = "Same Description";
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Description))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Description = "Same Description";

        // Assert
        Assert.False(propertyChangedRaised);
    }

    [Fact]
    public void Description_CanBeMultiLine()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        var multiLineDesc = "Line 1\nLine 2\nLine 3\n\nLine 5";

        // Act
        viewModel.Description = multiLineDesc;

        // Assert
        Assert.Equal(multiLineDesc, viewModel.Description);
    }

    [Fact]
    public void Description_CanBeEmpty()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        viewModel.Description = "Some Description";

        // Act
        viewModel.Description = "";

        // Assert
        Assert.Equal("", viewModel.Description);
    }

    [Fact]
    public void Description_CanBeLong()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        var longDescription = new string('A', 5000);

        // Act
        viewModel.Description = longDescription;

        // Assert
        Assert.Equal(longDescription, viewModel.Description);
    }

    [Fact]
    public void Description_CanContainSpecialCharacters()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        var specialDesc = "Description with: <tags>, \"quotes\", & äöü ß";

        // Act
        viewModel.Description = specialDesc;

        // Assert
        Assert.Equal(specialDesc, viewModel.Description);
    }

    #endregion

    #region UserNames Property Tests

    [Fact]
    public void UserNames_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.UserNames))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.UserNames = new List<string> { "Max Mustermann", "Anna Schmidt" };

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal(2, viewModel.UserNames.Count);
    }

    [Fact]
    public void UserNames_SetSameList_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        var users = new List<string> { "User 1" };
        viewModel.UserNames = users;
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.UserNames))
                propertyChangedRaised = true;
        };

        // Act - Setzen einer neuen Liste (nicht der gleichen Referenz)
        viewModel.UserNames = new List<string> { "User 1" };

        // Assert
        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void UserNames_CanBeSetToEmptyList()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        viewModel.UserNames = new List<string> { "User 1", "User 2" };

        // Act
        viewModel.UserNames = new List<string>();

        // Assert
        Assert.Empty(viewModel.UserNames);
    }

    [Fact]
    public void UserNames_CanContainMultipleUsers()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        var users = new List<string>();
        for (int i = 1; i <= 100; i++)
        {
            users.Add($"User {i}");
        }

        // Act
        viewModel.UserNames = users;

        // Assert
        Assert.Equal(100, viewModel.UserNames.Count);
    }

    [Fact]
    public void UserNames_CanContainDuplicates()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        var users = new List<string> { "User 1", "User 1", "User 2" };

        // Act
        viewModel.UserNames = users;

        // Assert
        Assert.Equal(3, viewModel.UserNames.Count);
    }

    #endregion

    #region RequestClose Event Tests

    [Fact]
    public void CancelCommand_InvokesRequestClose()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        bool requestCloseInvoked = false;
        viewModel.RequestClose += () => requestCloseInvoked = true;

        // Act
        viewModel.CancelCommand.Execute(null);

        // Assert
        Assert.True(requestCloseInvoked);
    }

    [Fact]
    public void RequestClose_CanHaveMultipleHandlers()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        int handlerCallCount = 0;
        viewModel.RequestClose += () => handlerCallCount++;
        viewModel.RequestClose += () => handlerCallCount++;
        viewModel.RequestClose += () => handlerCallCount++;

        // Act
        viewModel.CancelCommand.Execute(null);

        // Assert
        Assert.Equal(3, handlerCallCount);
    }

    #endregion

    #region Data Binding Tests

    [Fact]
    public void AllProperties_CanBeSetInSequence()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();

        // Act
        viewModel.Name = "Test Group";
        viewModel.Description = "Test Description";
        viewModel.UserNames = new List<string> { "User 1", "User 2" };

        // Assert
        Assert.Equal("Test Group", viewModel.Name);
        Assert.Equal("Test Description", viewModel.Description);
        Assert.Equal(2, viewModel.UserNames.Count);
    }

    [Fact]
    public void PropertyChanged_FiredForCorrectProperty()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        var changedProperties = new List<string>();
        viewModel.PropertyChanged += (sender, args) =>
        {
            changedProperties.Add(args.PropertyName!);
        };

        // Act
        viewModel.Name = "Group Name";
        viewModel.Description = "Group Description";
        viewModel.UserNames = new List<string> { "User" };

        // Assert
        Assert.Contains("Name", changedProperties);
        Assert.Contains("Description", changedProperties);
        Assert.Contains("UserNames", changedProperties);
        Assert.Equal(3, changedProperties.Count);
    }

    [Fact]
    public void PropertyChanged_NotFiredWhenValueUnchanged()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        viewModel.Name = "Initial Name";
        viewModel.Description = "Initial Description";
        
        var changedProperties = new List<string>();
        viewModel.PropertyChanged += (sender, args) =>
        {
            changedProperties.Add(args.PropertyName!);
        };

        // Act - Set same values
        viewModel.Name = "Initial Name";
        viewModel.Description = "Initial Description";

        // Assert
        Assert.Empty(changedProperties);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ViewModel_CanHandleRapidPropertyChanges()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();
        int changeCount = 0;
        viewModel.PropertyChanged += (sender, args) => changeCount++;

        // Act
        for (int i = 0; i < 1000; i++)
        {
            viewModel.Name = $"Name {i}";
        }

        // Assert
        Assert.Equal(1000, changeCount);
        Assert.Equal("Name 999", viewModel.Name);
    }

    [Fact]
    public void ViewModel_PropertiesAreIndependent()
    {
        // Arrange
        var viewModel = new AddGroupWindowViewModel();

        // Act
        viewModel.Name = "Name Only";

        // Assert - Andere Properties bleiben unverändert
        Assert.Equal("Name Only", viewModel.Name);
        Assert.Equal(string.Empty, viewModel.Description);
        Assert.Empty(viewModel.UserNames);
    }

    #endregion
}
