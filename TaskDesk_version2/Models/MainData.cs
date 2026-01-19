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
        Tasks = TasksOperator.LoadTasksFromJson();
        
        Users = UserOperator.LoadUsersFromJson();
        
        Groups = GroupOperator.LoadGroupsFromJson();
    }
}