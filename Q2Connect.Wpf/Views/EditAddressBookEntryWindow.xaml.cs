using System.Windows;
using Q2Connect.Core.Models;

namespace Q2Connect.Wpf.Views;

public partial class EditAddressBookEntryWindow : Window
{
    public AddressBookEntry Entry { get; }

    public EditAddressBookEntryWindow(AddressBookEntry entry)
    {
        InitializeComponent();
        Entry = new AddressBookEntry
        {
            Address = entry.Address,
            Label = entry.Label
        };
        DataContext = Entry;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Entry.Address))
        {
            MessageBox.Show(
                "Address cannot be empty.",
                "Validation Error",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
        Close();
    }
}

