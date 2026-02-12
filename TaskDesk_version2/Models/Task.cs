using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using TaskDesk_version2.Views;

namespace TaskDesk_version2.Models;

public class Task
{
    public Task(int id, string title, string description, DateOnly dueDate, TaskState state, List<int> groupIds,
        List<int> userIds)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        State = state;
        GroupIds = groupIds;
        UserIds = userIds;
        GroupsAsString = GetGroupsAsString(MainData.Groups);
        UsersAsString = GetUsersAsString(MainData.Users);
    }

    public Task()
    {
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    public TaskState State { get; set; }

    public string StateAsString
    {
        get => GetTaskStateAsString();
        set { }
    }

    public List<int> GroupIds { get; set; } = new();
    public List<int> UserIds { get; set; } = new();

    public string GroupsAsString
    {
        get => GetGroupsAsString(MainData.Groups);
        set { }
    }

    public string UsersAsString
    {
        get => GetUsersAsString(MainData.Users);
        set { }
    }

    public string DateAsString
    {
        get => GetDateAsString();
        set { }
    }

    public string GetTaskStateAsString()
    {
        return StateConverter.StateToString(State);
    }

    private string GetUsersAsString(ObservableCollection<User> allUsers)
    {
        var userNames = new List<string>();

        foreach (var id in UserIds)
        foreach (var user in allUsers)
            if (id == user.Id)
            {
                userNames.Add(user.FullName);
                break;
            }

        return userNames.Count > 0 ? string.Join(", ", userNames) : "No users assigned";
    }

    private string GetGroupsAsString(ObservableCollection<Group> allGroups)
    {
        var groupNames = new List<string>();

        foreach (var id in GroupIds)
        foreach (var group in allGroups)
            if (id == group.Id)
            {
                groupNames.Add(group.Name);
                break;
            }

        return groupNames.Count > 0 ? string.Join(", ", groupNames) : "No groups assigned";
    }

    private string GetDateAsString()
    {
        return DueDate.ToString("dd/MM/yyyy");
    }
}

public static class TasksOperator
{
    public static ObservableCollection<Task> LoadTasksFromJson()
    {
        if (Design.IsDesignMode) return null;

        try
        {
            var filePath = MainData.DataPath + @"\tasks.json";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();

                AppLogger.Warn($"No {filePath} found");
                AppLogger.Info($"{filePath} created");

                return new ObservableCollection<Task>();
            }

            var json = File.ReadAllText(filePath);

            AppLogger.Info($"Tasks loaded from {filePath}");

            return JsonSerializer.Deserialize<ObservableCollection<Task>>(json) ??
                   new ObservableCollection<Task>();
        }
        catch (Exception ex)
        {
            var errorWindow = new ErrorWindow("Error loading tasks from JSON:\n" + ex.Message);
            errorWindow.ShowDialog(App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow!
                : null);

            AppLogger.Error("Error loading tasks: " + ex.Message);
            return new ObservableCollection<Task>();
        }
    }

    public static void SaveTasksToJson(ObservableCollection<Task> tasks)
    {
        if (Design.IsDesignMode) return;

        try
        {
            var filePath = MainData.DataPath + @"\tasks.json";

            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);

            AppLogger.Info($"Tasks saved to {filePath}");
        }
        catch (Exception ex)
        {
            var errorWindow = new ErrorWindow("Error saving tasks to JSON:\n" + ex.Message);
            errorWindow.ShowDialog(App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow!
                : null);

            AppLogger.Error("Error saving tasks: " + ex.Message);
        }
    }

    public static int GetNextTaskId()
    {
        if (MainData.Tasks.Count == 0)
            return 1;

        return MainData.Tasks.Max(t => t.Id) + 1;
    }

    public static ObservableCollection<Task> GetListFromIds(List<int> taskIds, ObservableCollection<Task> allTasks)
    {
        var tasks = new ObservableCollection<Task>();

        foreach (var id in taskIds)
        foreach (var task in allTasks)
            if (id == task.Id)
            {
                tasks.Add(task);
                break;
            }

        return tasks;
    }

    public static List<int> GetIdsFromList(ObservableCollection<Task> tasks, ObservableCollection<Task> allTasks)
    {
        var ids = new List<int>();

        foreach (var task in tasks)
        foreach (var t in allTasks)
            if (t.Id == task.Id)
            {
                ids.Add(t.Id);
                break;
            }

        return ids;
    }
    
    public static ObservableCollection<Task> SearchInTasksForString(string searchText, IList<Task> allTasks)
    {
        var lowerSearchText = searchText.ToLower();
        var result = new ObservableCollection<Task>();

        foreach (var task in allTasks)
        {
            if (task.Title.ToLower().Contains(lowerSearchText) || task.Description.ToLower().Contains(lowerSearchText))
            {
                result.Add(task);
            }
        }

        return result;
    }
    
    public static ObservableCollection<Task> SearchInTasksForDate(DateTimeOffset date, IList<Task> allTasks)
    {
        var result = new ObservableCollection<Task>();
        var dateOnly = DateOnly.FromDateTime(date.DateTime);

        foreach (var task in allTasks)
            if (task.DueDate == dateOnly)
                result.Add(task);

        return result;
    }
    
    public static ObservableCollection<Task> SearchInTasksForUsers(IList<string> userFullNames, IList<Task> allTasks)
    {
        var result = new ObservableCollection<Task>();

        foreach (var task in allTasks)
        {
           foreach (var userId in task.UserIds)
           {
                var user = UsersOperator.GetUserById(userId);
                if (user != null && userFullNames.Contains(user.FullName))
                {
                    result.Add(task);
                    break;
                }
           }
        }

        return result;
    }
    
    public static ObservableCollection<Task> SearchInTasksForGroups(IList<string> groupNames, IList<Task> allTasks)
    {
        var result = new ObservableCollection<Task>();

        foreach (var task in allTasks)
        {
            foreach (var groupId in task.GroupIds)
            {
                var group = GroupsOperator.GetGroupById(groupId);
                if (group != null && groupNames.Contains(group.Name))
                {
                    result.Add(task);
                    break;
                }
            }
        }

        return result;
    }
    
    public static ObservableCollection<Task> SearchInTasksForStates(IList<string> stateStrings, IList<Task> allTasks)
    {
        var result = new ObservableCollection<Task>();

        foreach (var task in allTasks)
        {
            var stateString = StateConverter.StateToString(task.State);

            if (stateStrings.Contains(stateString))
            {
                result.Add(task);
            }
        }

        return result;
    }
}