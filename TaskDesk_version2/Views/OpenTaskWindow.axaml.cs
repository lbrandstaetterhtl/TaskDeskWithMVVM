using Avalonia.Controls;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.Views;

public partial class OpenTaskWindow : Window
{
    public OpenTaskWindow(Task task)
    {
        InitializeComponent();

        Title += " - " + task.Id;
        
        TitleBlock.Text = task.Title;
        
        DescriptionBlock.Text = task.Description;

        DueDateBlock.Text = task.DateAsString;
        
        StateBlock.Text = task.StateAsString;
        
        GroupsBlock.Text = task.GroupsAsString;
        
        UsersBlock.Text = task.UsersAsString;
    }
}