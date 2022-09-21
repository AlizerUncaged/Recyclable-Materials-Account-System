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

        private int wrongTries = 0;

        private void RegisterInstead(object sender, RoutedEventArgs e) =>
            ChangePage?.Invoke(this, new Register());

        private void LoginClicked(object sender, RoutedEventArgs e)
        {
            if (Database.LocalDatabase.CheckAdministrator(emailField.Text, passwordField.Password))
                ChangePage?.Invoke(this, new AdminOptions());

            if (wrongTries >= 3)
            {
                InputAlerts.MessageQueue?.Clear();
                InputAlerts.MessageQueue?.Enqueue("You have entered wrong passwords, please try again in 3 minutes.");
                loginButton.IsEnabled = false;
                Task.Run(async () =>
                {
                    for (int i = 0; i < 3 * 60; i++)
                    {
                        loginButton.Dispatcher.Invoke(new Action(() =>
                        {
                            loginButton.Content = $"Login ({Math.Abs(i - (3 * 60))}s)";
                        }));
                        await Task.Delay(1000); // Wait 3 minutes.
                    }

                    loginButton.Dispatcher.Invoke(new Action(() => { loginButton.IsEnabled = true; }));
                });

                return;
            }

            InputAlerts.MessageQueue?.Enqueue("Invalid email or password.");
            wrongTries++;
        }
    }
}