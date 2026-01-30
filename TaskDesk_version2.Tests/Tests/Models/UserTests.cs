using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Task = TaskDesk_version2.Models.Task;

namespace TaskDesk_version2.Tests.Tests.Models;

/// <summary>
/// Unit-Tests für die User-Klasse und UsersOperator
/// </summary>
public class UserTests : IDisposable
{
    public UserTests()
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

    #region User Constructor Tests

    [Fact]
    public void UserConstructor_WithValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var fullName = "Max Mustermann";
        var email = "max@example.com";
        var password = "securePassword123";
        var role = UserRole.User;
        var groupIds = new List<int> { 1, 2 };

        // Act
        var user = new User(fullName, email, password, role, groupIds);

        // Assert
        Assert.Equal(fullName, user.FullName);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
        Assert.Equal(role, user.Role);
        Assert.Equal(groupIds, user.GroupIds);
    }

    [Fact]
    public void UserConstructor_WithAdminRole_CreatesAdminUser()
    {
        // Arrange & Act
        var user = new User("Admin User", "admin@test.com", "admin123", UserRole.Admin, new List<int>());

        // Assert
        Assert.Equal(UserRole.Admin, user.Role);
    }

    [Fact]
    public void UserConstructor_WithReadOnlyRole_CreatesReadOnlyUser()
    {
        // Arrange & Act
        var user = new User("ReadOnly User", "readonly@test.com", "readonly123", UserRole.ReadOnly, new List<int>());

        // Assert
        Assert.Equal(UserRole.ReadOnly, user.Role);
    }

    [Fact]
    public void UserDefaultConstructor_CreatesUserWithEmptyLists()
    {
        // Act
        var user = new User();

        // Assert
        Assert.NotNull(user.GroupIds);
        Assert.NotNull(user.TaskIds);
        Assert.Empty(user.GroupIds);
        Assert.Empty(user.TaskIds);
    }

    [Fact]
    public void UserConstructor_AssignsUniqueId()
    {
        // Arrange
        MainData.Users.Clear();

        // Act
        var user1 = new User("User 1", "user1@test.com", "pass1", UserRole.User, new List<int>());
        MainData.Users.Add(user1);
        var user2 = new User("User 2", "user2@test.com", "pass2", UserRole.User, new List<int>());

        // Assert
        Assert.NotEqual(user1.Id, user2.Id);
    }

    #endregion

    #region GetRoleAsString Tests

    [Fact]
    public void GetRoleAsString_AdminRole_ReturnsAdminString()
    {
        // Arrange
        var user = new User { Role = UserRole.Admin };

        // Act
        var result = user.GetRoleAsString();

        // Assert
        Assert.Equal("Admin", result);
    }

    [Fact]
    public void GetRoleAsString_UserRole_ReturnsUserString()
    {
        // Arrange
        var user = new User { Role = UserRole.User };

        // Act
        var result = user.GetRoleAsString();

        // Assert
        Assert.Equal("User", result);
    }

    [Fact]
    public void GetRoleAsString_ReadOnlyRole_ReturnsReadOnlyString()
    {
        // Arrange
        var user = new User { Role = UserRole.ReadOnly };

        // Act
        var result = user.GetRoleAsString();

        // Assert
        Assert.Equal("Read-Only", result);
    }

    #endregion

    #region GetRoleFromString Tests

    [Fact]
    public void GetRoleFromString_ValidAdminString_ReturnsAdminRole()
    {
        // Arrange
        var user = new User();

        // Act
        var result = user.GetRoleFromString("Admin");

        // Assert
        Assert.Equal(UserRole.Admin, result);
    }

    [Fact]
    public void GetRoleFromString_InvalidString_ThrowsKeyNotFoundException()
    {
        // Arrange
        var user = new User();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => user.GetRoleFromString("InvalidRole"));
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
            new Task { Id = 2, Title = "Task 2" },
            new Task { Id = 3, Title = "Task 3" }
        };
        
        var user = new User { TaskIds = new List<int> { 1, 3 } };

        // Act
        var result = user.GetTasksAsString(tasks);

        // Assert
        Assert.Equal("Task 1, Task 3", result);
    }

    [Fact]
    public void GetTasksAsString_WithNoTasks_ReturnsEmptyString()
    {
        // Arrange
        var tasks = new List<Task>();
        var user = new User { TaskIds = new List<int>() };

        // Act
        var result = user.GetTasksAsString(tasks);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetTasksAsString_WithNonExistingTaskIds_ReturnsEmptyString()
    {
        // Arrange
        var tasks = new List<Task>
        {
            new Task { Id = 1, Title = "Task 1" }
        };
        
        var user = new User { TaskIds = new List<int> { 99, 100 } };

        // Act
        var result = user.GetTasksAsString(tasks);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region GetGroupsAsString Tests

    [Fact]
    public void GetGroupsAsString_WithValidGroups_ReturnsCommaSeparatedNames()
    {
        // Arrange
        var groups = new List<Group>
        {
            new Group { Id = 1, Name = "Development" },
            new Group { Id = 2, Name = "Marketing" },
            new Group { Id = 3, Name = "Sales" }
        };
        
        var user = new User { GroupIds = new List<int> { 1, 3 } };

        // Act
        var result = user.GetGroupsAsString(groups);

        // Assert
        Assert.Equal("Development, Sales", result);
    }

    [Fact]
    public void GetGroupsAsString_WithNoGroups_ReturnsEmptyString()
    {
        // Arrange
        var groups = new List<Group>();
        var user = new User { GroupIds = new List<int>() };

        // Act
        var result = user.GetGroupsAsString(groups);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region UsersOperator.GetIdsFromNames Tests

    [Fact]
    public void GetIdsFromNames_WithValidNames_ReturnsCorrectIds()
    {
        // Arrange
        var users = new ObservableCollection<User>
        {
            new User { Id = 1, FullName = "Max Mustermann" },
            new User { Id = 2, FullName = "Anna Schmidt" },
            new User { Id = 3, FullName = "Peter Müller" }
        };
        var names = new List<string> { "Max Mustermann", "Peter Müller" };

        // Act
        var result = UsersOperator.GetIdsFromNames(names, users);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(3, result);
    }

    [Fact]
    public void GetIdsFromNames_WithNonExistingNames_ReturnsEmptyList()
    {
        // Arrange
        var users = new ObservableCollection<User>
        {
            new User { Id = 1, FullName = "Max Mustermann" }
        };
        var names = new List<string> { "Non Existing User" };

        // Act
        var result = UsersOperator.GetIdsFromNames(names, users);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetIdsFromNames_WithEmptyNamesList_ReturnsEmptyList()
    {
        // Arrange
        var users = new ObservableCollection<User>
        {
            new User { Id = 1, FullName = "Max Mustermann" }
        };
        var names = new List<string>();

        // Act
        var result = UsersOperator.GetIdsFromNames(names, users);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetIdsFromNames_IsCaseSensitive()
    {
        // Arrange
        var users = new ObservableCollection<User>
        {
            new User { Id = 1, FullName = "Max Mustermann" }
        };
        var names = new List<string> { "max mustermann" };

        // Act
        var result = UsersOperator.GetIdsFromNames(names, users);

        // Assert
        Assert.Empty(result); // Sollte leer sein wegen Groß-/Kleinschreibung
    }

    #endregion

    #region UsersOperator.GetListFromIds Tests

    [Fact]
    public void GetListFromIds_WithValidIds_ReturnsCorrectUsers()
    {
        // Arrange
        var users = new ObservableCollection<User>
        {
            new User { Id = 1, FullName = "Max Mustermann" },
            new User { Id = 2, FullName = "Anna Schmidt" }
        };
        var ids = new List<int> { 1 };

        // Act
        var result = UsersOperator.GetListFromIds(ids, users);

        // Assert
        Assert.Single(result);
        Assert.Equal("Max Mustermann", result[0].FullName);
    }

    [Fact]
    public void GetListFromIds_WithNonExistingIds_ReturnsEmptyList()
    {
        // Arrange
        var users = new ObservableCollection<User>
        {
            new User { Id = 1, FullName = "Max Mustermann" }
        };
        var ids = new List<int> { 99, 100 };

        // Act
        var result = UsersOperator.GetListFromIds(ids, users);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetListFromIds_WithMultipleIds_ReturnsAllMatchingUsers()
    {
        // Arrange
        var users = new ObservableCollection<User>
        {
            new User { Id = 1, FullName = "User 1" },
            new User { Id = 2, FullName = "User 2" },
            new User { Id = 3, FullName = "User 3" }
        };
        var ids = new List<int> { 1, 3 };

        // Act
        var result = UsersOperator.GetListFromIds(ids, users);

        // Assert
        Assert.Equal(2, result.Count);
    }

    #endregion

    #region UsersOperator.GetIdsFromList Tests

    [Fact]
    public void GetIdsFromList_WithMatchingUsers_ReturnsCorrectIds()
    {
        // Arrange
        var allUsers = new ObservableCollection<User>
        {
            new User { Id = 1, FullName = "User 1" },
            new User { Id = 2, FullName = "User 2" }
        };
        var selectedUsers = new ObservableCollection<User>
        {
            new User { Id = 1, FullName = "User 1" }
        };

        // Act
        var result = UsersOperator.GetIdsFromList(selectedUsers, allUsers);

        // Assert
        Assert.Single(result);
        Assert.Contains(1, result);
    }

    #endregion

    #region UsersOperator.GetNextUserId Tests

    [Fact]
    public void GetNextUserId_WithEmptyUsers_Returns1()
    {
        // Arrange
        MainData.Users.Clear();

        // Act
        var result = UsersOperator.GetNextUserId();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetNextUserId_WithExistingUsers_ReturnsMaxIdPlusOne()
    {
        // Arrange
        MainData.Users.Clear();
        MainData.Users.Add(new User { Id = 5, FullName = "User 1" });
        MainData.Users.Add(new User { Id = 3, FullName = "User 2" });

        // Act
        var result = UsersOperator.GetNextUserId();

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void GetNextUserId_WithGapsInIds_ReturnsMaxIdPlusOne()
    {
        // Arrange
        MainData.Users.Clear();
        MainData.Users.Add(new User { Id = 1, FullName = "User 1" });
        MainData.Users.Add(new User { Id = 100, FullName = "User 2" });

        // Act
        var result = UsersOperator.GetNextUserId();

        // Assert
        Assert.Equal(101, result);
    }

    #endregion

    #region UsersOperator.GetUserById Tests

    [Fact]
    public void GetUserById_WithValidId_ReturnsCorrectUser()
    {
        // Arrange
        MainData.Users.Clear();
        var user = new User { Id = 1, FullName = "Test User", Email = "test@test.com" };
        MainData.Users.Add(user);

        // Act
        var result = UsersOperator.GetUserById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.FullName);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public void GetUserById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        MainData.Users.Clear();

        // Act
        var result = UsersOperator.GetUserById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUserById_WithMultipleUsers_ReturnsCorrectOne()
    {
        // Arrange
        MainData.Users.Clear();
        MainData.Users.Add(new User { Id = 1, FullName = "User 1" });
        MainData.Users.Add(new User { Id = 2, FullName = "User 2" });
        MainData.Users.Add(new User { Id = 3, FullName = "User 3" });

        // Act
        var result = UsersOperator.GetUserById(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("User 2", result.FullName);
    }

    #endregion

    #region UsersOperator.GetUserByEmail Tests

    [Fact]
    public void GetUserByEmail_WithValidEmail_ReturnsCorrectUser()
    {
        // Arrange
        MainData.Users.Clear();
        var user = new User { Id = 1, FullName = "Test User", Email = "test@example.com" };
        MainData.Users.Add(user);

        // Act
        var result = UsersOperator.GetUserByEmail("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.FullName);
    }

    [Fact]
    public void GetUserByEmail_WithInvalidEmail_ReturnsNull()
    {
        // Arrange
        MainData.Users.Clear();

        // Act
        var result = UsersOperator.GetUserByEmail("nonexisting@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUserByEmail_IsCaseSensitive()
    {
        // Arrange
        MainData.Users.Clear();
        var user = new User { Id = 1, FullName = "Test User", Email = "Test@Example.com" };
        MainData.Users.Add(user);

        // Act
        var resultLowerCase = UsersOperator.GetUserByEmail("test@example.com");

        // Assert
        Assert.Null(resultLowerCase); // Sollte null sein wegen Groß-/Kleinschreibung
    }

    #endregion

    #region UsersOperator.GetUserByFullname Tests

    [Fact]
    public void GetUserByFullname_WithValidFullname_ReturnsCorrectUser()
    {
        // Arrange
        MainData.Users.Clear();
        var user = new User { Id = 1, FullName = "Max Mustermann", Email = "max@test.com" };
        MainData.Users.Add(user);

        // Act
        var result = UsersOperator.GetUserByFullname("Max Mustermann");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("max@test.com", result.Email);
    }

    [Fact]
    public void GetUserByFullname_WithInvalidFullname_ReturnsNull()
    {
        // Arrange
        MainData.Users.Clear();

        // Act
        var result = UsersOperator.GetUserByFullname("Non Existing User");

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void User_WithSpecialCharactersInName_HandlesCorrectly()
    {
        // Arrange & Act
        var user = new User("Müller-Schmidt, André", "andre@test.com", "pass", UserRole.User, new List<int>());

        // Assert
        Assert.Equal("Müller-Schmidt, André", user.FullName);
    }

    [Fact]
    public void User_WithEmptyPassword_AllowsCreation()
    {
        // Arrange & Act
        var user = new User("User", "user@test.com", "", UserRole.User, new List<int>());

        // Assert
        Assert.Equal("", user.Password);
    }

    [Fact]
    public void User_WithVeryLongName_HandlesCorrectly()
    {
        // Arrange
        var longName = new string('A', 1000);

        // Act
        var user = new User(longName, "user@test.com", "pass", UserRole.User, new List<int>());

        // Assert
        Assert.Equal(longName, user.FullName);
    }

    #endregion
}
