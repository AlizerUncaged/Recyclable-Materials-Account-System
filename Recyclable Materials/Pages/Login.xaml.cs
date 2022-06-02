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

namespace Recyclable_Materials.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl, IPage
    {

        public Login()
        {
            InitializeComponent();
        }

        public event EventHandler<IPage> ChangePage;

        private void RegisterInstead(object sender, RoutedEventArgs e) =>

            ChangePage?.Invoke(this, new Register());

        private void LoginClicked(object sender, RoutedEventArgs e)
        {
            if (Database.LocalDatabase.CheckAdministrator(emailField.Text, passwordField.Password))
                ChangePage?.Invoke(this, new AdminOptions());
            else
                InputAlerts.MessageQueue?.Enqueue("Invalid email or password.");
        }
    }
}
