using TaskDesk_version2.Models;
using Xunit;
using System.Collections.Generic;

namespace TaskDesk_version2.Tests.Tests.Models;

/// <summary>
/// Unit-Tests für TaskState Enum und StateConverter Klasse
/// </summary>
public class TaskStateTests
{
    #region StateToString Tests

    [Fact]
    public void StateToString_PendingState_ReturnsPendingString()
    {
        // Arrange
        var state = TaskState.Pending;

        // Act
        var result = StateConverter.StateToString(state);

        // Assert
        Assert.Equal("Pending...", result);
    }

    [Fact]
    public void StateToString_InProgressState_ReturnsInProgressString()
    {
        // Arrange
        var state = TaskState.InProgress;

        // Act
        var result = StateConverter.StateToString(state);

        // Assert
        Assert.Equal("In Progress...", result);
    }

    [Fact]
    public void StateToString_CompletedState_ReturnsCompletedString()
    {
        // Arrange
        var state = TaskState.Completed;

        // Act
        var result = StateConverter.StateToString(state);

        // Assert
        Assert.Equal("Completed!", result);
    }

    [Fact]
    public void StateToString_OnHoldState_ReturnsOnHoldString()
    {
        // Arrange
        var state = TaskState.OnHold;

        // Act
        var result = StateConverter.StateToString(state);

        // Assert
        Assert.Equal("On Hold...", result);
    }

    [Fact]
    public void StateToString_CancelledState_ReturnsCancelledString()
    {
        // Arrange
        var state = TaskState.Cancelled;

        // Act
        var result = StateConverter.StateToString(state);

        // Assert
        Assert.Equal("Cancelled!", result);
    }

    [Theory]
    [InlineData(TaskState.Pending, "Pending...")]
    [InlineData(TaskState.InProgress, "In Progress...")]
    [InlineData(TaskState.Completed, "Completed!")]
    [InlineData(TaskState.OnHold, "On Hold...")]
    [InlineData(TaskState.Cancelled, "Cancelled!")]
    public void StateToString_AllValidStates_ReturnsCorrectStrings(TaskState state, string expected)
    {
        // Act
        var result = StateConverter.StateToString(state);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region StringToState Tests

    [Fact]
    public void StringToState_PendingString_ReturnsPendingState()
    {
        // Arrange
        var stateString = "Pending...";

        // Act
        var result = StateConverter.StringToState(stateString);

        // Assert
        Assert.Equal(TaskState.Pending, result);
    }

    [Fact]
    public void StringToState_InProgressString_ReturnsInProgressState()
    {
        // Arrange
        var stateString = "In Progress...";

        // Act
        var result = StateConverter.StringToState(stateString);

        // Assert
        Assert.Equal(TaskState.InProgress, result);
    }

    [Fact]
    public void StringToState_CompletedString_ReturnsCompletedState()
    {
        // Arrange
        var stateString = "Completed!";

        // Act
        var result = StateConverter.StringToState(stateString);

        // Assert
        Assert.Equal(TaskState.Completed, result);
    }

    [Fact]
    public void StringToState_OnHoldString_ReturnsOnHoldState()
    {
        // Arrange
        var stateString = "On Hold...";

        // Act
        var result = StateConverter.StringToState(stateString);

        // Assert
        Assert.Equal(TaskState.OnHold, result);
    }

    [Fact]
    public void StringToState_CancelledString_ReturnsCancelledState()
    {
        // Arrange
        var stateString = "Cancelled!";

        // Act
        var result = StateConverter.StringToState(stateString);

        // Assert
        Assert.Equal(TaskState.Cancelled, result);
    }

    [Theory]
    [InlineData("Pending...", TaskState.Pending)]
    [InlineData("In Progress...", TaskState.InProgress)]
    [InlineData("Completed!", TaskState.Completed)]
    [InlineData("On Hold...", TaskState.OnHold)]
    [InlineData("Cancelled!", TaskState.Cancelled)]
    public void StringToState_AllValidStrings_ReturnsCorrectStates(string stateString, TaskState expected)
    {
        // Act
        var result = StateConverter.StringToState(stateString);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void StringToState_InvalidString_ThrowsKeyNotFoundException()
    {
        // Arrange
        var invalidString = "InvalidState";

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() => StateConverter.StringToState(invalidString));
        Assert.Contains("InvalidState", exception.Message);
    }

    [Fact]
    public void StringToState_EmptyString_ThrowsKeyNotFoundException()
    {
        // Arrange
        var emptyString = "";

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => StateConverter.StringToState(emptyString));
    }

    [Fact]
    public void StringToState_NullString_ThrowsKeyNotFoundException()
    {
        // Arrange
        string? nullString = null;

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => StateConverter.StringToState(nullString!));
    }

    [Theory]
    [InlineData("pending...")]  // Kleinbuchstaben
    [InlineData("PENDING...")]  // Großbuchstaben
    [InlineData(" Pending... ")] // Mit Leerzeichen
    [InlineData("Pending")]     // Ohne Auslassungszeichen
    public void StringToState_InvalidFormats_ThrowsKeyNotFoundException(string invalidFormat)
    {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => StateConverter.StringToState(invalidFormat));
    }

    #endregion

    #region Round-Trip Tests (Bidirektionale Konvertierung)

    [Theory]
    [InlineData(TaskState.Pending)]
    [InlineData(TaskState.InProgress)]
    [InlineData(TaskState.Completed)]
    [InlineData(TaskState.OnHold)]
    [InlineData(TaskState.Cancelled)]
    public void StateConversion_RoundTrip_ReturnsOriginalState(TaskState originalState)
    {
        // Act
        var stringRepresentation = StateConverter.StateToString(originalState);
        var convertedBack = StateConverter.StringToState(stringRepresentation);

        // Assert
        Assert.Equal(originalState, convertedBack);
    }

    #endregion

    #region Enum Values Tests

    [Fact]
    public void TaskState_HasExpectedNumberOfValues()
    {
        // Act
        var values = Enum.GetValues(typeof(TaskState));

        // Assert
        Assert.Equal(5, values.Length);
    }

    [Fact]
    public void TaskState_ContainsAllExpectedValues()
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(TaskState), TaskState.Pending));
        Assert.True(Enum.IsDefined(typeof(TaskState), TaskState.InProgress));
        Assert.True(Enum.IsDefined(typeof(TaskState), TaskState.Completed));
        Assert.True(Enum.IsDefined(typeof(TaskState), TaskState.OnHold));
        Assert.True(Enum.IsDefined(typeof(TaskState), TaskState.Cancelled));
    }

    #endregion
}
