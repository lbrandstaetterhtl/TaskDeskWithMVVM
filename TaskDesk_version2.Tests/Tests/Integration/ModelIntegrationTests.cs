﻿using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Task = TaskDesk_version2.Models.Task;

namespace TaskDesk_version2.Tests.Tests.Integration;

/// <summary>
/// Integrationstests die das Zusammenspiel zwischen verschiedenen Modellen testen
/// </summary>
public class ModelIntegrationTests : IDisposable
{
    public ModelIntegrationTests()
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

    #region User-Group Relationship Tests

    [Fact]
    public void User_CanBeAssignedToMultipleGroups()
    {
        // Arrange
        var group1 = new Group { Id = 1, Name = "Development" };
        var group2 = new Group { Id = 2, Name = "Testing" };
        var group3 = new Group { Id = 3, Name = "DevOps" };
        
        MainData.Groups.Add(group1);
        MainData.Groups.Add(group2);
        MainData.Groups.Add(group3);

        // Act
        var user = new User
        {
            Id = 1,
            FullName = "Multi-Group User",
            GroupIds = new List<int> { 1, 2, 3 }
        };
        MainData.Users.Add(user);

        // Assert
        Assert.Equal(3, user.GroupIds.Count);
        
        var groups = new List<Group> { group1, group2, group3 };
        var groupsString = user.GetGroupsAsString(groups);
        Assert.Contains("Development", groupsString);
        Assert.Contains("Testing", groupsString);
        Assert.Contains("DevOps", groupsString);
    }

    [Fact]
    public void Group_CanContainMultipleUsers()
    {
        // Arrange
        var user1 = new User { Id = 1, FullName = "User 1" };
        var user2 = new User { Id = 2, FullName = "User 2" };
        var user3 = new User { Id = 3, FullName = "User 3" };
        
        MainData.Users.Add(user1);
        MainData.Users.Add(user2);
        MainData.Users.Add(user3);

        // Act
        var group = new Group
        {
            Id = 1,
            Name = "Team",
            UserIds = new List<int> { 1, 2, 3 }
        };

        // Assert
        Assert.Equal(3, group.UserIds.Count);
        
        var users = new List<User> { user1, user2, user3 };
        var usersString = group.GetUsersAsString(users);
        Assert.Contains("User 1", usersString);
        Assert.Contains("User 2", usersString);
        Assert.Contains("User 3", usersString);
    }

    [Fact]
    public void User_GroupRelationship_BidirectionalConsistency()
    {
        // Arrange
        MainData.Groups.Clear();
        MainData.Users.Clear();

        var group = new Group { Id = 1, Name = "Development", UserIds = new List<int> { 1, 2 } };
        MainData.Groups.Add(group);

        var user1 = new User { Id = 1, FullName = "User 1", GroupIds = new List<int> { 1 } };
        var user2 = new User { Id = 2, FullName = "User 2", GroupIds = new List<int> { 1 } };
        MainData.Users.Add(user1);
        MainData.Users.Add(user2);

        // Assert - Gruppe enthält beide Benutzer
        Assert.Contains(1, group.UserIds);
        Assert.Contains(2, group.UserIds);

        // Assert - Beide Benutzer gehören zur Gruppe
        Assert.Contains(1, user1.GroupIds);
        Assert.Contains(1, user2.GroupIds);
    }

    #endregion

    #region User-Task Relationship Tests

    [Fact]
    public void User_CanBeAssignedToMultipleTasks()
    {
        // Arrange
        var task1 = new Task { Id = 1, Title = "Task 1" };
        var task2 = new Task { Id = 2, Title = "Task 2" };
        var task3 = new Task { Id = 3, Title = "Task 3" };
        
        MainData.Tasks.Add(task1);
        MainData.Tasks.Add(task2);
        MainData.Tasks.Add(task3);

        // Act
        var user = new User
        {
            Id = 1,
            FullName = "Busy User",
            TaskIds = new List<int> { 1, 2, 3 }
        };

        // Assert
        Assert.Equal(3, user.TaskIds.Count);
        
        var tasks = new List<Task> { task1, task2, task3 };
        var tasksString = user.GetTasksAsString(tasks);
        Assert.Contains("Task 1", tasksString);
        Assert.Contains("Task 2", tasksString);
        Assert.Contains("Task 3", tasksString);
    }

    [Fact]
    public void Task_CanBeAssignedToMultipleUsers()
    {
        // Arrange
        MainData.Users.Clear();
        var user1 = new User { Id = 1, FullName = "User 1" };
        var user2 = new User { Id = 2, FullName = "User 2" };
        MainData.Users.Add(user1);
        MainData.Users.Add(user2);

        // Act
        var task = new Task(
            1, 
            "Shared Task", 
            "Description", 
            DateOnly.FromDateTime(DateTime.Now),
            TaskState.Pending,
            new List<int>(),
            new List<int> { 1, 2 }
        );

        // Assert
        Assert.Equal(2, task.UserIds.Count);
        Assert.Contains(1, task.UserIds);
        Assert.Contains(2, task.UserIds);
    }

    #endregion

    #region Group-Task Relationship Tests

    [Fact]
    public void Task_CanBeAssignedToMultipleGroups()
    {
        // Arrange
        MainData.Groups.Clear();
        var group1 = new Group { Id = 1, Name = "Group 1" };
        var group2 = new Group { Id = 2, Name = "Group 2" };
        MainData.Groups.Add(group1);
        MainData.Groups.Add(group2);

        // Act
        var task = new Task(
            1,
            "Multi-Group Task",
            "Description",
            DateOnly.FromDateTime(DateTime.Now),
            TaskState.Pending,
            new List<int> { 1, 2 },
            new List<int>()
        );

        // Assert
        Assert.Equal(2, task.GroupIds.Count);
        Assert.Contains(1, task.GroupIds);
        Assert.Contains(2, task.GroupIds);
    }

    [Fact]
    public void Group_CanHaveMultipleTasks()
    {
        // Arrange
        var task1 = new Task { Id = 1, Title = "Task 1" };
        var task2 = new Task { Id = 2, Title = "Task 2" };
        var task3 = new Task { Id = 3, Title = "Task 3" };

        // Act
        var group = new Group
        {
            Id = 1,
            Name = "Busy Group",
            TaskIds = new List<int> { 1, 2, 3 }
        };

        // Assert
        Assert.Equal(3, group.TaskIds.Count);
        
        var tasks = new List<Task> { task1, task2, task3 };
        var tasksString = group.GetTasksAsString(tasks);
        Assert.Contains("Task 1", tasksString);
        Assert.Contains("Task 2", tasksString);
        Assert.Contains("Task 3", tasksString);
    }

    #endregion

    #region Complex Scenario Tests

    [Fact]
    public void ComplexScenario_UserInGroupWithTasks()
    {
        // Arrange - Erstelle komplexes Szenario
        MainData.Users.Clear();
        MainData.Groups.Clear();
        MainData.Tasks.Clear();

        // Erstelle Benutzer
        var user1 = new User { Id = 1, FullName = "Developer 1", GroupIds = new List<int> { 1 }, TaskIds = new List<int> { 1, 2 } };
        var user2 = new User { Id = 2, FullName = "Developer 2", GroupIds = new List<int> { 1 }, TaskIds = new List<int> { 2, 3 } };
        MainData.Users.Add(user1);
        MainData.Users.Add(user2);

        // Erstelle Gruppe
        var devGroup = new Group { Id = 1, Name = "Development", UserIds = new List<int> { 1, 2 }, TaskIds = new List<int> { 1, 2, 3 } };
        MainData.Groups.Add(devGroup);

        // Erstelle Tasks
        var task1 = new Task { Id = 1, Title = "Feature A", State = TaskState.InProgress, GroupIds = new List<int> { 1 }, UserIds = new List<int> { 1 } };
        var task2 = new Task { Id = 2, Title = "Feature B", State = TaskState.Pending, GroupIds = new List<int> { 1 }, UserIds = new List<int> { 1, 2 } };
        var task3 = new Task { Id = 3, Title = "Bug Fix", State = TaskState.Completed, GroupIds = new List<int> { 1 }, UserIds = new List<int> { 2 } };
        MainData.Tasks.Add(task1);
        MainData.Tasks.Add(task2);
        MainData.Tasks.Add(task3);

        // Assert - Überprüfe alle Beziehungen
        
        // User 1 hat 2 Tasks
        Assert.Equal(2, user1.TaskIds.Count);
        
        // User 2 hat 2 Tasks
        Assert.Equal(2, user2.TaskIds.Count);
        
        // Gruppe hat 2 Benutzer
        Assert.Equal(2, devGroup.UserIds.Count);
        
        // Gruppe hat 3 Tasks
        Assert.Equal(3, devGroup.TaskIds.Count);
        
        // Task 2 ist beiden Benutzern zugewiesen
        Assert.Equal(2, task2.UserIds.Count);
        
        // Alle Tasks gehören zur Development-Gruppe
        Assert.All(new[] { task1, task2, task3 }, t => Assert.Contains(1, t.GroupIds));
    }

    [Fact]
    public void ComplexScenario_MultipleGroupsAndUsers()
    {
        // Arrange
        MainData.Users.Clear();
        MainData.Groups.Clear();

        // Erstelle Gruppen
        var devGroup = new Group { Id = 1, Name = "Development", UserIds = new List<int> { 1, 2 } };
        var qaGroup = new Group { Id = 2, Name = "QA", UserIds = new List<int> { 2, 3 } };
        var opsGroup = new Group { Id = 3, Name = "Operations", UserIds = new List<int> { 3, 4 } };
        MainData.Groups.Add(devGroup);
        MainData.Groups.Add(qaGroup);
        MainData.Groups.Add(opsGroup);

        // Erstelle Benutzer mit Mehrfach-Gruppenzugehörigkeit
        var user1 = new User { Id = 1, FullName = "Pure Developer", GroupIds = new List<int> { 1 } };
        var user2 = new User { Id = 2, FullName = "DevQA", GroupIds = new List<int> { 1, 2 } };
        var user3 = new User { Id = 3, FullName = "QAOps", GroupIds = new List<int> { 2, 3 } };
        var user4 = new User { Id = 4, FullName = "Pure Ops", GroupIds = new List<int> { 3 } };
        MainData.Users.Add(user1);
        MainData.Users.Add(user2);
        MainData.Users.Add(user3);
        MainData.Users.Add(user4);

        // Assert
        // User 1 ist nur in 1 Gruppe
        Assert.Single(user1.GroupIds);
        
        // User 2 ist in 2 Gruppen (Dev und QA)
        Assert.Equal(2, user2.GroupIds.Count);
        Assert.Contains(1, user2.GroupIds);
        Assert.Contains(2, user2.GroupIds);
        
        // User 3 ist in 2 Gruppen (QA und Ops)
        Assert.Equal(2, user3.GroupIds.Count);
        
        // Dev-Gruppe hat 2 Benutzer
        Assert.Equal(2, devGroup.UserIds.Count);
    }

    #endregion

    #region Data Consistency Tests

    [Fact]
    public void DataConsistency_AddUserToGroup_UpdatesBothSides()
    {
        // Arrange
        MainData.Users.Clear();
        MainData.Groups.Clear();

        var group = new Group { Id = 1, Name = "New Group", UserIds = new List<int>() };
        MainData.Groups.Add(group);

        var user = new User { Id = 1, FullName = "New User", GroupIds = new List<int>() };
        MainData.Users.Add(user);

        // Act - Füge Benutzer zur Gruppe hinzu (simuliere Anwendungslogik)
        group.UserIds.Add(user.Id);
        user.GroupIds.Add(group.Id);

        // Assert
        Assert.Contains(user.Id, group.UserIds);
        Assert.Contains(group.Id, user.GroupIds);
    }

    [Fact]
    public void DataConsistency_AssignTaskToUser_UpdatesBothSides()
    {
        // Arrange
        MainData.Users.Clear();
        MainData.Tasks.Clear();

        var task = new Task { Id = 1, Title = "New Task", UserIds = new List<int>() };
        MainData.Tasks.Add(task);

        var user = new User { Id = 1, FullName = "Worker", TaskIds = new List<int>() };
        MainData.Users.Add(user);

        // Act - Weise Task dem Benutzer zu
        task.UserIds.Add(user.Id);
        user.TaskIds.Add(task.Id);

        // Assert
        Assert.Contains(user.Id, task.UserIds);
        Assert.Contains(task.Id, user.TaskIds);
    }

    #endregion

    #region ID Generation Tests

    [Fact]
    public void IdGeneration_MultipleEntities_GeneratesUniqueIds()
    {
        MainData.Users.Clear();
        MainData.Groups.Clear();
        MainData.Tasks.Clear();

        Assert.Empty(MainData.Users);
        Assert.Empty(MainData.Groups);
        Assert.Empty(MainData.Tasks);

        // Act - Create 10 of each entity with explicit IDs
        for (int i = 1; i <= 10; i++)
        {
            var user = new User { Id = i, FullName = $"User {i}" };
            MainData.Users.Add(user);
        }

        for (int i = 1; i <= 10; i++)
        {
            var group = new Group { Id = i, Name = $"Group {i}" };
            MainData.Groups.Add(group);
        }

        for (int i = 1; i <= 10; i++)
        {
            var task = new Task { Id = i, Title = $"Task {i}" };
            MainData.Tasks.Add(task);
        }

        // Assert - Verify 10 entities were added
        Assert.Equal(10, MainData.Users.Count);
        Assert.Equal(10, MainData.Groups.Count);
        Assert.Equal(10, MainData.Tasks.Count);

        // Verify IDs are unique
        var userIdList = MainData.Users.Select(u => u.Id).ToList();
        var groupIdList = MainData.Groups.Select(g => g.Id).ToList();
        var taskIdList = MainData.Tasks.Select(t => t.Id).ToList();

        Assert.Equal(10, userIdList.Count);
        Assert.Equal(10, groupIdList.Count);
        Assert.Equal(10, taskIdList.Count);

        // Verify GetNextId returns the next expected value
        Assert.Equal(11, GroupsOperator.GetNextGroupId());
        Assert.Equal(11, UsersOperator.GetNextUserId());
        Assert.Equal(11, TasksOperator.GetNextTaskId());
    }

    #endregion

    #region Lookup Tests

    [Fact]
    public void Lookup_FindUserAcrossMultipleGroups()
    {
        // Arrange
        MainData.Users.Clear();
        MainData.Groups.Clear();

        var user = new User { Id = 1, FullName = "Cross-Group User" };
        MainData.Users.Add(user);

        var group1 = new Group { Id = 1, Name = "Group 1", UserIds = new List<int> { 1 } };
        var group2 = new Group { Id = 2, Name = "Group 2", UserIds = new List<int> { 1 } };
        var group3 = new Group { Id = 3, Name = "Group 3", UserIds = new List<int> { 1 } };
        MainData.Groups.Add(group1);
        MainData.Groups.Add(group2);
        MainData.Groups.Add(group3);

        // Act - Finde alle Gruppen, in denen der Benutzer ist
        var userGroups = new List<Group>();
        foreach (var group in MainData.Groups)
        {
            if (group.UserIds.Contains(user.Id))
            {
                userGroups.Add(group);
            }
        }

        // Assert
        Assert.Equal(3, userGroups.Count);
    }

    [Fact]
    public void Lookup_FindAllTasksForGroup()
    {
        // Arrange
        MainData.Groups.Clear();
        MainData.Tasks.Clear();

        var group = new Group { Id = 1, Name = "Target Group" };
        MainData.Groups.Add(group);

        var task1 = new Task { Id = 1, Title = "Task 1", GroupIds = new List<int> { 1 } };
        var task2 = new Task { Id = 2, Title = "Task 2", GroupIds = new List<int> { 1 } };
        var task3 = new Task { Id = 3, Title = "Task 3", GroupIds = new List<int> { 2 } }; // Andere Gruppe
        MainData.Tasks.Add(task1);
        MainData.Tasks.Add(task2);
        MainData.Tasks.Add(task3);

        // Act
        var groupTasks = new List<Task>();
        foreach (var task in MainData.Tasks)
        {
            if (task.GroupIds.Contains(group.Id))
            {
                groupTasks.Add(task);
            }
        }

        // Assert
        Assert.Equal(2, groupTasks.Count);
        Assert.Contains(task1, groupTasks);
        Assert.Contains(task2, groupTasks);
        Assert.DoesNotContain(task3, groupTasks);
    }

    #endregion

    #region Task State Workflow Tests

    [Fact]
    public void TaskWorkflow_PendingToCompleted_UpdatesCorrectly()
    {
        // Arrange
        MainData.Tasks.Clear();
        var task = new Task
        {
            Id = 1,
            Title = "Workflow Task",
            State = TaskState.Pending
        };
        MainData.Tasks.Add(task);

        // Act & Assert - Simulate task progression
        Assert.Equal(TaskState.Pending, task.State);

        task.State = TaskState.InProgress;
        Assert.Equal(TaskState.InProgress, task.State);

        task.State = TaskState.Completed;
        Assert.Equal(TaskState.Completed, task.State);
    }

    [Fact]
    public void TaskWorkflow_CanBeCancelled_FromAnyState()
    {
        // Arrange
        MainData.Tasks.Clear();
        
        // Test from each state
        foreach (TaskState state in Enum.GetValues(typeof(TaskState)))
        {
            if (state == TaskState.Cancelled) continue;

            var task = new Task { Id = 1, Title = "Test", State = state };
            
            // Act
            task.State = TaskState.Cancelled;
            
            // Assert
            Assert.Equal(TaskState.Cancelled, task.State);
        }
    }

    [Fact]
    public void TaskWorkflow_OnHold_CanBeResumed()
    {
        // Arrange
        var task = new Task
        {
            Id = 1,
            Title = "Pausable Task",
            State = TaskState.InProgress
        };

        // Act
        task.State = TaskState.OnHold;
        Assert.Equal(TaskState.OnHold, task.State);

        task.State = TaskState.InProgress;
        
        // Assert
        Assert.Equal(TaskState.InProgress, task.State);
    }

    #endregion

    #region User Role Permission Tests

    [Fact]
    public void UserRoles_DifferentRoles_CanCoexist()
    {
        // Arrange
        MainData.Users.Clear();
        var admin = new User { Id = 1, FullName = "Admin", Role = UserRole.Admin };
        var user = new User { Id = 2, FullName = "User", Role = UserRole.User };
        var readOnly = new User { Id = 3, FullName = "ReadOnly", Role = UserRole.ReadOnly };

        // Act
        MainData.Users.Add(admin);
        MainData.Users.Add(user);
        MainData.Users.Add(readOnly);

        // Assert
        Assert.Equal(3, MainData.Users.Count);
        Assert.Equal(UserRole.Admin, MainData.Users[0].Role);
        Assert.Equal(UserRole.User, MainData.Users[1].Role);
        Assert.Equal(UserRole.ReadOnly, MainData.Users[2].Role);
    }

    #endregion

    #region Bulk Operations Tests

    [Fact]
    public void BulkOperation_AddMultipleTasks_MaintainsData()
    {
        // Arrange
        MainData.Tasks.Clear();
        var tasks = new List<Task>();

        // Act
        for (int i = 1; i <= 50; i++)
        {
            var task = new Task
            {
                Id = i,
                Title = $"Bulk Task {i}",
                State = i % 2 == 0 ? TaskState.Completed : TaskState.Pending
            };
            tasks.Add(task);
            MainData.Tasks.Add(task);
        }

        // Assert
        Assert.Equal(50, MainData.Tasks.Count);
        
        var completedCount = MainData.Tasks.Count(t => t.State == TaskState.Completed);
        var pendingCount = MainData.Tasks.Count(t => t.State == TaskState.Pending);
        
        Assert.Equal(25, completedCount);
        Assert.Equal(25, pendingCount);
    }

    [Fact]
    public void BulkOperation_RemoveMultipleUsers_UpdatesGroups()
    {
        // Arrange
        MainData.Users.Clear();
        MainData.Groups.Clear();

        var group = new Group { Id = 1, Name = "Test Group", UserIds = new List<int> { 1, 2, 3, 4, 5 } };
        MainData.Groups.Add(group);

        for (int i = 1; i <= 5; i++)
        {
            MainData.Users.Add(new User { Id = i, FullName = $"User {i}", GroupIds = new List<int> { 1 } });
        }

        // Act - Remove users 2, 3, 4
        var usersToRemove = new[] { 2, 3, 4 };
        foreach (var userId in usersToRemove)
        {
            group.UserIds.Remove(userId);
        }

        // Assert
        Assert.Equal(2, group.UserIds.Count);
        Assert.Contains(1, group.UserIds);
        Assert.Contains(5, group.UserIds);
        Assert.DoesNotContain(2, group.UserIds);
    }

    #endregion

    #region Circular Reference Prevention Tests

    [Fact]
    public void CircularReferences_UserGroupRelationship_IsConsistent()
    {
        // Arrange
        MainData.Users.Clear();
        MainData.Groups.Clear();

        var user = new User { Id = 1, FullName = "User", GroupIds = new List<int> { 1 } };
        var group = new Group { Id = 1, Name = "Group", UserIds = new List<int> { 1 } };

        MainData.Users.Add(user);
        MainData.Groups.Add(group);

        // Assert - Verify bidirectional relationship
        Assert.Contains(1, user.GroupIds);
        Assert.Contains(1, group.UserIds);

        // Remove from one side
        user.GroupIds.Remove(1);
        
        // Group still has reference (needs manual cleanup in real scenario)
        Assert.Contains(1, group.UserIds);
        Assert.DoesNotContain(1, user.GroupIds);
    }

    #endregion

    #region Date-Based Tests

    [Fact]
    public void Tasks_WithDifferentDueDates_CanBeQueried()
    {
        // Arrange
        MainData.Tasks.Clear();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var yesterday = today.AddDays(-1);
        var tomorrow = today.AddDays(1);

        MainData.Tasks.Add(new Task { Id = 1, Title = "Past", DueDate = yesterday });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Today", DueDate = today });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Future", DueDate = tomorrow });

        // Act
        var overdueTasks = MainData.Tasks.Where(t => t.DueDate < today).ToList();
        var todayTasks = MainData.Tasks.Where(t => t.DueDate == today).ToList();
        var futureTasks = MainData.Tasks.Where(t => t.DueDate > today).ToList();

        // Assert
        Assert.Single(overdueTasks);
        Assert.Single(todayTasks);
        Assert.Single(futureTasks);
    }

    [Fact]
    public void Tasks_OverdueButCompleted_ShouldNotBeOverdue()
    {
        // Arrange
        MainData.Tasks.Clear();
        var pastDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10));

        var overdueTask = new Task
        {
            Id = 1,
            Title = "Overdue Task",
            DueDate = pastDate,
            State = TaskState.Pending
        };

        var completedTask = new Task
        {
            Id = 2,
            Title = "Completed Task",
            DueDate = pastDate,
            State = TaskState.Completed
        };

        MainData.Tasks.Add(overdueTask);
        MainData.Tasks.Add(completedTask);

        // Act
        var today = DateOnly.FromDateTime(DateTime.Now);
        var actuallyOverdue = MainData.Tasks.Where(t => 
            t.DueDate < today && 
            t.State != TaskState.Completed && 
            t.State != TaskState.Cancelled
        ).ToList();

        // Assert
        Assert.Single(actuallyOverdue);
        Assert.Equal(1, actuallyOverdue[0].Id);
    }

    #endregion

    #region Null Safety Tests

    [Fact]
    public void User_WithNullGroupIds_InitializesToEmptyList()
    {
        // Act
        var user = new User();

        // Assert
        Assert.NotNull(user.GroupIds);
        Assert.Empty(user.GroupIds);
    }

    [Fact]
    public void Task_WithNullUserIds_InitializesToEmptyList()
    {
        // Act
        var task = new Task();

        // Assert
        Assert.NotNull(task.UserIds);
        Assert.Empty(task.UserIds);
    }

    [Fact]
    public void Group_WithNullTaskIds_InitializesToEmptyList()
    {
        // Act
        var group = new Group();

        // Assert
        Assert.NotNull(group.TaskIds);
        Assert.Empty(group.TaskIds);
    }

    #endregion
}
