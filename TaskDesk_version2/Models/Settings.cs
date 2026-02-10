using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TaskDesk_version2.Models;

public class Settings
{
    public int LastLoggedInUserId { get; set; }
    public List<int> SavedUserIds { get; set; } = new();
    public bool IsThemeDark { get; set; }


    public User GetLastLoggedInUser()
    {
        return UsersOperator.GetUserById(LastLoggedInUserId);
    }

    public List<string> GetSavedUserEmails()
    {
        var emails = new List<string>();

        foreach (var id in SavedUserIds)
        {
            var user = UsersOperator.GetUserById(id);
            if (user != null) emails.Add(user.Email);
        }

        return emails;
    }
}

public static class SettingsOperator
{
    public static Settings LoadSettingsFromJson()
    {
        try
        {
            var filePath = Path.Combine(MainData.DataPath, "settings.json");
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var settings = JsonSerializer.Deserialize<Settings>(json);
                AppLogger.Info($"Settings loaded from {filePath}");
                return settings ?? new Settings();
            }

            AppLogger.Warn($"No {filePath} found");
            File.Create(filePath).Close();
            AppLogger.Info($"{filePath} created");
            return new Settings();
        }
        catch (Exception e)
        {
            AppLogger.Error("Error loading settings: " + e.Message);
            return new Settings();
        }
    }

    public static void SaveSettingsToJson(Settings settings)
    {
        try
        {
            var filePath = Path.Combine(MainData.DataPath, "settings.json");
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            AppLogger.Info($"Settings saved to {filePath}");
        }
        catch (Exception e)
        {
            AppLogger.Error("Error saving settings: " + e.Message);
        }
    }
}