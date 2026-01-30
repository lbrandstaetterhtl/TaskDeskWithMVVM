using TaskDesk_version2.ViewModels;
using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.Generic;

namespace TaskDesk_version2.Tests.Tests.ViewModels;

/// <summary>
/// Unit-Tests für LoginWindowViewModel
/// </summary>
public class LoginWindowViewModelTests : IDisposable
{
    public LoginWindowViewModelTests()
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
        MainData.CurrentUser = null!;
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_InitializesDefaultValues()
    {
        // Act
        var viewModel = new LoginWindowViewModel();

        // Assert
        Assert.Equal(string.Empty, viewModel.Email);
        Assert.Equal(string.Empty, viewModel.Password);
        Assert.False(viewModel.IsValid);
    }

    [Fact]
    public void Constructor_InitializesCommands()
    {
        // Act
        var viewModel = new LoginWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.LoginCommand);
        Assert.NotNull(viewModel.CloseCommand);
    }

    #endregion

    #region Email Property Tests

    [Fact]
    public void Email_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
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
        var viewModel = new LoginWindowViewModel();
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

    [Fact]
    public void Email_CanBeEmpty()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        viewModel.Email = "some@email.com";

        // Act
        viewModel.Email = "";

        // Assert
        Assert.Equal("", viewModel.Email);
    }

    [Theory]
    [InlineData("user@domain.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("user+tag@domain.org")]
    [InlineData("USER@DOMAIN.COM")]
    public void Email_AcceptsVariousFormats(string email)
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();

        // Act
        viewModel.Email = email;

        // Assert
        Assert.Equal(email, viewModel.Email);
    }

    #endregion

    #region Password Property Tests

    [Fact]
    public void Password_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
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
        var viewModel = new LoginWindowViewModel();
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
    public void Password_CanBeEmpty()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        viewModel.Password = "SomePassword";

        // Act
        viewModel.Password = "";

        // Assert
        Assert.Equal("", viewModel.Password);
    }

    [Fact]
    public void Password_CanContainSpecialCharacters()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        var specialPassword = "P@$$w0rd!#%&*()äöü";

        // Act
        viewModel.Password = specialPassword;

        // Assert
        Assert.Equal(specialPassword, viewModel.Password);
    }

    #endregion

    #region IsValid Property Tests

    [Fact]
    public void IsValid_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.IsValid))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.IsValid = true;

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.True(viewModel.IsValid);
    }

    [Fact]
    public void IsValid_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        viewModel.IsValid = true;
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(viewModel.IsValid))
                propertyChangedRaised = true;
        };

        // Act
        viewModel.IsValid = true;

        // Assert
        Assert.False(propertyChangedRaised);
    }

    [Fact]
    public void IsValid_DefaultIsFalse()
    {
        // Arrange & Act
        var viewModel = new LoginWindowViewModel();

        // Assert
        Assert.False(viewModel.IsValid);
    }

    [Fact]
    public void IsValid_CanToggleBetweenTrueAndFalse()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();

        // Act & Assert
        viewModel.IsValid = true;
        Assert.True(viewModel.IsValid);

        viewModel.IsValid = false;
        Assert.False(viewModel.IsValid);

        viewModel.IsValid = true;
        Assert.True(viewModel.IsValid);
    }

    #endregion

    #region RequestClose Event Tests

    [Fact]
    public void CloseCommand_InvokesRequestClose()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        bool requestCloseInvoked = false;
        viewModel.RequestClose += () => requestCloseInvoked = true;

        // Act
        viewModel.CloseCommand.Execute(null);

        // Assert
        Assert.True(requestCloseInvoked);
    }

    #endregion

    #region Data Binding Tests

    [Fact]
    public void AllProperties_CanBeSetInSequence()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();

        // Act
        viewModel.Email = "test@test.com";
        viewModel.Password = "password123";
        viewModel.IsValid = true;

        // Assert
        Assert.Equal("test@test.com", viewModel.Email);
        Assert.Equal("password123", viewModel.Password);
        Assert.True(viewModel.IsValid);
    }

    [Fact]
    public void PropertyChanged_FiredForCorrectProperty()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        var changedProperties = new List<string>();
        viewModel.PropertyChanged += (sender, args) =>
        {
            changedProperties.Add(args.PropertyName!);
        };

        // Act
        viewModel.Email = "email@test.com";
        viewModel.Password = "pass";
        viewModel.IsValid = true;

        // Assert
        Assert.Contains("Email", changedProperties);
        Assert.Contains("Password", changedProperties);
        Assert.Contains("IsValid", changedProperties);
        Assert.Equal(3, changedProperties.Count);
    }

    #endregion

    #region Credential Reset Tests

    [Fact]
    public void Credentials_CanBeResetToEmpty()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        viewModel.Email = "test@test.com";
        viewModel.Password = "password";

        // Act
        viewModel.Email = "";
        viewModel.Password = "";

        // Assert
        Assert.Equal("", viewModel.Email);
        Assert.Equal("", viewModel.Password);
    }

    [Fact]
    public void IsValid_CanBeResetAfterFailedLogin()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        viewModel.IsValid = false;

        // Act - Simuliere neuen Login-Versuch
        viewModel.Email = "new@email.com";
        viewModel.Password = "newpassword";

        // Assert - IsValid sollte immer noch false sein bis Login erfolgreich
        Assert.False(viewModel.IsValid);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ViewModel_HandlesWhitespaceInEmail()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();

        // Act
        viewModel.Email = "  test@test.com  ";

        // Assert - ViewModel speichert den Wert wie eingegeben
        Assert.Equal("  test@test.com  ", viewModel.Email);
    }

    [Fact]
    public void ViewModel_HandlesWhitespaceInPassword()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();

        // Act
        viewModel.Password = "  password  ";

        // Assert - ViewModel speichert den Wert wie eingegeben
        Assert.Equal("  password  ", viewModel.Password);
    }

    [Fact]
    public void ViewModel_HandlesVeryLongCredentials()
    {
        // Arrange
        var viewModel = new LoginWindowViewModel();
        var longEmail = new string('a', 1000) + "@test.com";
        var longPassword = new string('b', 1000);

        // Act
        viewModel.Email = longEmail;
        viewModel.Password = longPassword;

        // Assert
        Assert.Equal(longEmail, viewModel.Email);
        Assert.Equal(longPassword, viewModel.Password);
    }

    #endregion
}
