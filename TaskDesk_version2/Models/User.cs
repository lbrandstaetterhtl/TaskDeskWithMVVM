using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using TaskDesk_version2.Views;

namespace TaskDesk_version2.Models;

public class User
{
    public User(string fullName, string email, string password, UserRole role, List<int> groupIds)
    {
        Id = UsersOperator.GetNextUserId();
        FullName = fullName;
        Email = email;
        Password = password;
        Role = role;
        GroupIds = groupIds;
    }

    public User()
    {
    }

    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public List<int> GroupIds { get; set; } = new();
    public List<int> TaskIds { get; set; } = new();

    public string GroupsAsString
    {
        get => GetGroupsAsString(MainData.Groups);
        set { }
    }

    public string TasksAsString
    {
        get => GetTasksAsString(MainData.Tasks);
        set { }
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
        var taskTitles = new List<string>();

        foreach (var id in TaskIds)
        {
            var task = allTasks.Find(x => x.Id == id);

            if (task != null) taskTitles.Add(task.Title);
        }

        if (taskTitles.Count > 0) return string.Join(", ", taskTitles);

        return string.Empty;
    }

    public string GetGroupsAsString(List<Group> allGroups)
    {
        var groupNames = new List<string>();

        foreach (var id in GroupIds)
        {
            var group = allGroups.Find(x => x.Id == id);

            if (group != null) groupNames.Add(group.Name);
        }

        if (groupNames.Count > 0) return string.Join(", ", groupNames);

        return string.Empty;
    }

    private string GetGroupsAsString(ObservableCollection<Group> allGroups)
    {
        var groupNames = new List<string>();

        foreach (var id in GroupIds)
        foreach (var group in allGroups)
            if (id == group.Id)
            {
                groupNames.Add(group.Name);
                break;
            }

        return string.Join(", ", groupNames);
    }

    private string GetTasksAsString(ObservableCollection<Task> allTasks)
    {
        var taskTitles = new List<string>();

        foreach (var id in TaskIds)
        foreach (var task in allTasks)
            if (id == task.Id)
            {
                taskTitles.Add(task.Title);
                break;
            }

        return string.Join(", ", taskTitles);
    }
}

public static class UsersOperator
{
    public static ObservableCollection<User> LoadUsersFromJson()
    {
        if (Design.IsDesignMode) return null;

        try
        {
            var filePath = MainData.DataPath + @"\users.json";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();

                AppLogger.Warn($"No {filePath} found");
                AppLogger.Info($"{filePath} created");
                AppLogger.Info("Added Test User as no users.json found. E-Mail: TestU0, Password: TestU0");

                var users = new ObservableCollection<User>
                {
                    new("TestU0", "TestU0", "TestU0", UserRole.User, new List<int>())
                };

                return users;
            }

            var json = File.ReadAllText(filePath);

            AppLogger.Info($"Users loaded from {filePath}");

            return JsonSerializer.Deserialize<ObservableCollection<User>>(json) ??
                   new ObservableCollection<User>();
        }
        catch (Exception ex)
        {
            var errorWindow = new ErrorWindow($"An error occurred while loading users: {ex.Message}");
            errorWindow.ShowDialog(App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow!
                : null);

            AppLogger.Error("Error loading users: " + ex.Message);

            return new ObservableCollection<User>();
        }
    }

    public static void SaveUsersToJson(ObservableCollection<User> allUsers)
    {
        if (Design.IsDesignMode) return;

        try
        {
            var filePath = MainData.DataPath + @"\users.json";

            var json = JsonSerializer.Serialize(allUsers, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);

            AppLogger.Info("Users saved to " + filePath);
        }
        catch (Exception ex)
        {
            var errorWindow = new ErrorWindow($"An error occurred while saving users: {ex.Message}");
            errorWindow.ShowDialog(App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow!
                : null);

            AppLogger.Error("Error saving users: " + ex.Message);
        }
    }

    public static List<int> GetIdsFromNames(List<string> userNames, ObservableCollection<User> allUsers)
    {
        var ids = new List<int>();

        foreach (var name in userNames)
        foreach (var user in allUsers)
            if (user.FullName == name)
                ids.Add(user.Id);

        return ids;
    }

    public static ObservableCollection<User> GetListFromIds(List<int> userIds, ObservableCollection<User> allUsers)
    {
        var users = new ObservableCollection<User>();

        foreach (var id in userIds)
        foreach (var user in allUsers)
            if (user.Id == id)
            {
                users.Add(user);
                break;
            }

        return users;
    }

    public static List<int> GetIdsFromList(ObservableCollection<User> users, ObservableCollection<User> allUsers)
    {
        var ids = new List<int>();

        foreach (var user in users)
        foreach (var userFromData in allUsers)
            if (user.Id == userFromData.Id)
                ids.Add(user.Id);

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
            if (user.Id == userId)
                return user;

        return null;
    }

    public static User GetUserByEmail(string email)
    {
        foreach (var user in MainData.Users)
            if (user.Email == email)
                return user;

        return null;
    }

    public static User GetUserByFullname(string fullname)
    {
        foreach (var user in MainData.Users)
            if (user.FullName == fullname)
                return user;

        return null;
    }

    public static List<string> GetAllUserFullNames()
    {
        var fullNames = new List<string>();

        foreach (var user in MainData.Users) fullNames.Add(user.FullName);

        return fullNames;
    }
}