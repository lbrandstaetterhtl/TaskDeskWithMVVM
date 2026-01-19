using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace TaskDesk_version2.Models;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    public TaskState State { get; set; }
    public List<int> GroupIds { get; set; } = new List<int>();
    public List<int> UserIds { get; set; } = new List<int>();
    public string GroupsAsString => GetGroupsAsString(MainData.Groups);
    public string UsersAsString => GetUsersAsString(MainData.Users);
    

    public Task(int id, string title, string description, DateOnly dueDate, TaskState state, List<int> groupIds)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        State = state;
        GroupIds = groupIds;
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

    private string GetUsersAsString(List<User> allUsers)
    {
        List<string> userNames = new List<string>();
        
        foreach (var id in UserIds)
        {
            var user = allUsers.Find(x => x.Id == id);
            if (user != null)
                userNames.Add(user.FullName);
        }

        return userNames.Count > 0 ? string.Join(", ", userNames) : string.Empty;
    }
    
    private string GetGroupsAsString(List<Group> allGroups)
    {
        List<string> groupNames = new List<string>();
        
        foreach (var id in GroupIds)
        {
            var group = allGroups.Find(x => x.Id == id);
            if (group != null)
                groupNames.Add(group.Name);
        }

        return groupNames.Count > 0 ? string.Join(", ", groupNames) : string.Empty;
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
}