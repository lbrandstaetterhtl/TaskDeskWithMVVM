using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace TaskDesk_version2.Models;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    public TaskState State { get; set; }

    public string StateAsString
    {
        get => GetTaskStateAsString();
        set {}
    }
    public List<int> GroupIds { get; set; } = new List<int>();
    public List<int> UserIds { get; set; } = new List<int>();

    public string GroupsAsString
    {
        get => GetGroupsAsString(MainData.Groups);
        set {  }
    }

    public string UsersAsString
    {
        get => GetUsersAsString(MainData.Users);
        set {  }
    }
    
    public string DateAsString
    {
        get => GetDateAsString();
        set { }
    }

    public Task(int id, string title, string description, DateOnly dueDate, TaskState state, List<int> groupIds, List<int> userIds)
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

    public Task() { }

    public string GetTaskStateAsString()
    {
        return StateConverter.StateToString(State);
    }
    
    public TaskState GetTaskStateFromString(string state)
    {
        return StateConverter.StringToState(state);
    }

    private string GetUsersAsString(ObservableCollection<User> allUsers)
    {
        List<string> userNames = new List<string>();
        
        foreach (var id in UserIds)
        {
            foreach (var user in allUsers)
            {
                if (id == user.Id)
                {
                    userNames.Add(user.FullName);
                    break;
                }
            }
        }

        return userNames.Count > 0 ? string.Join(", ", userNames) : "No users assigned";
    }
    
    private string GetGroupsAsString(ObservableCollection<Group> allGroups)
    {
        List<string> groupNames = new List<string>();
        
        foreach (var id in GroupIds)
        {
            foreach (var group in allGroups)
            {
                if (id == group.Id)
                {
                    groupNames.Add(group.Name);
                    break;
                }
            }
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
        string filePath = MainData.DataPath + "/tasks.json";
        
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            
            return new ObservableCollection<Task>();
        }
        
        string json = File.ReadAllText(filePath);
        
        return System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<Task>>(json) ?? new ObservableCollection<Task>();
    }

    public static void SaveTasksToJson(ObservableCollection<Task> tasks)
    {
        string filePath = MainData.DataPath + "/tasks.json";
        
        string json = System.Text.Json.JsonSerializer.Serialize(tasks, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        
        File.WriteAllText(filePath, json);
    }
    
    public static int GetNextTaskId()
    {
        if (MainData.Tasks.Count == 0)
            return 1;

        return MainData.Tasks.Max(t => t.Id) + 1;
    }
    
    public static ObservableCollection<Task> GetListFromIds(List<int> taskIds, ObservableCollection<Task> allTasks)
    {
        ObservableCollection<Task> tasks = new ObservableCollection<Task>();
        
        foreach (var id in taskIds)
        {
            foreach (var task in allTasks)
            {
                if (id == task.Id)
                {
                    tasks.Add(task);
                    break;
                }
            }
        }

        return tasks;
    }
    
    public static List<int> GetIdsFromList(ObservableCollection<Task> tasks, ObservableCollection<Task> allTasks)
    {
        List<int> ids = new List<int>();
        
        foreach (var task in tasks)
        {
            foreach (var t in allTasks)
            {
                if (t.Id == task.Id)
                {
                    ids.Add(t.Id);
                    break;
                }
            }
        }

        return ids;
    }
}