using System.Collections.Generic;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.ViewModels;

public class EditYourUserWindowViewModel
{
    // TODO: Implement EditYourUserWindowViewModel, which allows users to edit their own profile information. This ViewModel should have properties for the user's name, email, and password, as well as commands for saving changes and canceling edits. The ViewModel should also include an Action to request closing the window when necessary.
    
    private string _FullName;
    private string _Email;
    private string _Password;
    private List<string> _RoleOptions => RoleConverter.GetAllRoleStrings();
}