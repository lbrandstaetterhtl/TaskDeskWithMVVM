using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaskDesk_version2.Models;

public class MainData
{
    public static ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
    public static List<User> Users { get; set; } = new List<User>();
    public static List<Group> Groups { get; set; } = new List<Group>();
    public static string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/TaskDeskData";
    
    public MainData()
    {
        var loadedTasks = TasksOperator.LoadTasksFromJson();
        Tasks.Clear();
        foreach (var t in loadedTasks)
            Tasks.Add(t);
        
        var loadedUsers = UserOperator.LoadUsersFromJson();
        Users.Clear();
        Users.AddRange(loadedUsers);
        
        var loadedGroups = GroupOperator.LoadGroupsFromJson();
        Groups.Clear();
        Groups.AddRange(loadedGroups);
    }
}