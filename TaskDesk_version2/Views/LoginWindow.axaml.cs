using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace TaskDesk_version2.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        
        var vm = new ViewModels.LoginWindowViewModel();
        vm.RequestClose += Close;
        vm.PasswordVisibleCommand = new RelayCommand(PasswordVisibility);
        DataContext = vm;
    }

    private void PasswordVisibility()
    {
        if (PasswordBox.PasswordChar == '*')
        {
            PasswordBox.PasswordChar = '\0';
            return;
        }
        
        PasswordBox.PasswordChar = '*';
    }
}