using System.Collections.Generic;

namespace TaskDesk_version2.Models;

public enum UserRole
{
    Admin,
    User,
    ReadOnly
}

public class RoleConverter
{
    public static string RoleToString(UserRole role)
    {
        return role switch
        {
            UserRole.Admin => "Admin",
            UserRole.User => "User",
            UserRole.ReadOnly => "Read-Only",
            _ => "Unknown"
        };
    }
    
    public static UserRole StringToRole(string role)
    {
        return role switch
        {
            "Admin" => UserRole.Admin,
            "User" => UserRole.User,
            "Read-Only" => UserRole.ReadOnly,
            _ => throw new KeyNotFoundException($"Role '{role}' not recognized.")
        };
    }
}