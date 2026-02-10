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

public class Group
{
    public Group()
    {
    }

    public Group(string name, string description, List<int> usersIds)
    {
        Id = GroupsOperator.GetNextGroupId();
        Name = name;
        Description = description;
        UserIds = usersIds;
    }

    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public List<int> UserIds { get; set; } = new();
    public List<int> TaskIds { get; set; } = new();

    public string GetUsersAsString(List<User> allUsers)
    {
        var userNames = new List<string>();

        foreach (var id in UserIds)
        {
            var user = allUsers.Find(x => x.Id == id);

            if (user != null) userNames.Add(user.FullName);
        }

        if (userNames.Count > 0) return string.Join(", ", userNames);

        return string.Empty;
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
}

public static class GroupsOperator
{
    public static ObservableCollection<Group> LoadGroupsFromJson()
    {
        if (Design.IsDesignMode) return null;

        try
        {
            var filePath = MainData.DataPath + @"\groups.json";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();

                AppLogger.Warn($"No {filePath} found");
                AppLogger.Info($"{filePath} created");

                return new ObservableCollection<Group>();
            }

            var json = File.ReadAllText(filePath);

            AppLogger.Info($"Groups loaded from {filePath}");

            return JsonSerializer.Deserialize<ObservableCollection<Group>>(json) ??
                   new ObservableCollection<Group>();
        }
        catch (Exception ex)
        {
            var errorWindow = new ErrorWindow("Error loading groups: " + ex.Message);
            errorWindow.ShowDialog(App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow!
                : null);

            AppLogger.Error("Error loading groups: " + ex.Message);

            return new ObservableCollection<Group>();
        }
    }

    public static void SaveGroupsToJson(ObservableCollection<Group> allGroups)
    {
        if (Design.IsDesignMode) return;

        try
        {
            var filePath = MainData.DataPath + @"\groups.json";

            var json = JsonSerializer.Serialize(allGroups,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(filePath, json);

            AppLogger.Info($"Groups saved to {filePath}");
        }
        catch (Exception ex)
        {
            var errorWindow = new ErrorWindow("Error saving groups: " + ex.Message);
            errorWindow.ShowDialog(App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow!
                : null);

            AppLogger.Error("Error saving groups: " + ex.Message);
        }
    }

    public static List<int> GetIdsFromNames(List<string> groupNames, ObservableCollection<Group> allGroups)
    {
        var ids = new List<int>();

        foreach (var name in groupNames)
        foreach (var group in allGroups)
            if (group.Name == name)
            {
                ids.Add(group.Id);
                break;
            }

        return ids;
    }

    public static List<string> GetNamesFromIds(List<int> groupIds, List<Group> allGroups)
    {
        var names = new List<string>();

        foreach (var id in groupIds)
        {
            var group = allGroups.Find(x => x.Id == id);

            if (group != null) names.Add(group.Name);
        }

        return names;
    }

    public static ObservableCollection<Group> GetListFromIds(List<int> groupIds, ObservableCollection<Group> allGroups)
    {
        var groups = new ObservableCollection<Group>();

        foreach (var id in groupIds)
        foreach (var group in allGroups)
            if (group.Id == id)
            {
                groups.Add(group);
                break;
            }

        return groups;
    }

    public static List<int> GetIdsFromList(ObservableCollection<Group> groups, ObservableCollection<Group> allGroups)
    {
        var resultGroupIds = new List<int>();

        foreach (var group in groups)
        foreach (var groupFromData in allGroups)
            if (group.Id == groupFromData.Id)
            {
                resultGroupIds.Add(group.Id);
                break;
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
            if (group.Id == groupId)
                return group;

        return null;
    }

    public static List<string> GetAllGroupNames()
    {
        var groupNames = new List<string>();

        foreach (var group in MainData.Groups) groupNames.Add(group.Name);

        return groupNames;
    }
}