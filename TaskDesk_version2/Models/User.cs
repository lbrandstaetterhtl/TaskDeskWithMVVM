using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace TaskDesk_version2.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public List<int> GroupIds { get; set; } = new List<int>();
    public List<int> TaskIds { get; set; } = new List<int>();
    
    public string GroupsAsString
    {
        get => GetGroupsAsString(MainData.Groups);
        set {  }
    }
    
    public string TasksAsString
    {
        get => GetTasksAsString(MainData.Tasks);
        set {  }
    }
    
    public User( string fullName, string email, string password, UserRole role, List<int> groupIds)
    {
        Id = UsersOperator.GetNextUserId();
        FullName = fullName;
        Email = email;
        Password = password;
        Role = role;
        GroupIds = groupIds;
    }
    
    public User() { }
    
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
        
        return string.Join(", ", groupNames);
    }
    
    private string GetTasksAsString(ObservableCollection<Task> allTasks)
    {
        List<string> taskTitles = new List<string>();
        
        foreach (var id in TaskIds)
        {
            foreach (var task in allTasks)
            {
                if (id == task.Id)
                {
                    taskTitles.Add(task.Title);
                    break;
                }
            }
        }
        
        return string.Join(", ", taskTitles);
    }
}

public static class UsersOperator
{
    public static ObservableCollection<User> LoadUsersFromJson()
    {
        string filePath = MainData.DataPath + "/users.json";
        
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            return new ObservableCollection<User>();
        }
        
        string json = File.ReadAllText(filePath);
        
        return System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<User>>(json) ?? new ObservableCollection<User>();
    }

    public static void SaveUsersToJson(ObservableCollection<User> allUsers)
    {
        string filePath = MainData.DataPath + "/users.json";
        
        string json = System.Text.Json.JsonSerializer.Serialize(allUsers, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        
        File.WriteAllText(filePath, json);
    }
    
    public static List<int> GetIdsFromNames(List<string> userNames, ObservableCollection<User> allUsers)
    {
        List<int> ids = new List<int>();
        
        foreach (var name in userNames)
        {
            foreach (var user in allUsers)
            {
                if (user.FullName == name)
                {
                    ids.Add(user.Id);
                }
            }
        }

        return ids;
    }
    
    public static ObservableCollection<User> GetListFromIds(List<int> userIds, ObservableCollection<User> allUsers)
    {
        ObservableCollection<User> users = new ObservableCollection<User>();
        
        foreach (var id in userIds)
        {
            foreach (var user in allUsers)
            {
                if (user.Id == id)
                {
                    users.Add(user);
                    break;
                }
            }
        }

        return users;
    }
    
    public static List<int> GetIdsFromList(ObservableCollection<User> users, ObservableCollection<User> allUsers)
    {
        List<int> ids = new List<int>();
        
        foreach (var user in users)
        {
            foreach (var userFromData in allUsers)
            {
                if (user.Id == userFromData.Id)
                {
                    ids.Add(user.Id);
                }
            }
        }

        return ids;
    }
    
    public static int GetNextUserId()
    {
        if (MainData.Users.Count == 0)
            return 1;

        return MainData.Users.Max(t => t.Id) + 1;
    }
    
    public static User GetUserById(int userId)
    {
        foreach (var user in MainData.Users)
        {
            if (user.Id == userId)
            {
                return user;
            }
        }

        return null;
    }
    
    public static User GetUserByEmail(string email)
    {
        foreach (var user in MainData.Users)
        {
            if (user.Email == email)
            {
                return user;
            }
        }

        return null;
    }
    
    public static User GetUserByFullname(string fullname)
    {
        foreach (var user in MainData.Users)
        {
            if (user.FullName == fullname)
            {
                return user;
            }
        }

        return null;
    }
}