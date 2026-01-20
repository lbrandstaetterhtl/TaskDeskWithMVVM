using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace TaskDesk_version2.Models;

public class Group(int id, string name, string description)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public List<int> UsersIds { get; set; }
    public List<int> TasksIds { get; set; }
    
    public string GetUsersAsString(List<User> allUsers)
    {
        List<string> userNames = new List<string>();
        
        foreach (var id in UsersIds)
        {
            var user = allUsers.Find(x => x.Id == id);
            
            if (user != null)
            {
                userNames.Add(user.FullName);
            }
        }

        if (userNames.Count > 0)
        {
            return string.Join(", ", userNames);
        }
        
        return string.Empty;
    }
    
    public string GetTasksAsString(List<Task> allTasks)
    {
        List<string> taskTitles = new List<string>();
        
        foreach (var id in TasksIds)
        {
            var task = allTasks.Find(x => x.Id == id);
            
            if (task != null)
            {
                taskTitles.Add(task.Title);
            }
        }

        if (taskTitles.Count > 0)
        {
            return string.Join(", ", taskTitles);
        }
        
        return string.Empty;
    }
}

public static class GroupOperator
{
    public static ObservableCollection<Group> LoadGroupsFromJson()
    {
        string filePath = MainData.DataPath + "/groups.json";

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            return new ObservableCollection<Group>();
        }
        
        string json = File.ReadAllText(filePath);
        
        return System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<Group>>(json) ?? new ObservableCollection<Group>();
    }

    public static void SaveGroupsToJson(ObservableCollection<Group> allGroups)
    {
        string filePath = MainData.DataPath + "/groups.json";
        
        string json = System.Text.Json.JsonSerializer.Serialize(allGroups, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        
        File.WriteAllText(filePath, json);
    }
    
    public static List<int> GetIdsFromNames(List<string> groupNames, ObservableCollection<Group> allGroups)
    {
        List<int> ids = new List<int>();
        
        foreach (var name in groupNames)
        {
            foreach (var group in allGroups)
            {
                if (group.Name == name)
                {
                    ids.Add(group.Id);
                }
            }
        }

        return ids;
    }

    public static List<string> GetNamesFromIds(List<int> groupIds, List<Group> allGroups)
    {
        List<string> names = new List<string>();
        
        foreach (var id in groupIds)
        {
            var group = allGroups.Find(x => x.Id == id);
            
            if (group != null)
            {
                names.Add(group.Name);
            }
        }
        
        return names;
    }
}