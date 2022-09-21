using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
using Recyclable_Materials.Utilities;

namespace Recyclable_Materials.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Register : UserControl, IPage
    {
        public Register() =>
            InitializeComponent();


        public event EventHandler<IPage> ChangePage;

        /// <summary>
        /// Gets called whenever the login button is clicked.
        /// </summary>
        private void LoginInstead(object sender, RoutedEventArgs e) =>
            ChangePage?.Invoke(this, new Login());


        /// <summary>
        /// Gets called whenever the register button is clicked.
        /// </summary>
        private async void RegisterClicked(object sender, RoutedEventArgs e)
        {

            Database.LocalDatabase.InsertAdministrators(email.Text, username.Text,  password.Password);

            await DialogHost.ShowDialog(DialogHost.Content);
        }
    }
}
