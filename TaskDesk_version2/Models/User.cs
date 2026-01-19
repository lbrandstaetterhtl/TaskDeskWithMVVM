using System.Collections.Generic;
using System.IO;

namespace TaskDesk_version2.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public UserRole Role { get; set; }
    public List<int> GroupIds { get; set; } = new List<int>();
    public List<int> TaskIds { get; set; } = new List<int>();
    
    public User(int id, string fullName, string email, string passwordHash, UserRole role)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
    
    public string GetRoleAsString()
    {
        return RoleConverter.RoleToString(Role);
    }
    
    public UserRole GetRoleFromString(string role)
    {
        return RoleConverter.StringToRole(role);
    }
    
    public string GetTasksAsString(List<Task> allTasks)
    {
        List<string> taskTitles = new List<string>();
        
        foreach (var id in TaskIds)
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
    
    public string GetGroupsAsString(List<Group> allGroups)
    {
        List<string> groupNames = new List<string>();
        
        foreach (var id in GroupIds)
        {
            var group = allGroups.Find(x => x.Id == id);
            
            if (group != null)
            {
                groupNames.Add(group.Name);
            }
        }

        if (groupNames.Count > 0)
        {
            return string.Join(", ", groupNames);
        }
        
        return string.Empty;
    }
}

public static class UserOperator
{
    public static List<User> LoadUsersFromJson()
    {
        string filePath = MainData.DataPath + "/users.json";
        
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            return new List<User>();
        }
        
        string json = File.ReadAllText(filePath);
        
        return System.Text.Json.JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    public static void SaveUsersToJson(List<User> allUsers)
    {
        string filePath = MainData.DataPath + "/users.json";
        
        string json = System.Text.Json.JsonSerializer.Serialize(allUsers, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        
        File.WriteAllText(filePath, json);
    }
}