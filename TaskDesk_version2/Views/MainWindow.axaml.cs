using Avalonia.Controls;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = new MainWindowViewModel();
        
        AddTaskMenuItem.Click += MainWindowViewModel.OnAddTaskClick;
    }
}