using System;
using Avalonia.Controls;

namespace TaskDesk_version2.Views;

public partial class ErrorWindow : Window
{
    private readonly string _errorMassage;
    private readonly string _logMassage;

    public ErrorWindow(string massage, string logMassage = "")
    {
        InitializeComponent();

        _logMassage = logMassage;
        _errorMassage = massage;

        ErrorMassageBlock.Text = "🚫  " + massage;

        OkButton.Click += (_, _) => Close();

        Closing += OnClosing;
        Opened += OnOpened;
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        AppLogger.Info("------------- Error Window Opened -------------");
        if (!string.IsNullOrEmpty(_logMassage)) AppLogger.Warn(_logMassage + " | Error massage: " + _errorMassage);
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        AppLogger.Info("------------- Error Window Closed -------------");
    }
}