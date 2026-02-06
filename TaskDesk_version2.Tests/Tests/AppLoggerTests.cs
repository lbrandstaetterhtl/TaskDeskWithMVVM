using TaskDesk_version2;
using Xunit;
using System;
using System.IO;

namespace TaskDesk_version2.Tests.Tests;

/// <summary>
/// Unit-Tests für die AppLogger-Klasse
/// </summary>
public class AppLoggerTests
{
    #region GetLogFilePath Tests

    [Fact]
    public void GetLogFilePath_ReturnsValidPath()
    {
        // Act
        var path = AppLogger.GetLogFilePath();

        // Assert
        Assert.NotNull(path);
        Assert.NotEmpty(path);
    }

    [Fact]
    public void GetLogFilePath_ContainsLogExtension()
    {
        // Act
        var path = AppLogger.GetLogFilePath();

        // Assert
        Assert.EndsWith(".taskdesk.log", path);
    }

    [Fact]
    public void GetLogFilePath_ContainsLogsDirectory()
    {
        // Act
        var path = AppLogger.GetLogFilePath();

        // Assert
        Assert.Contains("logs", path);
    }

    [Fact]
    public void GetLogFilePath_ContainsDatePattern()
    {
        // Act
        var path = AppLogger.GetLogFilePath();
        var filename = Path.GetFileName(path);

        // Assert
        // Dateiname enthält Jahr-Monat-Tag Muster
        Assert.Matches(@"\d{4}-\d{2}-\d{2}", filename);
    }

    [Fact]
    public void GetLogFilePath_ContainsTimePattern()
    {
        // Act
        var path = AppLogger.GetLogFilePath();
        var filename = Path.GetFileName(path);

        // Assert
        // Dateiname enthält Stunden-Minuten-Sekunden Muster
        Assert.Matches(@"\d{2}-\d{2}-\d{2}", filename);
    }

    [Fact]
    public void GetLogFilePath_ReturnsConsistentPath()
    {
        // Act
        var path1 = AppLogger.GetLogFilePath();
        var path2 = AppLogger.GetLogFilePath();

        // Assert
        Assert.Equal(path1, path2);
    }

    #endregion

    #region Method Signature Tests

    [Fact]
    public void Info_AcceptsStringMessage()
    {
        // Arrange
        var message = "Test info message";

        // Act & Assert - sollte keine Exception werfen
        var exception = Record.Exception(() => AppLogger.Info(message));
        
        // Da wir im Design-Mode sind oder die Methode fehlertolerant ist,
        // sollte keine Exception geworfen werden
        Assert.Null(exception);
    }

    [Fact]
    public void Warn_AcceptsStringMessage()
    {
        // Arrange
        var message = "Test warning message";

        // Act & Assert
        var exception = Record.Exception(() => AppLogger.Warn(message));
        Assert.Null(exception);
    }

    [Fact]
    public void Error_AcceptsStringMessage()
    {
        // Arrange
        var message = "Test error message";

        // Act & Assert
        var exception = Record.Exception(() => AppLogger.Error(message));
        Assert.Null(exception);
    }

    [Fact]
    public void Error_AcceptsMessageWithException()
    {
        // Arrange
        var message = "Test error message";
        var ex = new InvalidOperationException("Test exception");

        // Act & Assert
        var recordedException = Record.Exception(() => AppLogger.Error(message, ex));
        Assert.Null(recordedException);
    }

    [Fact]
    public void Error_AcceptsNullException()
    {
        // Arrange
        var message = "Test error message";

        // Act & Assert
        var exception = Record.Exception(() => AppLogger.Error(message, null));
        Assert.Null(exception);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Info_AcceptsEmptyString()
    {
        // Act & Assert
        var exception = Record.Exception(() => AppLogger.Info(""));
        Assert.Null(exception);
    }

    [Fact]
    public void Warn_AcceptsEmptyString()
    {
        // Act & Assert
        var exception = Record.Exception(() => AppLogger.Warn(""));
        Assert.Null(exception);
    }

    [Fact]
    public void Error_AcceptsEmptyString()
    {
        // Act & Assert
        var exception = Record.Exception(() => AppLogger.Error(""));
        Assert.Null(exception);
    }

    [Fact]
    public void Info_AcceptsSpecialCharacters()
    {
        // Arrange
        var message = "Test: <tag> & \"quotes\" äöü ß 日本語";

        // Act & Assert
        var exception = Record.Exception(() => AppLogger.Info(message));
        Assert.Null(exception);
    }

    [Fact]
    public void Info_AcceptsVeryLongMessage()
    {
        // Arrange
        var message = new string('A', 10000);

        // Act & Assert
        var exception = Record.Exception(() => AppLogger.Info(message));
        Assert.Null(exception);
    }

    [Fact]
    public void Info_AcceptsMultilineMessage()
    {
        // Arrange
        var message = "Line 1\nLine 2\nLine 3";

        // Act & Assert
        var exception = Record.Exception(() => AppLogger.Info(message));
        Assert.Null(exception);
    }

    [Fact]
    public void Error_WithNestedExceptions_HandlesCorrectly()
    {
        // Arrange
        var innerException = new ArgumentException("Inner exception");
        var outerException = new InvalidOperationException("Outer exception", innerException);

        // Act & Assert
        var recordedException = Record.Exception(() => AppLogger.Error("Test", outerException));
        Assert.Null(recordedException);
    }

    #endregion

    #region Thread Safety Tests (Basic)

    [Fact]
    public void Logger_MultipleCallsInSequence_DoNotThrow()
    {
        // Act & Assert
        var exception = Record.Exception(() =>
        {
            for (int i = 0; i < 100; i++)
            {
                AppLogger.Info($"Info message {i}");
                AppLogger.Warn($"Warn message {i}");
                AppLogger.Error($"Error message {i}");
            }
        });

        Assert.Null(exception);
    }

    #endregion
}
