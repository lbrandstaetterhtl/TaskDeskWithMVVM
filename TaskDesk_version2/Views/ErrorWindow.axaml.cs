using System;
using Avalonia.Controls;
using Splat;

namespace TaskDesk_version2.Views;

public partial class ErrorWindow : Window
{
    private readonly string _logMassage;
    private readonly string _errorMassage;
    
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