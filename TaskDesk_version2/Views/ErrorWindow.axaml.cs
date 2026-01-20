using Avalonia.Controls;

namespace TaskDesk_version2.Views;

public partial class ErrorWindow : Window
{
    public ErrorWindow(string massage)
    {
        InitializeComponent();
        
        ErrorMassageBlock.Text = "🚫  " + massage;
        
        OkButton.Click += (_, _) => Close();
    }
}