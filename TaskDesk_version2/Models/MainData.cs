using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaskDesk_version2.Models;

public class MainData
{
    public static ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
    public static ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
    public static ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();
    public static string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TaskDeskData";
    public static User CurrentUser { get; set; }

    public MainData()
    {
        var loadedTasks = TasksOperator.LoadTasksFromJson();
        
        Tasks = loadedTasks;
        
        var loadedUsers = UsersOperator.LoadUsersFromJson();
        
        Users = loadedUsers;
        
        var loadedGroups = GroupsOperator.LoadGroupsFromJson();
        Groups = loadedGroups;
        
    }
}