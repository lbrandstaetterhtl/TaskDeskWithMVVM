using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using TaskDesk_version2.Models;

namespace TaskDesk_version2.ViewModels;

public class ManageUsersWindowViewModel : INotifyPropertyChanged
{
    private User _originalUser;
    private string _fullname = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _roleString;
    private ObservableCollection<Task> _assignedTasks = new();
    private ObservableCollection<Group> _assignedGroups = new();
    private ObservableCollection<Task> _allTasks = MainData.Tasks;
    private ObservableCollection<Group> _allGroups = MainData.Groups;
    private ObservableCollection<User> _allUsers = MainData.Users;
    private User? _selectedUser;
    private string _searchInput = string.Empty;

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (_selectedUser != value)
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));

                if (value != null)
                {
                    OriginalUser = value;
                    UpdateData();
                }
            }
        }
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            if (_searchInput != value)
            {
                _searchInput = value;
                OnPropertyChanged(nameof(SearchInput));
            }
        }
    }

    public User OriginalUser
    {
        get => _originalUser;
        set
        {
            if (_originalUser != value)
            {
                _originalUser = value;
                OnPropertyChanged(nameof(OriginalUser));
            }
        }
    }

    public string Fullname
    {
        get => _fullname;
        set
        {
            if (_fullname != value)
            {
                _fullname = value;
                OnPropertyChanged(nameof(Fullname));
            }
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }

    public string RoleString
    {
        get => _roleString;
        set
        {
            if (_roleString != value)
            {
                _roleString = value;
                OnPropertyChanged(nameof(RoleString));
            }
        }
    }

    public ObservableCollection<Task> AssignedTasks
    {
        get => _assignedTasks;
        set
        {
            if (_assignedTasks != value)
            {
                _assignedTasks = value;
                OnPropertyChanged(nameof(AssignedTasks));
            }
        }
    }

    public ObservableCollection<Group> AssignedGroups
    {
        get => _assignedGroups;
        set
        {
            if (_assignedGroups != value)
            {
                _assignedGroups = value;
                OnPropertyChanged(nameof(AssignedGroups));
            }
        }
    }

    public ObservableCollection<Task> AllTasks
    {
        get => _allTasks;
        set
        {
            if (_allTasks != value)
            {
                _allTasks = value;
                OnPropertyChanged(nameof(AllTasks));
            }
        }
    }

    public ObservableCollection<User> AllUsers
    {
        get => _allUsers;
        set
        {
            if (_allUsers != value)
            {
                _allUsers = value;
                OnPropertyChanged(nameof(AllUsers));
            }
        }
    }

    public ObservableCollection<Group> AllGroups
    {
        get => _allGroups;
        set
        {
            if (_allGroups != value)
            {
                _allGroups = value;
                OnPropertyChanged(nameof(AllGroups));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ICommand saveCommand { get; }
    public ICommand cancelCommand { get; }
    public Action? RequestClose;

    public ManageUsersWindowViewModel(User user)
    {
        if (user == null)
        {
            return;
        }

        cancelCommand = new RelayCommand(() => RequestClose?.Invoke());
        saveCommand = new RelayCommand(SaveUser);

        OriginalUser = user;

        Fullname = user.FullName;
        Email = user.Email;
        Password = user.Password;
        RoleString = RoleConverter.RoleToString(user.Role);
        AssignedGroups = GroupsOperator.GetListFromIds(user.GroupIds, AllGroups);
        AssignedTasks = TasksOperator.GetListFromIds(user.TaskIds, AllTasks);
        SelectedUser = user;
    }

    public void UpdateData()
    {
        Fullname = SelectedUser!.FullName;
        Email = SelectedUser.Email;
        Password = SelectedUser.Password;
        RoleString = RoleConverter.RoleToString(SelectedUser.Role);
        AssignedGroups = GroupsOperator.GetListFromIds(SelectedUser.GroupIds, AllGroups);
        AssignedTasks = TasksOperator.GetListFromIds(SelectedUser.TaskIds, AllTasks);
        OriginalUser = SelectedUser;
    }

    private async void SaveUser()
    {
        try
        {
            var desktop = App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

            if (SelectedUser == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(Fullname) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RoleString))
            {
                var errorWindow = new Views.ErrorWindow("All fields must be filled out.");
                await errorWindow.ShowDialog(desktop.Windows[0]);

                return;
            }

            User updatedUser = new User
            {
                Id = SelectedUser.Id,
                FullName = Fullname,
                Email = Email,
                Password = Password,
                Role = RoleConverter.StringToRole(RoleString),
                GroupIds = GroupsOperator.GetIdsFromList(AssignedGroups, MainData.Groups),
                TaskIds = TasksOperator.GetIdsFromList(AssignedTasks, MainData.Tasks)
            };

            for (int i = 0; i < MainData.Users.Count; i++)
            {
                if (MainData.Users[i].Id == updatedUser.Id)
                {
                    MainData.Users.RemoveAt(i);
                    MainData.Users.Insert(i, updatedUser);
                    break;
                }
            }

            foreach (Group group in MainData.Groups)
            {
                if (updatedUser.GroupIds.Contains(group.Id))
                {
                    if (!group.UserIds.Contains(updatedUser.Id))
                    {
                        group.UserIds.Add(updatedUser.Id);
                    }
                }
                else
                {
                    if (group.UserIds.Contains(updatedUser.Id))
                    {
                        group.UserIds.Remove(updatedUser.Id);
                    }
                }
            }

            foreach (Task task in MainData.Tasks)
            {
                if (updatedUser.TaskIds.Contains(task.Id))
                {
                    if (!task.UserIds.Contains(updatedUser.Id))
                    {
                        task.UserIds.Add(updatedUser.Id);
                    }
                }
                else
                {
                    if (task.UserIds.Contains(updatedUser.Id))
                    {
                        task.UserIds.Remove(updatedUser.Id);
                    }
                }
            }

            MainData.Tasks[0] = MainData.Tasks[0];

            if (updatedUser != SelectedUser) AppLogger.Info("Changed user: ID: " + updatedUser.Id);

            RequestClose?.Invoke();
        }
        catch (Exception ex)
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            AppLogger.Error("Error saving user: " + ex.Message);

            var errorWindow = new Views.ErrorWindow($"An error occurred while saving the user: {ex.Message}");
            await errorWindow.ShowDialog(desktop.Windows[0]);
        }
    }

    public void SearchUpdate()
    {
        if (string.IsNullOrWhiteSpace(SearchInput))
        {
            AllUsers = MainData.Users;
        }
        else
        {
            var filteredUsers = new ObservableCollection<User>();
            foreach (var user in MainData.Users)
            {
                if (user.FullName.Contains(SearchInput, StringComparison.OrdinalIgnoreCase) ||
                    user.Email.Contains(SearchInput, StringComparison.OrdinalIgnoreCase) ||
                    user.Id.ToString().Contains(SearchInput, StringComparison.OrdinalIgnoreCase))
                {
                    filteredUsers.Add(user);
                }
            }

            if (filteredUsers.Count > 0)
            {
                AllUsers = filteredUsers;
            }
        }
    }

    public void ClearData()
    {
        Fullname = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        RoleString = string.Empty;
        AssignedGroups.Clear();
        AssignedTasks.Clear();
    }

    public void ClearSearch()
    {
        SearchInput = string.Empty;
    }
}