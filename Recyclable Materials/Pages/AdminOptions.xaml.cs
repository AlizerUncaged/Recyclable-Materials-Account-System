using System;
using System.Collections.Generic;
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

namespace Recyclable_Materials.Pages
{
    public partial class AdminOptions : UserControl, IPage
    {

        public AdminOptions()
        {
            InitializeComponent();
        }

        public event EventHandler<IPage> ChangePage;

        private void RegisterInstead(object sender, RoutedEventArgs e) =>
            ChangePage?.Invoke(this, new Register());

        private void ChangeContentPage(IPage page, MouseButtonEventArgs ev = null)
        {
            // Prevent window from getting dragged on click.
            if (!(ev is null)) ev.Handled = true;


            if (ActivePage.Children.Count >= 1)
            {
                // If the current page is already the same page, ignore changes.
                if (!(page is null) && ActivePage.Children[0].GetType().FullName == page.GetType().FullName)
                    return;
                ActivePage.Children.RemoveAt(0);
            }

            if (!(page is null))
            {
                Debug.WriteLine($"[adminoptions] page changed to {page.GetType().FullName}");
                page.ChangePage += RequestChangePage;
                ActivePage.Children.Add(page as UserControl);
            }
        }

        private void RequestChangePage(object sender, IPage e)
        {
            ChangeContentPage(e, null);
        }

        private void OpenMembers(object sender, MouseButtonEventArgs e) =>
            ChangeContentPage(new AdminPages.Members(), e);

        private void OpenTransactions(object sender, MouseButtonEventArgs e) =>
            ChangeContentPage(new AdminPages.Transactions(), e);

        private void OpenMaterials(object sender, MouseButtonEventArgs e) =>
            ChangeContentPage(new AdminPages.Materials(), e);

        private void OpenDashboard(object sender, MouseButtonEventArgs e) =>
            ChangeContentPage(null, e);

    }
}
