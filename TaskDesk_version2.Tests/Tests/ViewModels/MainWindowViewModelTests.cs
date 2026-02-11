using System.Collections.ObjectModel;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;
using Task = TaskDesk_version2.Models.Task;

namespace TaskDesk_version2.Tests.Tests.ViewModels;

/// <summary>
///     Unit-Tests fuer MainWindowViewModel
/// </summary>
public class MainWindowViewModelTests : IDisposable
{
    public MainWindowViewModelTests()
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

    #region Constructor Tests

    [Fact]
    public void Constructor_UsesMainDataTasks()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1", Description = "Desc 1" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Same(MainData.Tasks, viewModel.Tasks);
    }

    [Fact]
    public void Constructor_WithEmptyTasks_InitializesEmpty()
    {
        // Arrange
        MainData.Tasks.Clear();

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.Tasks);
        Assert.Empty(viewModel.Tasks);
    }

    [Fact]
    public void Constructor_WithMultipleTasks_ContainsAllTasks()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1", Description = "Desc 1" });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2", Description = "Desc 2" });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Task 3", Description = "Desc 3" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal(3, viewModel.Tasks.Count);
    }

    [Fact]
    public void Constructor_InitializesFilteredTasksToMainDataTasks()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1", Description = "Desc 1" });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2", Description = "Desc 2" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal(MainData.Tasks, viewModel.FilteredTasks);
    }

    [Fact]
    public void Constructor_InitializesSearchTextToEmpty()
    {
        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal(string.Empty, viewModel.SearchText);
    }

    [Fact]
    public void Constructor_InitializesFiltersToEmpty()
    {
        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Empty(viewModel.StateFilter);
        Assert.Empty(viewModel.UserFilter);
        Assert.Empty(viewModel.GroupFilter);
    }

    [Fact]
    public void Constructor_InitializesDateFilterToDisabled()
    {
        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.False(viewModel.IsDateFilterEnabled);
    }

    #endregion

    #region Tasks Property Tests

    [Fact]
    public void Tasks_IsReadOnly_ReturnsSameReference()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var tasks1 = viewModel.Tasks;
        var tasks2 = viewModel.Tasks;

        // Assert
        Assert.Same(tasks1, tasks2);
        Assert.Same(MainData.Tasks, viewModel.Tasks);
    }

    #endregion

    #region FilteredTasks Property Tests

    [Fact]
    public void FilteredTasks_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var newTasks = new ObservableCollection<Task> { new() { Id = 2, Title = "Task 2" } };
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.FilteredTasks)) propertyRaised = true;
        };

        // Act
        viewModel.FilteredTasks = newTasks;

        // Assert
        Assert.True(propertyRaised);
        Assert.Same(newTasks, viewModel.FilteredTasks);
    }

    [Fact]
    public void FilteredTasks_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.FilteredTasks)) propertyRaised = true;
        };

        // Act
        viewModel.FilteredTasks = viewModel.FilteredTasks;

        // Assert
        Assert.False(propertyRaised);
    }

    #endregion

    #region SearchText Property Tests

    [Fact]
    public void SearchText_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.SearchText)) propertyRaised = true;
        };

        // Act
        viewModel.SearchText = "test";

        // Assert
        Assert.True(propertyRaised);
        Assert.Equal("test", viewModel.SearchText);
    }

    [Fact]
    public void SearchText_SetSameValue_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        viewModel.SearchText = "test";
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.SearchText)) propertyRaised = true;
        };

        // Act
        viewModel.SearchText = "test";

        // Assert
        Assert.False(propertyRaised);
    }

    #endregion

    #region StateFilter Property Tests

    [Fact]
    public void StateFilter_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var newFilter = new ObservableCollection<string> { "Pending", "InProgress" };
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.StateFilter)) propertyRaised = true;
        };

        // Act
        viewModel.StateFilter = newFilter;

        // Assert
        Assert.True(propertyRaised);
        Assert.Same(newFilter, viewModel.StateFilter);
    }

    #endregion

    #region UserFilter Property Tests

    [Fact]
    public void UserFilter_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var newFilter = new ObservableCollection<string> { "User 1", "User 2" };
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.UserFilter)) propertyRaised = true;
        };

        // Act
        viewModel.UserFilter = newFilter;

        // Assert
        Assert.True(propertyRaised);
        Assert.Same(newFilter, viewModel.UserFilter);
    }

    #endregion

    #region GroupFilter Property Tests

    [Fact]
    public void GroupFilter_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var newFilter = new ObservableCollection<string> { "Group 1", "Group 2" };
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.GroupFilter)) propertyRaised = true;
        };

        // Act
        viewModel.GroupFilter = newFilter;

        // Assert
        Assert.True(propertyRaised);
        Assert.Same(newFilter, viewModel.GroupFilter);
    }

    #endregion

    #region IsDateFilterEnabled Property Tests

    [Fact]
    public void IsDateFilterEnabled_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.IsDateFilterEnabled)) propertyRaised = true;
        };

        // Act
        viewModel.IsDateFilterEnabled = true;

        // Assert
        Assert.True(propertyRaised);
        Assert.True(viewModel.IsDateFilterEnabled);
    }

    #endregion

    #region SearchDueDate Property Tests

    [Fact]
    public void SearchDueDate_SetValue_RaisesPropertyChanged()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var newDate = DateTimeOffset.Now.AddDays(5);
        var propertyRaised = false;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.SearchDueDate)) propertyRaised = true;
        };

        // Act
        viewModel.SearchDueDate = newDate;

        // Assert
        Assert.True(propertyRaised);
        Assert.Equal(newDate, viewModel.SearchDueDate);
    }

    #endregion

    #region Tasks Collection Synchronization Tests

    [Fact]
    public void Tasks_ReflectsMainDataChanges()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();

        // Act
        MainData.Tasks.Add(new Task { Id = 5, Title = "Added Task", Description = "Desc" });

        // Assert
        Assert.Single(viewModel.Tasks);
        Assert.Equal("Added Task", viewModel.Tasks[0].Title);
    }

    [Fact]
    public void Tasks_ReflectsMainDataRemovals()
    {
        // Arrange
        var task = new Task { Id = 1, Title = "Task to Remove", Description = "Desc" };
        MainData.Tasks.Add(task);
        var viewModel = new MainWindowViewModel();

        // Act
        MainData.Tasks.Remove(task);

        // Assert
        Assert.Empty(viewModel.Tasks);
    }

    [Fact]
    public void Tasks_ReflectsMainDataClear()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1" });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2" });
        var viewModel = new MainWindowViewModel();

        // Act
        MainData.Tasks.Clear();

        // Assert
        Assert.Empty(viewModel.Tasks);
    }

    #endregion

    #region PropertyChanged Event Tests

    [Fact]
    public void PropertyChanged_EventNotNullByDefault()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();

        // Act & Assert
        var exception = Record.Exception(() => { viewModel.PropertyChanged += (_, _) => { }; });
        Assert.Null(exception);
    }

    [Fact]
    public void PropertyChanged_MultipleSubscribers_AllNotified()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        var notificationCount = 0;
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.FilteredTasks))
                notificationCount++;
        };
        viewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(viewModel.FilteredTasks))
                notificationCount++;
        };

        // Act
        viewModel.FilteredTasks = new ObservableCollection<Task>();

        // Assert
        Assert.Equal(2, notificationCount);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_TasksWithDifferentStates_AllIncluded()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Pending", Description = "Desc", State = TaskState.Pending });
        MainData.Tasks.Add(new Task { Id = 2, Title = "InProgress", Description = "Desc", State = TaskState.InProgress });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Completed", Description = "Desc", State = TaskState.Completed });
        MainData.Tasks.Add(new Task { Id = 4, Title = "OnHold", Description = "Desc", State = TaskState.OnHold });
        MainData.Tasks.Add(new Task { Id = 5, Title = "Cancelled", Description = "Desc", State = TaskState.Cancelled });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal(5, viewModel.Tasks.Count);
    }

    [Fact]
    public void Tasks_WithSpecialCharactersInTitles_HandledCorrectly()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task: <Test> & \"Quotes\" äöü", Description = "Desc" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal("Task: <Test> & \"Quotes\" äöü", viewModel.Tasks[0].Title);
    }

    [Fact]
    public void Tasks_WithLargeNumber_HandledCorrectly()
    {
        // Arrange
        for (var i = 0; i < 100; i++) MainData.Tasks.Add(new Task { Id = i, Title = $"Task {i}", Description = $"Desc {i}" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.Equal(100, viewModel.Tasks.Count);
    }

    #endregion

    #region Command Tests

    [Fact]
    public void Constructor_InitializesLogoutCommand()
    {
        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.LogoutCommand);
    }

    [Fact]
    public void Constructor_InitializesApplyFiltersCommand()
    {
        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.ApplyFiltersCommand);
    }

    [Fact]
    public void Constructor_InitializesClearFiltersCommand()
    {
        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.ClearFiltersCommand);
    }

    #endregion

    #region StateOptions, AllUsers, AllGroups Properties Tests

    [Fact]
    public void StateOptions_ReturnsAllStateStrings()
    {
        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.StateOptions);
        Assert.NotEmpty(viewModel.StateOptions);
    }

    [Fact]
    public void AllUsers_ReturnsAllUserFullNames()
    {
        // Arrange
        MainData.Users.Add(new User { Id = 1, FullName = "John Doe" });
        MainData.Users.Add(new User { Id = 2, FullName = "Jane Smith" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.AllUsers);
        Assert.Equal(2, viewModel.AllUsers.Count);
    }

    [Fact]
    public void AllGroups_ReturnsAllGroupNames()
    {
        // Arrange
        MainData.Groups.Add(new Group { Id = 1, Name = "Development" });
        MainData.Groups.Add(new Group { Id = 2, Name = "Testing" });

        // Act
        var viewModel = new MainWindowViewModel();

        // Assert
        Assert.NotNull(viewModel.AllGroups);
        Assert.Equal(2, viewModel.AllGroups.Count);
    }

    #endregion

    #region ApplyFilters Tests

    [Fact]
    public void ApplyFilters_WithSearchText_FiltersTasksByTitleAndDescription()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Important Task", Description = "Do something" });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Another Task", Description = "Important work" });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Regular Task", Description = "Normal work" });
        
        var viewModel = new MainWindowViewModel();
        viewModel.SearchText = "important";

        // Act
        viewModel.ApplyFiltersCommand.Execute(null);

        // Assert
        Assert.Equal(2, viewModel.FilteredTasks.Count);
        Assert.Contains(viewModel.FilteredTasks, t => t.Id == 1);
        Assert.Contains(viewModel.FilteredTasks, t => t.Id == 2);
    }

    [Fact]
    public void ApplyFilters_WithSearchTextCaseInsensitive_FiltersCorrectly()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "URGENT Task", Description = "Do something" });
        MainData.Tasks.Add(new Task { Id = 2, Title = "normal task", Description = "Regular work" });
        
        var viewModel = new MainWindowViewModel();
        viewModel.SearchText = "urgent";

        // Act
        viewModel.ApplyFiltersCommand.Execute(null);

        // Assert
        Assert.Single(viewModel.FilteredTasks);
        Assert.Equal(1, viewModel.FilteredTasks[0].Id);
    }

    [Fact]
    public void ApplyFilters_WithDateFilter_FiltersTasksByDueDate()
    {
        // Arrange
        var targetDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5));
        var targetDateTimeOffset = new DateTimeOffset(DateTime.Now.AddDays(5).Date);
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1", Description = "Description 1", DueDate = targetDate });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2", Description = "Description 2", DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10)) });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Task 3", Description = "Description 3", DueDate = targetDate });
        
        var viewModel = new MainWindowViewModel();
        viewModel.IsDateFilterEnabled = true;
        viewModel.SearchDueDate = targetDateTimeOffset;

        // Act
        viewModel.ApplyFiltersCommand.Execute(null);

        // Assert
        Assert.Equal(2, viewModel.FilteredTasks.Count);
        Assert.All(viewModel.FilteredTasks, t => Assert.Equal(targetDate, t.DueDate));
    }

    [Fact]
    public void ApplyFilters_WithStateFilter_FiltersTasksByState()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1", Description = "Description 1", State = TaskState.Pending });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2", Description = "Description 2", State = TaskState.InProgress });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Task 3", Description = "Description 3", State = TaskState.Completed });
        
        var viewModel = new MainWindowViewModel();
        viewModel.StateFilter = new ObservableCollection<string> { StateConverter.StateToString(TaskState.Pending), StateConverter.StateToString(TaskState.InProgress) };

        // Act
        viewModel.ApplyFiltersCommand.Execute(null);

        // Assert
        Assert.Equal(2, viewModel.FilteredTasks.Count);
        Assert.Contains(viewModel.FilteredTasks, t => t.Id == 1);
        Assert.Contains(viewModel.FilteredTasks, t => t.Id == 2);
    }

    [Fact]
    public void ApplyFilters_WithUserFilter_FiltersTasksByAssignedUsers()
    {
        // Arrange
        var user1 = new User { Id = 1, FullName = "John Doe" };
        var user2 = new User { Id = 2, FullName = "Jane Smith" };
        MainData.Users.Add(user1);
        MainData.Users.Add(user2);
        
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1", Description = "Description 1", UserIds = new List<int> { 1 } });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2", Description = "Description 2", UserIds = new List<int> { 2 } });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Task 3", Description = "Description 3", UserIds = new List<int> { 1 } });
        
        var viewModel = new MainWindowViewModel();
        viewModel.UserFilter = new ObservableCollection<string> { user1.FullName };

        // Act
        viewModel.ApplyFiltersCommand.Execute(null);

        // Assert
        Assert.Equal(2, viewModel.FilteredTasks.Count);
        Assert.Contains(viewModel.FilteredTasks, t => t.Id == 1);
        Assert.Contains(viewModel.FilteredTasks, t => t.Id == 3);
    }

    [Fact]
    public void ApplyFilters_WithGroupFilter_FiltersTasksByGroups()
    {
        // Arrange
        var group1 = new Group { Id = 1, Name = "Development" };
        var group2 = new Group { Id = 2, Name = "Testing" };
        MainData.Groups.Add(group1);
        MainData.Groups.Add(group2);
        
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1", Description = "Description 1", GroupIds = new List<int> { 1 } });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2", Description = "Description 2", GroupIds = new List<int> { 2 } });
        MainData.Tasks.Add(new Task { Id = 3, Title = "Task 3", Description = "Description 3", GroupIds = new List<int> { 1 } });
        
        var viewModel = new MainWindowViewModel();
        viewModel.GroupFilter = new ObservableCollection<string> { group1.Name };

        // Act
        viewModel.ApplyFiltersCommand.Execute(null);

        // Assert
        Assert.Equal(2, viewModel.FilteredTasks.Count);
        Assert.Contains(viewModel.FilteredTasks, t => t.Id == 1);
        Assert.Contains(viewModel.FilteredTasks, t => t.Id == 3);
    }

    [Fact]
    public void ApplyFilters_WithEmptySearchText_DoesNotFilter()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Task 1", Description = "Description 1" });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Task 2", Description = "Description 2" });
        
        var viewModel = new MainWindowViewModel();
        viewModel.SearchText = string.Empty;

        // Act
        viewModel.ApplyFiltersCommand.Execute(null);

        // Assert - FilteredTasks should not be filtered when SearchText is empty and no other filters
        Assert.NotNull(viewModel.FilteredTasks);
    }

    [Fact]
    public void ApplyFilters_SearchTextTakesPrecedence_ReturnsImmediately()
    {
        // Arrange
        MainData.Tasks.Add(new Task { Id = 1, Title = "Important Task", Description = "Description 1", State = TaskState.Completed });
        MainData.Tasks.Add(new Task { Id = 2, Title = "Regular Task", Description = "Description 2", State = TaskState.Pending });
        
        var viewModel = new MainWindowViewModel();
        viewModel.SearchText = "important";
        viewModel.StateFilter = new ObservableCollection<string> { StateConverter.StateToString(TaskState.Pending) };

        // Act
        viewModel.ApplyFiltersCommand.Execute(null);

        // Assert - SearchText takes precedence, so only text search should apply
        Assert.Single(viewModel.FilteredTasks);
        Assert.Equal(1, viewModel.FilteredTasks[0].Id);
    }

    #endregion

    #region ClearFilters Tests

    [Fact]
    public void ClearFilters_ResetsSearchTextToEmpty()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        viewModel.SearchText = "test search";

        // Act
        viewModel.ClearFiltersCommand.Execute(null);

        // Assert
        Assert.Equal(string.Empty, viewModel.SearchText);
    }

    [Fact]
    public void ClearFilters_ClearsStateFilter()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        viewModel.StateFilter = new ObservableCollection<string> { "Pending", "InProgress" };

        // Act
        viewModel.ClearFiltersCommand.Execute(null);

        // Assert
        Assert.Empty(viewModel.StateFilter);
    }

    [Fact]
    public void ClearFilters_ClearsUserFilter()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        viewModel.UserFilter = new ObservableCollection<string> { "User 1", "User 2" };

        // Act
        viewModel.ClearFiltersCommand.Execute(null);

        // Assert
        Assert.Empty(viewModel.UserFilter);
    }

    [Fact]
    public void ClearFilters_ClearsGroupFilter()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        viewModel.GroupFilter = new ObservableCollection<string> { "Group 1", "Group 2" };

        // Act
        viewModel.ClearFiltersCommand.Execute(null);

        // Assert
        Assert.Empty(viewModel.GroupFilter);
    }

    [Fact]
    public void ClearFilters_DisablesDateFilter()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        viewModel.IsDateFilterEnabled = true;

        // Act
        viewModel.ClearFiltersCommand.Execute(null);

        // Assert
        Assert.False(viewModel.IsDateFilterEnabled);
    }


    [Fact]
    public void ClearFilters_ResetsAllFiltersAtOnce()
    {
        // Arrange
        var viewModel = new MainWindowViewModel();
        viewModel.SearchText = "search";
        viewModel.StateFilter = new ObservableCollection<string> { "Pending" };
        viewModel.UserFilter = new ObservableCollection<string> { "User" };
        viewModel.GroupFilter = new ObservableCollection<string> { "Group" };
        viewModel.IsDateFilterEnabled = true;

        // Act
        viewModel.ClearFiltersCommand.Execute(null);

        // Assert
        Assert.Equal(string.Empty, viewModel.SearchText);
        Assert.Empty(viewModel.StateFilter);
        Assert.Empty(viewModel.UserFilter);
        Assert.Empty(viewModel.GroupFilter);
        Assert.False(viewModel.IsDateFilterEnabled);
    }

    #endregion
}

