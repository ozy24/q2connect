using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Q2Connect.Wpf.ViewModels;

namespace Q2Connect.Wpf.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        if (DataContext is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // Don't connect when double-clicking on column headers
        if (e.OriginalSource is DependencyObject dep && FindAncestor<DataGridColumnHeader>(dep) != null)
            return;
        // Don't connect when double-clicking on a checkbox
        if (e.OriginalSource is FrameworkElement element && (element is CheckBox || element.Parent is CheckBox))
            return;

        if (DataContext is MainViewModel viewModel && viewModel.SelectedServer != null)
        {
            viewModel.ConnectCommand.Execute(null);
        }
    }

    private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T found)
                return found;
            current = VisualTreeHelper.GetParent(current);
        }
        return null;
    }

    private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Prevent double-click from triggering when clicking checkbox
        if (e.OriginalSource is FrameworkElement element)
        {
            if (element is CheckBox || element.Parent is CheckBox)
            {
                e.Handled = false; // Allow checkbox to handle the event
            }
        }
    }

    private void AddressBookDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainViewModel viewModel && viewModel.AddressBookViewModel?.SelectedEntry != null)
        {
            viewModel.AddressBookViewModel.ConnectCommand.Execute(null);
        }
    }

    private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if ((e.Key == Key.Enter || e.Key == Key.Return) && DataContext is MainViewModel viewModel && viewModel.SelectedServer != null)
        {
            viewModel.ConnectCommand.Execute(null);
            e.Handled = true;
        }
    }

    private void DataGrid_ServerSorting(object sender, DataGridSortingEventArgs e)
    {
        if (DataContext is not MainViewModel viewModel)
            return;
        var column = e.Column;
        var propertyName = column.SortMemberPath;
        if (string.IsNullOrEmpty(propertyName) && column is DataGridBoundColumn boundColumn && boundColumn.Binding is Binding binding && binding.Path != null)
            propertyName = binding.Path.Path;
        if (string.IsNullOrEmpty(propertyName))
            propertyName = "CurrentPlayers";
        // When Handled = true the DataGrid does not set SortDirection, so compute and set it ourselves
        var direction = column.SortDirection != ListSortDirection.Ascending
            ? ListSortDirection.Ascending
            : ListSortDirection.Descending;
        column.SortDirection = direction;
        viewModel.ApplyServerSort(propertyName, direction);
        e.Handled = true;
    }

    private void TabControl_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Tab && DataContext is MainViewModel viewModel)
        {
            // Cycle between tabs: 0 = Public Servers, 1 = Address Book
            viewModel.SelectedTabIndex = viewModel.SelectedTabIndex == 0 ? 1 : 0;
            e.Handled = true;
        }
    }
}

