using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaskDesk_version2.Models;

public class MainData
{
    public static ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
    public static ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
    public static ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();
    public static string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/TaskDeskData";

    public MainData()
    {
        var loadedTasks = TasksOperator.LoadTasksFromJson();
        Tasks.Clear();
        foreach (var t in loadedTasks)
            Tasks.Add(t);
        
        TasksOperator.SaveTasksToJson(Tasks);

        var loadedUsers = UserOperator.LoadUsersFromJson();
        Users.Clear();
        foreach (var u in loadedUsers)
            Users.Add(u);
        
        UserOperator.SaveUsersToJson(Users);

        var loadedGroups = GroupOperator.LoadGroupsFromJson();
        Groups.Clear();
        foreach (var g in loadedGroups)
            Groups.Add(g);
        
        GroupOperator.SaveGroupsToJson(Groups);
    }
}