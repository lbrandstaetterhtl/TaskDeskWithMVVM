using TaskDesk_version2.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Task = TaskDesk_version2.Models.Task;

namespace TaskDesk_version2.Tests.Tests.Models;

/// <summary>
/// Unit-Tests für die Task-Klasse und TasksOperator
/// </summary>
public class TaskTests : IDisposable
{
    public TaskTests()
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

    #region Task Constructor Tests

    [Fact]
    public void TaskConstructor_WithValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 1;
        var title = "Test Task";
        var description = "Test Description";
        var dueDate = new DateOnly(2026, 12, 31);
        var state = TaskState.Pending;
        var groupIds = new List<int> { 1, 2 };
        var userIds = new List<int> { 1, 2, 3 };

        // Act
        var task = new Task(id, title, description, dueDate, state, groupIds, userIds);

        // Assert
        Assert.Equal(id, task.Id);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(dueDate, task.DueDate);
        Assert.Equal(state, task.State);
        Assert.Equal(groupIds, task.GroupIds);
        Assert.Equal(userIds, task.UserIds);
    }

    [Fact]
    public void TaskConstructor_WithPendingState_CreatesTaskWithPendingState()
    {
        // Act
        var task = new Task(1, "Task", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(TaskState.Pending, task.State);
    }

    [Fact]
    public void TaskConstructor_WithInProgressState_CreatesTaskWithInProgressState()
    {
        // Act
        var task = new Task(1, "Task", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.InProgress, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(TaskState.InProgress, task.State);
    }

    [Fact]
    public void TaskConstructor_WithCompletedState_CreatesTaskWithCompletedState()
    {
        // Act
        var task = new Task(1, "Task", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.Completed, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(TaskState.Completed, task.State);
    }

    [Fact]
    public void TaskConstructor_WithOnHoldState_CreatesTaskWithOnHoldState()
    {
        // Act
        var task = new Task(1, "Task", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.OnHold, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(TaskState.OnHold, task.State);
    }

    [Fact]
    public void TaskConstructor_WithCancelledState_CreatesTaskWithCancelledState()
    {
        // Act
        var task = new Task(1, "Task", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.Cancelled, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(TaskState.Cancelled, task.State);
    }

    [Fact]
    public void TaskDefaultConstructor_CreatesTaskWithEmptyLists()
    {
        // Act
        var task = new Task();

        // Assert
        Assert.NotNull(task.GroupIds);
        Assert.NotNull(task.UserIds);
        Assert.Empty(task.GroupIds);
        Assert.Empty(task.UserIds);
    }

    [Fact]
    public void TaskConstructor_WithEmptyLists_CreatesTaskSuccessfully()
    {
        // Act
        var task = new Task(1, "Task", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Empty(task.GroupIds);
        Assert.Empty(task.UserIds);
    }

    #endregion

    #region GetTaskStateAsString Tests

    [Fact]
    public void GetTaskStateAsString_PendingState_ReturnsPendingString()
    {
        // Arrange
        var task = new Task { State = TaskState.Pending };

        // Act
        var result = task.GetTaskStateAsString();

        // Assert
        Assert.Equal("Pending...", result);
    }

    [Fact]
    public void GetTaskStateAsString_InProgressState_ReturnsInProgressString()
    {
        // Arrange
        var task = new Task { State = TaskState.InProgress };

        // Act
        var result = task.GetTaskStateAsString();

        // Assert
        Assert.Equal("In Progress...", result);
    }

    [Fact]
    public void GetTaskStateAsString_CompletedState_ReturnsCompletedString()
    {
        // Arrange
        var task = new Task { State = TaskState.Completed };

        // Act
        var result = task.GetTaskStateAsString();

        // Assert
        Assert.Equal("Completed!", result);
    }

    [Fact]
    public void GetTaskStateAsString_OnHoldState_ReturnsOnHoldString()
    {
        // Arrange
        var task = new Task { State = TaskState.OnHold };

        // Act
        var result = task.GetTaskStateAsString();

        // Assert
        Assert.Equal("On Hold...", result);
    }

    [Fact]
    public void GetTaskStateAsString_CancelledState_ReturnsCancelledString()
    {
        // Arrange
        var task = new Task { State = TaskState.Cancelled };

        // Act
        var result = task.GetTaskStateAsString();

        // Assert
        Assert.Equal("Cancelled!", result);
    }

    [Theory]
    [InlineData(TaskState.Pending, "Pending...")]
    [InlineData(TaskState.InProgress, "In Progress...")]
    [InlineData(TaskState.Completed, "Completed!")]
    [InlineData(TaskState.OnHold, "On Hold...")]
    [InlineData(TaskState.Cancelled, "Cancelled!")]
    public void GetTaskStateAsString_AllStates_ReturnsCorrectStrings(TaskState state, string expected)
    {
        // Arrange
        var task = new Task { State = state };

        // Act
        var result = task.GetTaskStateAsString();

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region StateAsString Property Tests

    [Fact]
    public void StateAsString_Property_ReturnsCorrectString()
    {
        // Arrange
        var task = new Task { State = TaskState.InProgress };

        // Act
        var result = task.StateAsString;

        // Assert
        Assert.Equal("In Progress...", result);
    }

    #endregion

    #region DateAsString Property Tests

    [Fact]
    public void DateAsString_Property_ReturnsFormattedDate()
    {
        // Arrange
        var task = new Task { DueDate = new DateOnly(2026, 12, 31) };

        // Act
        var result = task.DateAsString;

        // Assert
        Assert.Equal("31.12.2026", result);
    }

    [Fact]
    public void DateAsString_WithSingleDigitDayAndMonth_PadsWithZeros()
    {
        // Arrange
        var task = new Task { DueDate = new DateOnly(2026, 1, 5) };

        // Act
        var result = task.DateAsString;

        // Assert
        Assert.Equal("05.01.2026", result);
    }

    #endregion

    #region TasksOperator.GetNextTaskId Tests

    [Fact]
    public void GetNextTaskId_WithEmptyTasks_Returns1()
    {
        // Arrange
        MainData.Tasks.Clear();

        // Act
        var result = TasksOperator.GetNextTaskId();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetNextTaskId_WithExistingTasks_ReturnsMaxIdPlusOne()
    {
        // Arrange
        MainData.Tasks.Clear();
        MainData.Tasks.Add(new Task { Id = 5, Title = "Task 1" });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Task 2" });

        // Act
        var result = TasksOperator.GetNextTaskId();

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void GetNextTaskId_WithGapsInIds_ReturnsMaxIdPlusOne()
    {
        // Arrange
        MainData.Tasks.Clear();
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1" });
        MainData.Tasks.Add(new Task { Id = 100, Title = "Task 2" });

        // Act
        var result = TasksOperator.GetNextTaskId();

        // Assert
        Assert.Equal(101, result);
    }

    [Fact]
    public void GetNextTaskId_WithSingleTask_ReturnsIdPlusOne()
    {
        // Arrange
        MainData.Tasks.Clear();
        MainData.Tasks.Add(new Task { Id = 42, Title = "Single Task" });

        // Act
        var result = TasksOperator.GetNextTaskId();

        // Assert
        Assert.Equal(43, result);
    }

    #endregion

    #region TasksOperator.GetListFromIds Tests

    [Fact]
    public void GetListFromIds_WithValidIds_ReturnsCorrectTasks()
    {
        // Arrange
        var allTasks = new ObservableCollection<Task>
        {
            new Task { Id = 1, Title = "Task 1" },
            new Task { Id = 2, Title = "Task 2" },
            new Task { Id = 3, Title = "Task 3" }
        };
        var ids = new List<int> { 1, 3 };

        // Act
        var result = TasksOperator.GetListFromIds(ids, allTasks);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Task 1", result[0].Title);
        Assert.Equal("Task 3", result[1].Title);
    }

    [Fact]
    public void GetListFromIds_WithNonExistingIds_ReturnsEmptyList()
    {
        // Arrange
        var allTasks = new ObservableCollection<Task>
        {
            new Task { Id = 1, Title = "Task 1" }
        };
        var ids = new List<int> { 99, 100 };

        // Act
        var result = TasksOperator.GetListFromIds(ids, allTasks);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetListFromIds_WithEmptyIdsList_ReturnsEmptyList()
    {
        // Arrange
        var allTasks = new ObservableCollection<Task>
        {
            new Task { Id = 1, Title = "Task 1" }
        };
        var ids = new List<int>();

        // Act
        var result = TasksOperator.GetListFromIds(ids, allTasks);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetListFromIds_WithEmptyAllTasks_ReturnsEmptyList()
    {
        // Arrange
        var allTasks = new ObservableCollection<Task>();
        var ids = new List<int> { 1, 2, 3 };

        // Act
        var result = TasksOperator.GetListFromIds(ids, allTasks);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetListFromIds_PreservesOrder()
    {
        // Arrange
        var allTasks = new ObservableCollection<Task>
        {
            new Task { Id = 1, Title = "Task 1" },
            new Task { Id = 2, Title = "Task 2" },
            new Task { Id = 3, Title = "Task 3" }
        };
        var ids = new List<int> { 3, 1, 2 };

        // Act
        var result = TasksOperator.GetListFromIds(ids, allTasks);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Task 3", result[0].Title);
        Assert.Equal("Task 1", result[1].Title);
        Assert.Equal("Task 2", result[2].Title);
    }

    #endregion

    #region TasksOperator.GetIdsFromList Tests

    [Fact]
    public void GetIdsFromList_WithMatchingTasks_ReturnsCorrectIds()
    {
        // Arrange
        var allTasks = new ObservableCollection<Task>
        {
            new Task { Id = 1, Title = "Task 1" },
            new Task { Id = 2, Title = "Task 2" }
        };
        var selectedTasks = new ObservableCollection<Task>
        {
            new Task { Id = 1, Title = "Task 1" },
            new Task { Id = 2, Title = "Task 2" }
        };

        // Act
        var result = TasksOperator.GetIdsFromList(selectedTasks, allTasks);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(2, result);
    }

    [Fact]
    public void GetIdsFromList_WithNoMatchingTasks_ReturnsEmptyList()
    {
        // Arrange
        var allTasks = new ObservableCollection<Task>
        {
            new Task { Id = 1, Title = "Task 1" }
        };
        var selectedTasks = new ObservableCollection<Task>
        {
            new Task { Id = 99, Title = "Non-matching Task" }
        };

        // Act
        var result = TasksOperator.GetIdsFromList(selectedTasks, allTasks);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetIdsFromList_WithEmptySelectedTasks_ReturnsEmptyList()
    {
        // Arrange
        var allTasks = new ObservableCollection<Task>
        {
            new Task { Id = 1, Title = "Task 1" }
        };
        var selectedTasks = new ObservableCollection<Task>();

        // Act
        var result = TasksOperator.GetIdsFromList(selectedTasks, allTasks);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region DueDate Tests

    [Fact]
    public void Task_DueDate_CanBeSetToFutureDate()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1));

        // Act
        var task = new Task(1, "Future Task", "Desc", futureDate, TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(futureDate, task.DueDate);
    }

    [Fact]
    public void Task_DueDate_CanBeSetToPastDate()
    {
        // Arrange
        var pastDate = new DateOnly(2020, 1, 1);

        // Act
        var task = new Task(1, "Past Task", "Desc", pastDate, TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(pastDate, task.DueDate);
    }

    [Fact]
    public void Task_DueDate_CanBeSetToToday()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var task = new Task(1, "Today Task", "Desc", today, TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(today, task.DueDate);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Task_WithSpecialCharactersInTitle_HandlesCorrectly()
    {
        // Arrange & Act
        var task = new Task(1, "Task: <Test> & \"Quotes\"", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Equal("Task: <Test> & \"Quotes\"", task.Title);
    }

    [Fact]
    public void Task_WithEmptyTitle_AllowsCreation()
    {
        // Arrange & Act
        var task = new Task(1, "", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Equal("", task.Title);
    }

    [Fact]
    public void Task_WithVeryLongDescription_HandlesCorrectly()
    {
        // Arrange
        var longDescription = new string('A', 10000);

        // Act
        var task = new Task(1, "Task", longDescription, DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(longDescription, task.Description);
    }

    [Fact]
    public void Task_WithNullDescription_AllowsCreation()
    {
        // Arrange & Act
        var task = new Task(1, "Task", null!, DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Null(task.Description);
    }

    [Fact]
    public void Task_WithLargeId_HandlesCorrectly()
    {
        // Arrange & Act
        var task = new Task(int.MaxValue, "Task", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>(), new List<int>());

        // Assert
        Assert.Equal(int.MaxValue, task.Id);
    }

    [Fact]
    public void Task_WithManyGroupIds_HandlesCorrectly()
    {
        // Arrange
        var manyGroupIds = new List<int>();
        for (int i = 1; i <= 1000; i++)
        {
            manyGroupIds.Add(i);
        }

        // Act
        var task = new Task(1, "Task", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, manyGroupIds, new List<int>());

        // Assert
        Assert.Equal(1000, task.GroupIds.Count);
    }

    [Fact]
    public void Task_WithManyUserIds_HandlesCorrectly()
    {
        // Arrange
        var manyUserIds = new List<int>();
        for (int i = 1; i <= 1000; i++)
        {
            manyUserIds.Add(i);
        }

        // Act
        var task = new Task(1, "Task", "Desc", DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>(), manyUserIds);

        // Assert
        Assert.Equal(1000, task.UserIds.Count);
    }

    #endregion

    #region State Transition Tests

    [Fact]
    public void Task_StateCanBeChangedFromPendingToInProgress()
    {
        // Arrange
        var task = new Task { State = TaskState.Pending };

        // Act
        task.State = TaskState.InProgress;

        // Assert
        Assert.Equal(TaskState.InProgress, task.State);
    }

    [Fact]
    public void Task_StateCanBeChangedFromInProgressToCompleted()
    {
        // Arrange
        var task = new Task { State = TaskState.InProgress };

        // Act
        task.State = TaskState.Completed;

        // Assert
        Assert.Equal(TaskState.Completed, task.State);
    }

    [Fact]
    public void Task_StateCanBeChangedToAnyState()
    {
        // Arrange
        var task = new Task { State = TaskState.Pending };

        // Act & Assert - Alle Zustandsübergänge sind erlaubt
        task.State = TaskState.InProgress;
        Assert.Equal(TaskState.InProgress, task.State);

        task.State = TaskState.OnHold;
        Assert.Equal(TaskState.OnHold, task.State);

        task.State = TaskState.Cancelled;
        Assert.Equal(TaskState.Cancelled, task.State);

        task.State = TaskState.Completed;
        Assert.Equal(TaskState.Completed, task.State);

        task.State = TaskState.Pending;
        Assert.Equal(TaskState.Pending, task.State);
    }

    #endregion
}
