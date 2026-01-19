using System.Collections.Generic;

namespace TaskDesk_version2.Models;

public enum TaskState
{
    Pending,
    InProgress,
    Completed,
    OnHold,
    Cancelled
}

public static class StateConverter
{
    public static string StateToString(TaskState state)
    {
        return state switch
        {
            TaskState.Pending => "Pending...",
            TaskState.InProgress => "In Progress...",
            TaskState.Completed => "Completed!",
            TaskState.OnHold => "On Hold...",
            TaskState.Cancelled => "Cancelled!",
            _ => "Unknown"
        };
    }

    public static TaskState StringToState(string state)
    {
        return state switch
        {
            "Pending..." => TaskState.Pending,
            "In Progress..." => TaskState.InProgress,
            "Completed!" => TaskState.Completed,
            "On Hold..." => TaskState.OnHold,
            "Cancelled!" => TaskState.Cancelled,
            _ => throw new KeyNotFoundException($"State '{state}' not recognized.")
        };
    }
}