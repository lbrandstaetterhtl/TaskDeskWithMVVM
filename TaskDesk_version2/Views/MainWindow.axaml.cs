using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Threading;
using TaskDesk_version2.Models;
using TaskDesk_version2.ViewModels;

namespace TaskDesk_version2.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var vm = new MainWindowViewModel();
        
        DataContext = vm;
        
        AddTaskMenuItem.Click += MainWindowViewModel.OnAddTaskClick;

        DebugAddButton.Click += (_, _) =>
        {
            var vm2 = DataContext as MainWindowViewModel;

            Console.WriteLine("---- DebugAdd START ----");
            Console.WriteLine($"MainData.Tasks reference: {MainData.Tasks.GetHashCode()}");
            Console.WriteLine($"MainData.Tasks count (before): {MainData.Tasks.Count}");
            Console.WriteLine($"VM.Tasks reference: {vm2?.Tasks.GetHashCode()}");
            Console.WriteLine($"VM.Tasks count (before): {vm2?.Tasks.Count}");
            Console.WriteLine($"ReferenceEquals(MainData.Tasks, vm.Tasks): {ReferenceEquals(MainData.Tasks, vm2?.Tasks)}");

            var debugTask = new Task(MainData.Tasks.Count, "DBG Task", "Debug entry", DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>());

            try
            {
                vm2?.Tasks.Add(debugTask);
                Console.WriteLine("Added debug task via vm.Tasks.Add (if vm != null).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception beim vm.Tasks.Add: {ex}");
            }

            try
            {
                MainData.Tasks.Add(new Task(MainData.Tasks.Count, "DBG Task 2", "Debug entry 2", DateOnly.FromDateTime(DateTime.Now), TaskState.Pending, new List<int>()));
                Console.WriteLine("Added debug task via MainData.Tasks.Add.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception beim MainData.Tasks.Add: {ex}");
            }

            Console.WriteLine($"MainData.Tasks count (after): {MainData.Tasks.Count}");
            Console.WriteLine($"VM.Tasks count (after): {vm2?.Tasks.Count}");
            Console.WriteLine("---- DebugAdd END ----");
        };
    }
}