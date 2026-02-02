using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();

        new MainData();
        
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