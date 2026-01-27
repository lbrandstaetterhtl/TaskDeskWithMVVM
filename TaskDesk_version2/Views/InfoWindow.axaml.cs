using System.Threading.Tasks;
using Avalonia.Controls;

namespace TaskDesk_version2.Views;

public partial class InfoWindow : Window
{
    private readonly TaskCompletionSource<bool>? _taskCompletionSource;
    
    public InfoWindow(string info, bool twoButtons = false)
    {
        InitializeComponent();
        
        InfoMassageBlock.Text = "ℹ️  " + info;
        
        if (twoButtons)
        {
            OkButton.Content = "Yes";

            CancelButton.IsVisible = true;
            CancelButton.Content = "No";
            
            _taskCompletionSource = new TaskCompletionSource<bool>();

            OkButton.Click += (_, _) =>
            {
                _taskCompletionSource.SetResult(true);
                Close();
            };
            
            CancelButton.Click += (_, _) =>
            {
                _taskCompletionSource.SetResult(false);
                Close();
            };
        }
        else
        {
            OkButton.Click += (_, _) => Close();
        }
    }

    public Task<bool> ShowDialogAsync(Window owner)
    {
        ShowDialog(owner);
        return _taskCompletionSource!.Task;
    }
}