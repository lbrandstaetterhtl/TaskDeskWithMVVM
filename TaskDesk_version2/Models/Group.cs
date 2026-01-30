using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace TaskDesk_version2.Models;

public class Group
{
    private int _id;
    
    public int Id 
    { 
        get
        {
            if (_id == 0)
            {
                _id = GroupsOperator.GetNextGroupId();
            }
            return _id;
        }
        set => _id = value;
    }
    
    public string Name { get; set; } 
    public string Description { get; set; }
    public List<int> UserIds { get; set; } = new List<int>();
    public List<int> TaskIds { get; set; } = new List<int>();
    
    public Group() { }
    
    public Group(string name, string description, List<int> usersIds)
    {
        Name = name;
        Description = description;
        UserIds = usersIds;
    }
    
    public string GetUsersAsString(List<User> allUsers)
    {
        List<string> userNames = new List<string>();
        
        foreach (var id in UserIds)
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
}

public static class GroupsOperator
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
                    break;
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
    
    public static ObservableCollection<Group> GetListFromIds(List<int> groupIds, ObservableCollection<Group> allGroups)
    {
        ObservableCollection<Group> groups = new ObservableCollection<Group>();
        
        foreach (var id in groupIds)
        {
            foreach (var group in allGroups)
            {
                if (group.Id == id)
                {
                    groups.Add(group);
                    break;
                }
            }
        }
        
        return groups;
    }
    
    public static List<int> GetIdsFromList(ObservableCollection<Group> groups, ObservableCollection<Group> allGroups)
    {
        List<int> resultGroupIds = new List<int>();
        
        foreach (var group in groups)
        {
            foreach (var groupFromData in allGroups)
            {
                if (group.Id == groupFromData.Id)
                {
                    resultGroupIds.Add(group.Id);
                    break;
                }
            }
        }
        
        return resultGroupIds;
    }
    
    public static int GetNextGroupId()
    {
        if (MainData.Groups.Count == 0)
            return 1;

        return MainData.Groups.Max(t => t.Id) + 1;
    }
    
    public static Group GetGroupById(int groupId)
    {
        foreach (var group in MainData.Groups)
        {
            if (group.Id == groupId)
            {
                return group;
            }
        }

        return null;
    }
}