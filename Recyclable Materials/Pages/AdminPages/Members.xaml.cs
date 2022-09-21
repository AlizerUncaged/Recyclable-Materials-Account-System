using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Recyclable_Materials.Models;
using Recyclable_Materials.Utilities;

namespace Recyclable_Materials.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for Members.xaml
    /// </summary>
    public partial class Members : UserControl, IPage
    {
        public Members()
        {
            InitializeComponent();

            RefreshMembers();
        }

        private void ResetFields()
        {
            FirstName.Text = "Select";
            LastName.Text = "a member.";


            Email.Text = string.Empty;
            Address.Text = string.Empty;
            Points.Text = string.Empty;
        }

        private void RefreshMembers(string query = null)
        {
            ResetFields();

            if (query is null)
                MembersTable.ItemsSource =
                    Database.LocalDatabase.SelectMembers();
            else
                MembersTable.ItemsSource =
                    new Models.Member().Search(Database.LocalDatabase.SelectMembers(), query);
        }

        public event EventHandler<IPage> ChangePage;

        private void TableSelectChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            // We can use bindings for these but that would take a while to set up.
            if (MembersTable.SelectedItem is Models.Member selectedMember)
            {
                Debug.WriteLine(
                    $"[members] selected table item changed selected first name: {selectedMember.FirstName}");

                FirstName.Text = selectedMember.FirstName;
                LastName.Text = selectedMember.LastName;
                Email.Text = selectedMember.Email;
                Address.Text = selectedMember.Address;
                Points.Text = $"{selectedMember.Points}";

                ViewTransactionsButton.Visibility = Visibility.Visible;
            }
            else
                ViewTransactionsButton.Visibility = Visibility.Collapsed;
        }

        private void ViewMemberTransactions(object sender, RoutedEventArgs e)
        {
            Transactions transactions = new Transactions();

            if (MembersTable.SelectedItem is Models.Member selectedMember)
                transactions.AccountID = selectedMember.Id;

            ChangePage?.Invoke(this, transactions);
        }

        private async void AddMember(object sender, RoutedEventArgs e)
        {
            DatabaseCommand.Content = "Add Member";
            await DialogHostT.ShowDialog(DialogHostT.Content);
        }


        private void AddMemberDHost(object sender, RoutedEventArgs e)
        {
            // Change this into something more optimized, TODO.
            if ((string)DatabaseCommand.Content == "Add Member")
            {
                var member = new Member()
                {
                    Address = address.Text, Email = email.Text, FirstName = fname.Text, LastName = lname.Text
                };

              //  member.Update(new[] { member });
            }
            else if ((string)DatabaseCommand.Content == "Update Member" &&
                     MembersTable.SelectedItem is Models.Member selectedMember)
            {
                var member = new Member()
                {
                    Address = address.Text, Email = email.Text, FirstName = fname.Text, LastName = lname.Text,
                    Id = selectedMember.Id
                };
                
              //  member.Update(new[] { member });
            }

            RefreshMembers();
            CancelDHost(sender, e);
        }


        private void CancelDHost(object sender, RoutedEventArgs e)
        {
            DialogHostT.IsOpen = false;
        }

        private void DeleteMember(object sender, RoutedEventArgs e)
        {
            if (MembersTable.SelectedItem is Models.Member selectedMember)
            {
                Debug.WriteLine($"[members] deleting: {selectedMember.FirstName} with id: {selectedMember.Id}");
              //  selectedMember.Delete(selectedMember);
            }

            RefreshMembers();
        }

        private void SearchMembers(object sender, RoutedEventArgs e) =>
            RefreshMembers(SearchQuery.Text);

        private async void UpdateMember(object sender, RoutedEventArgs e)
        {
            DatabaseCommand.Content = "Update Member";
            await DialogHostT.ShowDialog(DialogHostT.Content);
        }
    }
}