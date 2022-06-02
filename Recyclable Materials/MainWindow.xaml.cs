using Recyclable_Materials.Pages;
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
using System.Diagnostics;

namespace Recyclable_Materials
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Set the current page as login.
            InitializePage(new Pages.Login());

            // Connect to the database.
            if (!Database.LocalDatabase.Connect() & !Database.LocalDatabase.InitializeTables())
                InputAlerts.MessageQueue?.Enqueue("Error connecting to database, please check debug logs.");

        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        /// <summary>
        /// Gets called whenever the current page requests a page change.
        /// </summary>
        private void PageChanged(object sender, IPage e) => InitializePage(e);

        /// <summary>
        /// Sets the current page to the target page.
        /// </summary>
        private void InitializePage(IPage e)
        {
            Debug.WriteLine($"[mainwindow] page changed to {e.GetType().FullName}");

            // Remove any page inside the Parent grid if it exists.
            if (Parent.Children.Count >= 1)
                Parent.Children.RemoveAt(0);

            e.ChangePage += PageChanged;
            Parent.Children.Add(e as UIElement);
        }

        private void CloseClicked(object sender, MouseButtonEventArgs e) =>
           this.Close();

        private void MinimizeClicked(object sender, MouseButtonEventArgs e) =>
            this.WindowState = WindowState.Minimized;
    }
}
