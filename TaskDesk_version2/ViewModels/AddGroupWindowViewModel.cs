using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls.Primitives;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.ViewModels;

public class AddGroupWindowViewModel : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _description = string.Empty;
    private List<string> _userNames = new();
    
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
    
    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }
    
    public List<string> UserNames
    {
        get => _userNames;
        set
        {
            if (_userNames != value)
            {
                _userNames = value;
                OnPropertyChanged(nameof(UserNames));
            }
        }
    }
    
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public Action? RequestClose;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public AddGroupWindowViewModel()
    {
        SaveCommand = new RelayCommand(SaveGroup);
        CancelCommand = new RelayCommand(() => RequestClose?.Invoke());
    }

    private async void SaveGroup()
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        
        if (Name == string.Empty && Description == string.Empty)
        {
            var errorWindow = new Views.ErrorWindow("Group must have a name and description.");
            await errorWindow.ShowDialog(desktop.MainWindow!);
            return;
        }
        
        var userIds = UsersOperator.GetIdsFromNames(UserNames, MainData.Users);
        
        var newGroup = new Group(Name, Description, userIds);
        
        await Dispatcher.UIThread.InvokeAsync(() => MainData.Groups.Add(newGroup));
        
        RequestClose?.Invoke();
    }
}