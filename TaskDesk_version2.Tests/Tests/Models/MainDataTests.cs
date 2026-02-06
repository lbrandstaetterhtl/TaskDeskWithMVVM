using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.ObjectModel;
using Task = TaskDesk_version2.Models.Task;

namespace TaskDesk_version2.Tests.Tests.Models;

/// <summary>
/// Unit-Tests für die MainData-Klasse (statische Properties)
/// </summary>
public class MainDataTests : IDisposable
{
    public MainDataTests()
    {
        // Vor jedem Test: Testdaten initialisieren
        MainData.Tasks.Clear();
        MainData.Users.Clear();
        MainData.Groups.Clear();
        MainData.CurrentUser = null!;
    }

    public void Dispose()
    {
        // Nach jedem Test: Aufräumen
        MainData.Tasks.Clear();
        MainData.Users.Clear();
        MainData.Groups.Clear();
        MainData.CurrentUser = null!;
    }

    #region Static Properties Tests

    [Fact]
    public void Tasks_IsNotNull()
    {
        // Assert
        Assert.NotNull(MainData.Tasks);
    }

    [Fact]
    public void Users_IsNotNull()
    {
        // Assert
        Assert.NotNull(MainData.Users);
    }

    [Fact]
    public void Groups_IsNotNull()
    {
        // Assert
        Assert.NotNull(MainData.Groups);
    }

    [Fact]
    public void Settings_IsNotNull()
    {
        // Assert
        Assert.NotNull(MainData.Settings);
    }

    [Fact]
    public void DataPath_IsNotEmpty()
    {
        // Assert
        Assert.NotNull(MainData.DataPath);
        Assert.NotEmpty(MainData.DataPath);
    }

    [Fact]
    public void DataPath_ContainsTaskDeskData()
    {
        // Assert
        Assert.Contains("TaskDeskData", MainData.DataPath);
    }

    #endregion

    #region Tasks Collection Tests

    [Fact]
    public void Tasks_CanAddTask()
    {
        // Act
        MainData.Tasks.Add(new Task { Id = 1, Title = "Test Task" });

        // Assert
        Assert.Single(MainData.Tasks);
    }

    [Fact]
    public void Tasks_CanRemoveTask()
    {
        // Arrange
        var task = new Task { Id = 1, Title = "Test Task" };
        MainData.Tasks.Add(task);

        // Act
        MainData.Tasks.Remove(task);

        // Assert
        Assert.Empty(MainData.Tasks);
    }

    [Fact]
    public void Tasks_CanClear()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1" });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2" });

        // Act
        MainData.Tasks.Clear();

        // Assert
        Assert.Empty(MainData.Tasks);
    }

    [Fact]
    public void Tasks_IsObservableCollection()
    {
        // Assert
        Assert.IsType<ObservableCollection<Task>>(MainData.Tasks);
    }

    #endregion

    #region Users Collection Tests

    [Fact]
    public void Users_CanAddUser()
    {
        // Act
        MainData.Users.Add(new User { Id = 1, FullName = "Test User" });

        // Assert
        Assert.Single(MainData.Users);
    }

    [Fact]
    public void Users_CanRemoveUser()
    {
        // Arrange
        var user = new User { Id = 1, FullName = "Test User" };
        MainData.Users.Add(user);

        // Act
        MainData.Users.Remove(user);

        // Assert
        Assert.Empty(MainData.Users);
    }

    [Fact]
    public void Users_CanClear()
    {
        // Arrange
        MainData.Users.Add(new User { Id = 1, FullName = "User 1" });
        MainData.Users.Add(new User { Id = 2, FullName = "User 2" });

        // Act
        MainData.Users.Clear();

        // Assert
        Assert.Empty(MainData.Users);
    }

    [Fact]
    public void Users_IsObservableCollection()
    {
        // Assert
        Assert.IsType<ObservableCollection<User>>(MainData.Users);
    }

    #endregion

    #region Groups Collection Tests

    [Fact]
    public void Groups_CanAddGroup()
    {
        // Act
        MainData.Groups.Add(new Group { Id = 1, Name = "Test Group" });

        // Assert
        Assert.Single(MainData.Groups);
    }

    [Fact]
    public void Groups_CanRemoveGroup()
    {
        // Arrange
        var group = new Group { Id = 1, Name = "Test Group" };
        MainData.Groups.Add(group);

        // Act
        MainData.Groups.Remove(group);

        // Assert
        Assert.Empty(MainData.Groups);
    }

    [Fact]
    public void Groups_CanClear()
    {
        // Arrange
        MainData.Groups.Add(new Group { Id = 1, Name = "Group 1" });
        MainData.Groups.Add(new Group { Id = 2, Name = "Group 2" });

        // Act
        MainData.Groups.Clear();

        // Assert
        Assert.Empty(MainData.Groups);
    }

    [Fact]
    public void Groups_IsObservableCollection()
    {
        // Assert
        Assert.IsType<ObservableCollection<Group>>(MainData.Groups);
    }

    #endregion

    #region CurrentUser Tests

    [Fact]
    public void CurrentUser_CanBeSet()
    {
        // Arrange
        var user = new User { Id = 1, FullName = "Current User" };

        // Act
        MainData.CurrentUser = user;

        // Assert
        Assert.NotNull(MainData.CurrentUser);
        Assert.Equal("Current User", MainData.CurrentUser.FullName);
    }

    [Fact]
    public void CurrentUser_CanBeNull()
    {
        // Arrange
        MainData.CurrentUser = new User { Id = 1, FullName = "Test" };

        // Act
        MainData.CurrentUser = null!;

        // Assert
        Assert.Null(MainData.CurrentUser);
    }

    [Fact]
    public void CurrentUser_DefaultsToNull()
    {
        // Assert
        Assert.Null(MainData.CurrentUser);
    }

    #endregion

    #region Settings Tests

    [Fact]
    public void Settings_CanBeModified()
    {
        // Act
        MainData.Settings.IsThemeDark = true;
        MainData.Settings.LastLoggedInUserId = 5;

        // Assert
        Assert.True(MainData.Settings.IsThemeDark);
        Assert.Equal(5, MainData.Settings.LastLoggedInUserId);
    }

    [Fact]
    public void Settings_IsSettings()
    {
        // Assert
        Assert.IsType<Settings>(MainData.Settings);
    }

    #endregion

    #region Collection Interaction Tests

    [Fact]
    public void Collections_AreIndependent()
    {
        // Arrange & Act
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task" });
        MainData.Users.Add(new User { Id = 1, FullName = "User" });
        MainData.Groups.Add(new Group { Id = 1, Name = "Group" });

        // Assert
        Assert.Single(MainData.Tasks);
        Assert.Single(MainData.Users);
        Assert.Single(MainData.Groups);
    }

    [Fact]
    public void Collections_CanBeAccessedSimultaneously()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task" });
        MainData.Users.Add(new User { Id = 1, FullName = "User" });
        MainData.Groups.Add(new Group { Id = 1, Name = "Group" });

        // Act
        var taskCount = MainData.Tasks.Count;
        var userCount = MainData.Users.Count;
        var groupCount = MainData.Groups.Count;

        // Assert
        Assert.Equal(1, taskCount);
        Assert.Equal(1, userCount);
        Assert.Equal(1, groupCount);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Collections_CanHandleLargeNumberOfItems()
    {
        // Act
        for (int i = 0; i < 1000; i++)
        {
            MainData.Tasks.Add(new Task { Id = i, Title = $"Task {i}" });
            MainData.Users.Add(new User { Id = i, FullName = $"User {i}" });
            MainData.Groups.Add(new Group { Id = i, Name = $"Group {i}" });
        }

        // Assert
        Assert.Equal(1000, MainData.Tasks.Count);
        Assert.Equal(1000, MainData.Users.Count);
        Assert.Equal(1000, MainData.Groups.Count);
    }

    [Fact]
    public void CurrentUser_CanBeSwitched()
    {
        // Arrange
        var user1 = new User { Id = 1, FullName = "User 1" };
        var user2 = new User { Id = 2, FullName = "User 2" };

        // Act
        MainData.CurrentUser = user1;
        Assert.Equal("User 1", MainData.CurrentUser.FullName);

        MainData.CurrentUser = user2;
        Assert.Equal("User 2", MainData.CurrentUser.FullName);
    }

    #endregion
}
