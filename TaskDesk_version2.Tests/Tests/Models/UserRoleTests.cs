using TaskDesk_version2.Models;
using Xunit;
using System.Collections.Generic;

namespace TaskDesk_version2.Tests.Tests.Models;

/// <summary>
/// Unit-Tests für UserRole Enum und RoleConverter Klasse
/// </summary>
public class UserRoleTests
{
    #region RoleToString Tests

    [Fact]
    public void RoleToString_AdminRole_ReturnsAdminString()
    {
        // Arrange
        var role = UserRole.Admin;

        // Act
        var result = RoleConverter.RoleToString(role);

        // Assert
        Assert.Equal("Admin", result);
    }

    [Fact]
    public void RoleToString_UserRole_ReturnsUserString()
    {
        // Arrange
        var role = UserRole.User;

        // Act
        var result = RoleConverter.RoleToString(role);

        // Assert
        Assert.Equal("User", result);
    }

    [Fact]
    public void RoleToString_ReadOnlyRole_ReturnsReadOnlyString()
    {
        // Arrange
        var role = UserRole.ReadOnly;

        // Act
        var result = RoleConverter.RoleToString(role);

        // Assert
        Assert.Equal("Read-Only", result);
    }

    [Theory]
    [InlineData(UserRole.Admin, "Admin")]
    [InlineData(UserRole.User, "User")]
    [InlineData(UserRole.ReadOnly, "Read-Only")]
    public void RoleToString_AllValidRoles_ReturnsCorrectStrings(UserRole role, string expected)
    {
        // Act
        var result = RoleConverter.RoleToString(role);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region StringToRole Tests

    [Fact]
    public void StringToRole_AdminString_ReturnsAdminRole()
    {
        // Arrange
        var roleString = "Admin";

        // Act
        var result = RoleConverter.StringToRole(roleString);

        // Assert
        Assert.Equal(UserRole.Admin, result);
    }

    [Fact]
    public void StringToRole_UserString_ReturnsUserRole()
    {
        // Arrange
        var roleString = "User";

        // Act
        var result = RoleConverter.StringToRole(roleString);

        // Assert
        Assert.Equal(UserRole.User, result);
    }

    [Fact]
    public void StringToRole_ReadOnlyString_ReturnsReadOnlyRole()
    {
        // Arrange
        var roleString = "Read-Only";

        // Act
        var result = RoleConverter.StringToRole(roleString);

        // Assert
        Assert.Equal(UserRole.ReadOnly, result);
    }

    [Theory]
    [InlineData("Admin", UserRole.Admin)]
    [InlineData("User", UserRole.User)]
    [InlineData("Read-Only", UserRole.ReadOnly)]
    public void StringToRole_AllValidStrings_ReturnsCorrectRoles(string roleString, UserRole expected)
    {
        // Act
        var result = RoleConverter.StringToRole(roleString);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void StringToRole_InvalidString_ThrowsKeyNotFoundException()
    {
        // Arrange
        var invalidString = "InvalidRole";

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() => RoleConverter.StringToRole(invalidString));
        Assert.Contains("InvalidRole", exception.Message);
    }

    [Fact]
    public void StringToRole_EmptyString_ThrowsKeyNotFoundException()
    {
        // Arrange
        var emptyString = "";

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => RoleConverter.StringToRole(emptyString));
    }

    [Fact]
    public void StringToRole_NullString_ThrowsKeyNotFoundException()
    {
        // Arrange
        string? nullString = null;

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => RoleConverter.StringToRole(nullString!));
    }

    [Theory]
    [InlineData("admin")]       // Kleinbuchstaben
    [InlineData("ADMIN")]       // Großbuchstaben
    [InlineData(" Admin ")]     // Mit Leerzeichen
    [InlineData("Administrator")] // Andere Schreibweise
    [InlineData("read-only")]   // Kleinbuchstaben mit Bindestrich
    [InlineData("ReadOnly")]    // Ohne Bindestrich
    public void StringToRole_InvalidFormats_ThrowsKeyNotFoundException(string invalidFormat)
    {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => RoleConverter.StringToRole(invalidFormat));
    }

    #endregion

    #region Round-Trip Tests (Bidirektionale Konvertierung)

    [Theory]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.User)]
    [InlineData(UserRole.ReadOnly)]
    public void RoleConversion_RoundTrip_ReturnsOriginalRole(UserRole originalRole)
    {
        // Act
        var stringRepresentation = RoleConverter.RoleToString(originalRole);
        var convertedBack = RoleConverter.StringToRole(stringRepresentation);

        // Assert
        Assert.Equal(originalRole, convertedBack);
    }

    #endregion

    #region Enum Values Tests

    [Fact]
    public void UserRole_HasExpectedNumberOfValues()
    {
        // Act
        var values = Enum.GetValues(typeof(UserRole));

        // Assert
        Assert.Equal(3, values.Length);
    }

    [Fact]
    public void UserRole_ContainsAllExpectedValues()
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(UserRole), UserRole.Admin));
        Assert.True(Enum.IsDefined(typeof(UserRole), UserRole.User));
        Assert.True(Enum.IsDefined(typeof(UserRole), UserRole.ReadOnly));
    }

    [Fact]
    public void UserRole_AdminHasValue0()
    {
        // Assert
        Assert.Equal(0, (int)UserRole.Admin);
    }

    [Fact]
    public void UserRole_UserHasValue1()
    {
        // Assert
        Assert.Equal(1, (int)UserRole.User);
    }

    [Fact]
    public void UserRole_ReadOnlyHasValue2()
    {
        // Assert
        Assert.Equal(2, (int)UserRole.ReadOnly);
    }

    #endregion
}
