using System;
using System.Collections.ObjectModel;

namespace TaskDesk_version2.Models;

public class MainData
{
    public static string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TaskDeskData";

    public MainData()
    {
        var loadedSettings = SettingsOperator.LoadSettingsFromJson();

        Settings = loadedSettings;

        (App.Current as App).SetTheme(Settings.IsThemeDark);

        var loadedTasks = TasksOperator.LoadTasksFromJson();

        Tasks = loadedTasks;

        var loadedUsers = UsersOperator.LoadUsersFromJson();

        Users = loadedUsers;

        var loadedGroups = GroupsOperator.LoadGroupsFromJson();

        Groups = loadedGroups;
    }

    public static ObservableCollection<Task> Tasks { get; set; } = new();
    public static ObservableCollection<User> Users { get; set; } = new();
    public static ObservableCollection<Group> Groups { get; set; } = new();
    public static User CurrentUser { get; set; }
    public static Settings Settings { get; set; } = new();
}