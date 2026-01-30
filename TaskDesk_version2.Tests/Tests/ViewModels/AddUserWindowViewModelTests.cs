using TaskDesk_version2.ViewModels;
using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.Generic;

namespace TaskDesk_version2.Tests.Tests.ViewModels;

/// <summary>
/// Unit-Tests für AddUserWindowViewModel
/// </summary>
public class AddUserWindowViewModelTests : IDisposable
{
    public AddUserWindowViewModelTests()
    {
        // Vor jedem Test: Testdaten initialisieren
        MainData.Users.Clear();
        MainData.Groups.Clear();
        MainData.Tasks.Clear();
    }

    public void Dispose()
    {
        // Nach jedem Test: Aufräumen
        MainData.Users.Clear();
        MainData.Groups.Clear();
        MainData.Tasks.Clear();
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_InitializesDefaultValues()
    {
        // Act
        var viewModel = new AddUserWindowViewModel();

        // Assert
        Assert.Equal(string.Empty, viewModel.Fullname);
        Assert.Equal(string.Empty, viewModel.Email);
        Assert.Equal(string.Empty, viewModel.Password);
        Assert.NotNull(viewModel.GroupNames);
        Assert.Empty(viewModel.GroupNames);
    }

    [Fact]
    public void Constructor_InitializesCommands()
    {
        // Act
        var viewModel = new AddUserWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.SaveCommand);
        Assert.NotNull(viewModel.CancelCommand);
    }

    #endregion

    #region Fullname Property Tests

    [Fact]
    public void Fullname_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Fullname))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Fullname = "Max Mustermann";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("Max Mustermann", viewModel.Fullname);
    }

    [Fact]
    public void Fullname_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        viewModel.Fullname = "Max Mustermann";
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Fullname))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Fullname = "Max Mustermann";

        // Assert
        Assert.False(propertyChangedRaised);
    }

    [Fact]
    public void Fullname_CanContainSpecialCharacters()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        var specialName = "Müller-Schmidt, André";

        // Act
        viewModel.Fullname = specialName;

        // Assert
        Assert.Equal(specialName, viewModel.Fullname);
    }

    [Fact]
    public void Fullname_CanBeEmpty()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        viewModel.Fullname = "Some Name";

        // Act
        viewModel.Fullname = "";

        // Assert
        Assert.Equal("", viewModel.Fullname);
    }

    #endregion

    #region Email Property Tests

    [Fact]
    public void Email_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Email))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Email = "test@example.com";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("test@example.com", viewModel.Email);
    }

    [Fact]
    public void Email_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        viewModel.Email = "test@example.com";
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Email))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Email = "test@example.com";

        // Assert
        Assert.False(propertyChangedRaised);
    }

    [Theory]
    [InlineData("user@domain.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("user+tag@domain.org")]
    [InlineData("user123@subdomain.domain.com")]
    public void Email_AcceptsValidEmailFormats(string email)
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();

        // Act
        viewModel.Email = email;

        // Assert
        Assert.Equal(email, viewModel.Email);
    }

    [Fact]
    public void Email_CanBeEmpty()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        viewModel.Email = "test@example.com";

        // Act
        viewModel.Email = "";

        // Assert
        Assert.Equal("", viewModel.Email);
    }

    #endregion

    #region Password Property Tests

    [Fact]
    public void Password_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Password))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Password = "SecurePassword123!";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("SecurePassword123!", viewModel.Password);
    }

    [Fact]
    public void Password_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        viewModel.Password = "Password123";
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.Password))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.Password = "Password123";

        // Assert
        Assert.False(propertyChangedRaised);
    }

    [Fact]
    public void Password_CanContainSpecialCharacters()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        var complexPassword = "P@$$w0rd!#%&*()";

        // Act
        viewModel.Password = complexPassword;

        // Assert
        Assert.Equal(complexPassword, viewModel.Password);
    }

    [Fact]
    public void Password_CanBeEmpty()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        viewModel.Password = "SomePassword";

        // Act
        viewModel.Password = "";

        // Assert
        Assert.Equal("", viewModel.Password);
    }

    [Fact]
    public void Password_CanBeLong()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        var longPassword = new string('A', 1000);

        // Act
        viewModel.Password = longPassword;

        // Assert
        Assert.Equal(longPassword, viewModel.Password);
    }

    #endregion

    #region RoleString Property Tests

    [Fact]
    public void RoleString_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.RoleString))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.RoleString = "Admin";

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal("Admin", viewModel.RoleString);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("User")]
    [InlineData("Read-Only")]
    public void RoleString_CanBeSetToAllValidRoles(string role)
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();

        // Act
        viewModel.RoleString = role;

        // Assert
        Assert.Equal(role, viewModel.RoleString);
    }

    #endregion

    #region GroupNames Property Tests

    [Fact]
    public void GroupNames_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.GroupNames))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.GroupNames = new List<string> { "Development", "Marketing" };

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal(2, viewModel.GroupNames.Count);
    }

    [Fact]
    public void GroupNames_CanBeSetToEmptyList()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        viewModel.GroupNames = new List<string> { "Group 1" };

        // Act
        viewModel.GroupNames = new List<string>();

        // Assert
        Assert.Empty(viewModel.GroupNames);
    }

    [Fact]
    public void GroupNames_CanContainMultipleGroups()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        var groups = new List<string> { "Group 1", "Group 2", "Group 3", "Group 4", "Group 5" };

        // Act
        viewModel.GroupNames = groups;

        // Assert
        Assert.Equal(5, viewModel.GroupNames.Count);
    }

    #endregion

    #region RequestClose Event Tests

    [Fact]
    public void CancelCommand_InvokesRequestClose()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        bool requestCloseInvoked = false;
        viewModel.RequestClose += () => requestCloseInvoked = true;

        // Act
        viewModel.CancelCommand.Execute(null);

        // Assert
        Assert.True(requestCloseInvoked);
    }

    #endregion

    #region Data Binding Tests

    [Fact]
    public void AllProperties_CanBeSetInSequence()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();

        // Act
        viewModel.Fullname = "Test User";
        viewModel.Email = "test@test.com";
        viewModel.Password = "password123";
        viewModel.RoleString = "User";
        viewModel.GroupNames = new List<string> { "Development" };

        // Assert
        Assert.Equal("Test User", viewModel.Fullname);
        Assert.Equal("test@test.com", viewModel.Email);
        Assert.Equal("password123", viewModel.Password);
        Assert.Equal("User", viewModel.RoleString);
        Assert.Single(viewModel.GroupNames);
    }

    [Fact]
    public void PropertyChanged_FiredForCorrectProperty()
    {
        // Arrange
        var viewModel = new AddUserWindowViewModel();
        var changedProperties = new List<string>();
        viewModel.PropertyChanged += (sender, args) =>
        {
            changedProperties.Add(args.PropertyName!);
        };

        // Act
        viewModel.Fullname = "Name";
        viewModel.Email = "email@test.com";
        viewModel.Password = "pass";
        viewModel.RoleString = "Admin";
        viewModel.GroupNames = new List<string> { "Group" };

        // Assert
        Assert.Contains("Fullname", changedProperties);
        Assert.Contains("Email", changedProperties);
        Assert.Contains("Password", changedProperties);
        Assert.Contains("RoleString", changedProperties);
        Assert.Contains("GroupNames", changedProperties);
    }

    #endregion
}
