using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.Generic;

namespace TaskDesk_version2.Tests.Tests.Models;

/// <summary>
/// Unit-Tests für die Settings-Klasse
/// </summary>
public class SettingsTests : IDisposable
{
    public SettingsTests()
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
        var settings = new Settings();

        // Assert
        Assert.Equal(0, settings.LastLoggedInUserId);
        Assert.NotNull(settings.SavedUserIds);
        Assert.Empty(settings.SavedUserIds);
        Assert.False(settings.IsThemeDark);
    }

    [Fact]
    public void Constructor_SavedUserIds_IsEmptyList()
    {
        // Act
        var settings = new Settings();

        // Assert
        Assert.IsType<List<int>>(settings.SavedUserIds);
        Assert.Empty(settings.SavedUserIds);
    }

    #endregion

    #region Property Tests

    [Fact]
    public void LastLoggedInUserId_CanBeSet()
    {
        // Arrange
        var settings = new Settings();

        // Act
        settings.LastLoggedInUserId = 42;

        // Assert
        Assert.Equal(42, settings.LastLoggedInUserId);
    }

    [Fact]
    public void IsThemeDark_CanBeToggled()
    {
        // Arrange
        var settings = new Settings();

        // Act & Assert
        Assert.False(settings.IsThemeDark);
        
        settings.IsThemeDark = true;
        Assert.True(settings.IsThemeDark);
        
        settings.IsThemeDark = false;
        Assert.False(settings.IsThemeDark);
    }

    [Fact]
    public void SavedUserIds_CanAddIds()
    {
        // Arrange
        var settings = new Settings();

        // Act
        settings.SavedUserIds.Add(1);
        settings.SavedUserIds.Add(2);
        settings.SavedUserIds.Add(3);

        // Assert
        Assert.Equal(3, settings.SavedUserIds.Count);
        Assert.Contains(1, settings.SavedUserIds);
        Assert.Contains(2, settings.SavedUserIds);
        Assert.Contains(3, settings.SavedUserIds);
    }

    [Fact]
    public void SavedUserIds_CanBeReplaced()
    {
        // Arrange
        var settings = new Settings();
        settings.SavedUserIds.Add(1);

        // Act
        settings.SavedUserIds = new List<int> { 5, 6, 7 };

        // Assert
        Assert.Equal(3, settings.SavedUserIds.Count);
        Assert.DoesNotContain(1, settings.SavedUserIds);
        Assert.Contains(5, settings.SavedUserIds);
    }

    #endregion

    #region GetLastLoggedInUser Tests

    [Fact]
    public void GetLastLoggedInUser_WithValidUserId_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, FullName = "Test User", Email = "test@test.com" };
        MainData.Users.Add(user);
        var settings = new Settings { LastLoggedInUserId = 1 };

        // Act
        var result = settings.GetLastLoggedInUser();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.FullName);
    }

    [Fact]
    public void GetLastLoggedInUser_WithInvalidUserId_ReturnsNull()
    {
        // Arrange
        MainData.Users.Clear();
        var settings = new Settings { LastLoggedInUserId = 999 };

        // Act
        var result = settings.GetLastLoggedInUser();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetLastLoggedInUser_WithZeroId_ReturnsNull()
    {
        // Arrange
        MainData.Users.Clear();
        var settings = new Settings();

        // Act
        var result = settings.GetLastLoggedInUser();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetLastLoggedInUser_WithMultipleUsers_ReturnsCorrectUser()
    {
        // Arrange
        MainData.Users.Add(new User { Id = 1, FullName = "User 1" });
        MainData.Users.Add(new User { Id = 2, FullName = "User 2" });
        MainData.Users.Add(new User { Id = 3, FullName = "User 3" });
        var settings = new Settings { LastLoggedInUserId = 2 };

        // Act
        var result = settings.GetLastLoggedInUser();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("User 2", result.FullName);
    }

    #endregion

    #region GetSavedUserEmails Tests

    [Fact]
    public void GetSavedUserEmails_WithNoSavedUsers_ReturnsEmptyList()
    {
        // Arrange
        var settings = new Settings();

        // Act
        var result = settings.GetSavedUserEmails();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetSavedUserEmails_WithValidSavedUsers_ReturnsEmails()
    {
        // Arrange
        MainData.Users.Add(new User { Id = 1, FullName = "User 1", Email = "user1@test.com" });
        MainData.Users.Add(new User { Id = 2, FullName = "User 2", Email = "user2@test.com" });
        var settings = new Settings();
        settings.SavedUserIds.Add(1);
        settings.SavedUserIds.Add(2);

        // Act
        var result = settings.GetSavedUserEmails();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains("user1@test.com", result);
        Assert.Contains("user2@test.com", result);
    }

    [Fact]
    public void GetSavedUserEmails_WithInvalidUserIds_SkipsInvalidUsers()
    {
        // Arrange
        MainData.Users.Add(new User { Id = 1, FullName = "User 1", Email = "user1@test.com" });
        var settings = new Settings();
        settings.SavedUserIds.Add(1);
        settings.SavedUserIds.Add(999); // Nicht existierende ID

        // Act
        var result = settings.GetSavedUserEmails();

        // Assert
        Assert.Single(result);
        Assert.Contains("user1@test.com", result);
    }

    [Fact]
    public void GetSavedUserEmails_WithOnlyInvalidUserIds_ReturnsEmptyList()
    {
        // Arrange
        MainData.Users.Clear();
        var settings = new Settings();
        settings.SavedUserIds.Add(999);
        settings.SavedUserIds.Add(998);

        // Act
        var result = settings.GetSavedUserEmails();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetSavedUserEmails_PreservesOrder()
    {
        // Arrange
        MainData.Users.Add(new User { Id = 3, FullName = "User 3", Email = "user3@test.com" });
        MainData.Users.Add(new User { Id = 1, FullName = "User 1", Email = "user1@test.com" });
        MainData.Users.Add(new User { Id = 2, FullName = "User 2", Email = "user2@test.com" });
        var settings = new Settings();
        settings.SavedUserIds.Add(3);
        settings.SavedUserIds.Add(1);
        settings.SavedUserIds.Add(2);

        // Act
        var result = settings.GetSavedUserEmails();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("user3@test.com", result[0]);
        Assert.Equal("user1@test.com", result[1]);
        Assert.Equal("user2@test.com", result[2]);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Settings_CanHandleDuplicateSavedUserIds()
    {
        // Arrange
        MainData.Users.Add(new User { Id = 1, FullName = "User 1", Email = "user1@test.com" });
        var settings = new Settings();
        settings.SavedUserIds.Add(1);
        settings.SavedUserIds.Add(1);
        settings.SavedUserIds.Add(1);

        // Act
        var result = settings.GetSavedUserEmails();

        // Assert
        Assert.Equal(3, result.Count); // Duplikate werden nicht gefiltert
        Assert.All(result, email => Assert.Equal("user1@test.com", email));
    }

    [Fact]
    public void Settings_NegativeUserId_HandledCorrectly()
    {
        // Arrange
        var settings = new Settings { LastLoggedInUserId = -1 };

        // Act
        var result = settings.GetLastLoggedInUser();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Settings_LargeLastLoggedInUserId_HandledCorrectly()
    {
        // Arrange
        MainData.Users.Add(new User { Id = int.MaxValue, FullName = "Max User", Email = "max@test.com" });
        var settings = new Settings { LastLoggedInUserId = int.MaxValue };

        // Act
        var result = settings.GetLastLoggedInUser();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Max User", result.FullName);
    }

    #endregion
}
