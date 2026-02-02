using System;
using System.Collections.ObjectModel;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;
using Xunit;
using Task = TaskDesk_version2.Models.Task;

namespace TaskDesk_version2.Tests.Tests.ViewModels;

/// <summary>
/// Unit-Tests fuer ManageUsersWindowViewModel
/// </summary>
public class ManageUsersWindowViewModelTests : IDisposable
{
    public ManageUsersWindowViewModelTests()
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
    public void Constructor_InitializesFromUser()
    {
        // Arrange
        var group = new Group { Id = 1, Name = "Group A" };
        var task = new Task { Id = 1, Title = "Task A" };
        var user = new User
        {
            Id = 7,
            FullName = "User One",
            Email = "user1@test.com",
            Password = "pass",
            Role = UserRole.Admin,
            GroupIds = new() { 1 },
            TaskIds = new() { 1 }
        };
        MainData.Groups.Add(group);
        MainData.Tasks.Add(task);
        MainData.Users.Add(user);

        // Act
        var viewModel = new ManageUsersWindowViewModel(user);

        // Assert
        Assert.Equal("User One", viewModel.Fullname);
        Assert.Equal("user1@test.com", viewModel.Email);
        Assert.Equal("pass", viewModel.Password);
        Assert.Equal("Admin", viewModel.RoleString);
        Assert.Single(viewModel.AssignedGroups);
        Assert.Single(viewModel.AssignedTasks);
        Assert.Same(user, viewModel.SelectedUser);
    }

    [Fact]
    public void SelectedUser_UpdatesViewModelData()
    {
        // Arrange
        var group1 = new Group { Id = 1, Name = "Group A" };
        var group2 = new Group { Id = 2, Name = "Group B" };
        var task1 = new Task { Id = 1, Title = "Task A" };
        var task2 = new Task { Id = 2, Title = "Task B" };
        var user1 = new User
        {
            Id = 1,
            FullName = "User One",
            Email = "user1@test.com",
            Password = "pass1",
            Role = UserRole.User,
            GroupIds = new() { 1 },
            TaskIds = new() { 1 }
        };
        var user2 = new User
        {
            Id = 2,
            FullName = "User Two",
            Email = "user2@test.com",
            Password = "pass2",
            Role = UserRole.ReadOnly,
            GroupIds = new() { 2 },
            TaskIds = new() { 2 }
        };
        MainData.Groups.Add(group1);
        MainData.Groups.Add(group2);
        MainData.Tasks.Add(task1);
        MainData.Tasks.Add(task2);
        MainData.Users.Add(user1);
        MainData.Users.Add(user2);

        var viewModel = new ManageUsersWindowViewModel(user1);

        // Act
        viewModel.SelectedUser = user2;

        // Assert
        Assert.Equal("User Two", viewModel.Fullname);
        Assert.Equal("user2@test.com", viewModel.Email);
        Assert.Equal("pass2", viewModel.Password);
        Assert.Equal("Read-Only", viewModel.RoleString);
        Assert.Single(viewModel.AssignedGroups);
        Assert.Single(viewModel.AssignedTasks);
        Assert.Equal(2, viewModel.AssignedGroups[0].Id);
        Assert.Equal(2, viewModel.AssignedTasks[0].Id);
    }

    [Fact]
    public void SaveCommand_UpdatesUserAndRelations()
    {
        // Arrange
        var group1 = new Group { Id = 1, Name = "Group A", UserIds = new() { 1 } };
        var group2 = new Group { Id = 2, Name = "Group B", UserIds = new() };
        var task1 = new Task { Id = 1, Title = "Task A", UserIds = new() { 1 } };
        var task2 = new Task { Id = 2, Title = "Task B", UserIds = new() };
        var user = new User
        {
            Id = 1,
            FullName = "User One",
            Email = "user1@test.com",
            Password = "pass1",
            Role = UserRole.User,
            GroupIds = new() { 1 },
            TaskIds = new() { 1 }
        };
        MainData.Groups.Add(group1);
        MainData.Groups.Add(group2);
        MainData.Tasks.Add(task1);
        MainData.Tasks.Add(task2);
        MainData.Users.Add(user);

        var viewModel = new ManageUsersWindowViewModel(user);
        viewModel.Fullname = "User One Updated";
        viewModel.Email = "updated@test.com";
        viewModel.Password = "updated";
        viewModel.RoleString = "Admin";
        viewModel.AssignedGroups = new ObservableCollection<Group> { group2 };
        viewModel.AssignedTasks = new ObservableCollection<Task> { task2 };

        var closeInvoked = false;
        viewModel.RequestClose += () => closeInvoked = true;

        // Act
        viewModel.saveCommand.Execute(null);

        // Assert
        var updatedUser = MainData.Users[0];
        Assert.Equal("User One Updated", updatedUser.FullName);
        Assert.Equal("updated@test.com", updatedUser.Email);
        Assert.Equal("updated", updatedUser.Password);
        Assert.Equal(UserRole.Admin, updatedUser.Role);
        Assert.Single(updatedUser.GroupIds);
        Assert.Single(updatedUser.TaskIds);
        Assert.Contains(2, updatedUser.GroupIds);
        Assert.Contains(2, updatedUser.TaskIds);

        Assert.DoesNotContain(1, group1.UserIds);
        Assert.Contains(1, group2.UserIds);
        Assert.DoesNotContain(1, task1.UserIds);
        Assert.Contains(1, task2.UserIds);
        Assert.True(closeInvoked);
    }

    [Fact]
    public void SearchUpdate_EmptySearch_UsesAllUsers()
    {
        // Arrange
        var user = new User { Id = 1, FullName = "User One", Email = "user1@test.com" };
        MainData.Users.Add(user);
        var viewModel = new ManageUsersWindowViewModel(user);
        viewModel.SearchInput = "";

        // Act
        viewModel.SearchUpdate();

        // Assert
        Assert.Same(MainData.Users, viewModel.AllUsers);
    }

    [Fact]
    public void SearchUpdate_FiltersUsers_ByNameEmailOrId()
    {
        // Arrange
        var user1 = new User { Id = 1, FullName = "User One", Email = "user1@test.com" };
        var user2 = new User { Id = 2, FullName = "User Two", Email = "user2@test.com" };
        MainData.Users.Add(user1);
        MainData.Users.Add(user2);
        var viewModel = new ManageUsersWindowViewModel(user1);
        viewModel.SearchInput = "Two";

        // Act
        viewModel.SearchUpdate();

        // Assert
        Assert.Single(viewModel.AllUsers);
        Assert.Equal(2, viewModel.AllUsers[0].Id);
    }

    [Fact]
    public void SearchUpdate_NoMatches_LeavesAllUsersUnchanged()
    {
        // Arrange
        var user1 = new User { Id = 1, FullName = "User One", Email = "user1@test.com" };
        var user2 = new User { Id = 2, FullName = "User Two", Email = "user2@test.com" };
        MainData.Users.Add(user1);
        MainData.Users.Add(user2);
        var viewModel = new ManageUsersWindowViewModel(user1);
        var originalUsers = viewModel.AllUsers;
        viewModel.SearchInput = "NoMatch";

        // Act
        viewModel.SearchUpdate();

        // Assert
        Assert.Same(originalUsers, viewModel.AllUsers);
    }

    [Fact]
    public void ClearData_ResetsFieldsAndSelections()
    {
        // Arrange
        var user = new User { Id = 1, FullName = "User One", Email = "user1@test.com" };
        MainData.Users.Add(user);
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task A" });
        MainData.Groups.Add(new Group { Id = 1, Name = "Group A" });
        var viewModel = new ManageUsersWindowViewModel(user)
        {
            Fullname = "User One",
            Email = "user1@test.com",
            Password = "pass",
            RoleString = "User",
            AssignedGroups = new ObservableCollection<Group>(MainData.Groups),
            AssignedTasks = new ObservableCollection<Task>(MainData.Tasks)
        };

        // Act
        viewModel.ClearData();

        // Assert
        Assert.Equal(string.Empty, viewModel.Fullname);
        Assert.Equal(string.Empty, viewModel.Email);
        Assert.Equal(string.Empty, viewModel.Password);
        Assert.Equal(string.Empty, viewModel.RoleString);
        Assert.Empty(viewModel.AssignedGroups);
        Assert.Empty(viewModel.AssignedTasks);
    }

    [Fact]
    public void ClearSearch_ResetsSearchInput()
    {
        // Arrange
        var user = new User { Id = 1, FullName = "User One", Email = "user1@test.com" };
        MainData.Users.Add(user);
        var viewModel = new ManageUsersWindowViewModel(user) { SearchInput = "User" };

        // Act
        viewModel.ClearSearch();

        // Assert
        Assert.Equal(string.Empty, viewModel.SearchInput);
    }
}
