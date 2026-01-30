using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Task = TaskDesk_version2.Models.Task;

namespace TaskDesk_version2.Tests.Tests.Models;

/// <summary>
/// Unit-Tests für die Group-Klasse und GroupsOperator
/// </summary>
public class GroupTests : IDisposable
{
    public GroupTests()
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

    #region Group Constructor Tests

    [Fact]
    public void GroupConstructor_WithValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var name = "TestGroup";
        var description = "Test Description";
        var userIds = new List<int> { 1, 2, 3 };

        // Act
        var group = new Group(name, description, userIds);

        // Assert
        Assert.Equal(name, group.Name);
        Assert.Equal(description, group.Description);
        Assert.Equal(userIds, group.UserIds);
    }

    [Fact]
    public void GroupConstructor_WithEmptyUserIds_CreatesGroupWithEmptyList()
    {
        // Arrange
        var name = "EmptyGroup";
        var description = "No users";
        var userIds = new List<int>();

        // Act
        var group = new Group(name, description, userIds);

        // Assert
        Assert.Empty(group.UserIds);
    }

    [Fact]
    public void GroupDefaultConstructor_CreatesEmptyGroup()
    {
        // Act
        var group = new Group();

        // Assert
        Assert.NotNull(group.UserIds);
        Assert.NotNull(group.TaskIds);
    }

    #endregion

    #region GetUsersAsString Tests

    [Fact]
    public void GetUsersAsString_WithValidUsers_ReturnsCommaSeperatedNames()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, FullName = "Max Mustermann" },
            new User { Id = 2, FullName = "Anna Schmidt" }
        };
        
        var group = new Group("TestGroup", "Test", new List<int> { 1, 2 });

        // Act
        var result = group.GetUsersAsString(users);

        // Assert
        Assert.Equal("Max Mustermann, Anna Schmidt", result);
    }

    [Fact]
    public void GetUsersAsString_WithSingleUser_ReturnsSingleName()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, FullName = "Max Mustermann" }
        };
        
        var group = new Group("TestGroup", "Test", new List<int> { 1 });

        // Act
        var result = group.GetUsersAsString(users);

        // Assert
        Assert.Equal("Max Mustermann", result);
    }

    [Fact]
    public void GetUsersAsString_WithNoUsers_ReturnsEmptyString()
    {
        // Arrange
        var users = new List<User>();
        var group = new Group("TestGroup", "Test", new List<int>());

        // Act
        var result = group.GetUsersAsString(users);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetUsersAsString_WithNonExistingUserIds_ReturnsEmptyString()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, FullName = "Max Mustermann" }
        };
        
        var group = new Group("TestGroup", "Test", new List<int> { 99, 100 }); // Nicht existierende IDs

        // Act
        var result = group.GetUsersAsString(users);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region GetTasksAsString Tests

    [Fact]
    public void GetTasksAsString_WithValidTasks_ReturnsCommaSeparatedTitles()
    {
        // Arrange
        var tasks = new List<Task>
        {
            new Task { Id = 1, Title = "Task 1" },
            new Task { Id = 2, Title = "Task 2" }
        };
        
        var group = new Group("TestGroup", "Test", new List<int>());
        group.TaskIds = new List<int> { 1, 2 };

        // Act
        var result = group.GetTasksAsString(tasks);

        // Assert
        Assert.Equal("Task 1, Task 2", result);
    }

    [Fact]
    public void GetTasksAsString_WithNoTasks_ReturnsEmptyString()
    {
        // Arrange
        var tasks = new List<Task>();
        var group = new Group("TestGroup", "Test", new List<int>());

        // Act
        var result = group.GetTasksAsString(tasks);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region GroupsOperator.GetIdsFromNames Tests

    [Fact]
    public void GetIdsFromNames_WithValidNames_ReturnsCorrectIds()
    {
        // Arrange
        var groups = new ObservableCollection<Group>
        {
            new Group { Id = 1, Name = "Group A" },
            new Group { Id = 2, Name = "Group B" },
            new Group { Id = 3, Name = "Group C" }
        };
        var names = new List<string> { "Group A", "Group C" };

        // Act
        var result = GroupsOperator.GetIdsFromNames(names, groups);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(3, result);
    }

    [Fact]
    public void GetIdsFromNames_WithNonExistingNames_ReturnsEmptyList()
    {
        // Arrange
        var groups = new ObservableCollection<Group>
        {
            new Group { Id = 1, Name = "Group A" }
        };
        var names = new List<string> { "Non Existing Group" };

        // Act
        var result = GroupsOperator.GetIdsFromNames(names, groups);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetIdsFromNames_WithEmptyNamesList_ReturnsEmptyList()
    {
        // Arrange
        var groups = new ObservableCollection<Group>
        {
            new Group { Id = 1, Name = "Group A" }
        };
        var names = new List<string>();

        // Act
        var result = GroupsOperator.GetIdsFromNames(names, groups);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GroupsOperator.GetNamesFromIds Tests

    [Fact]
    public void GetNamesFromIds_WithValidIds_ReturnsCorrectNames()
    {
        // Arrange
        var groups = new List<Group>
        {
            new Group { Id = 1, Name = "Group A" },
            new Group { Id = 2, Name = "Group B" },
            new Group { Id = 3, Name = "Group C" }
        };
        var ids = new List<int> { 1, 3 };

        // Act
        var result = GroupsOperator.GetNamesFromIds(ids, groups);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains("Group A", result);
        Assert.Contains("Group C", result);
    }

    [Fact]
    public void GetNamesFromIds_WithNonExistingIds_ReturnsEmptyList()
    {
        // Arrange
        var groups = new List<Group>
        {
            new Group { Id = 1, Name = "Group A" }
        };
        var ids = new List<int> { 99, 100 };

        // Act
        var result = GroupsOperator.GetNamesFromIds(ids, groups);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GroupsOperator.GetListFromIds Tests

    [Fact]
    public void GetListFromIds_WithValidIds_ReturnsCorrectGroups()
    {
        // Arrange
        var groups = new ObservableCollection<Group>
        {
            new Group { Id = 1, Name = "Group A" },
            new Group { Id = 2, Name = "Group B" }
        };
        var ids = new List<int> { 1 };

        // Act
        var result = GroupsOperator.GetListFromIds(ids, groups);

        // Assert
        Assert.Single(result);
        Assert.Equal("Group A", result[0].Name);
    }

    #endregion

    #region GroupsOperator.GetNextGroupId Tests

    [Fact]
    public void GetNextGroupId_WithEmptyGroups_Returns1()
    {
        // Arrange
        MainData.Groups.Clear();

        // Act
        var result = GroupsOperator.GetNextGroupId();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetNextGroupId_WithExistingGroups_ReturnsMaxIdPlusOne()
    {
        // Arrange
        MainData.Groups.Clear();
        MainData.Groups.Add(new Group { Id = 5, Name = "Group 1" });
        MainData.Groups.Add(new Group { Id = 3, Name = "Group 2" });

        // Act
        var result = GroupsOperator.GetNextGroupId();

        // Assert
        Assert.Equal(6, result);
    }

    #endregion

    #region GroupsOperator.GetGroupById Tests

    [Fact]
    public void GetGroupById_WithValidId_ReturnsCorrectGroup()
    {
        // Arrange
        MainData.Groups.Clear();
        var group = new Group { Id = 1, Name = "Test Group" };
        MainData.Groups.Add(group);

        // Act
        var result = GroupsOperator.GetGroupById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Group", result.Name);
    }

    [Fact]
    public void GetGroupById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        MainData.Groups.Clear();

        // Act
        var result = GroupsOperator.GetGroupById(999);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GroupsOperator.GetIdsFromList Tests

    [Fact]
    public void GetIdsFromList_WithMatchingGroups_ReturnsCorrectIds()
    {
        // Arrange
        var allGroups = new ObservableCollection<Group>
        {
            new Group { Id = 1, Name = "Group A" },
            new Group { Id = 2, Name = "Group B" }
        };
        var selectedGroups = new ObservableCollection<Group>
        {
            new Group { Id = 1, Name = "Group A" }
        };

        // Act
        var result = GroupsOperator.GetIdsFromList(selectedGroups, allGroups);

        // Assert
        Assert.Single(result);
        Assert.Contains(1, result);
    }

    #endregion
}
