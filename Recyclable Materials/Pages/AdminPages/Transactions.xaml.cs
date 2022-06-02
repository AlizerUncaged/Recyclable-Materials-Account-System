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

namespace Recyclable_Materials.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for Members.xaml
    /// </summary>
    public partial class Transactions : UserControl, IPage
    {

        public Transactions()
        {
            InitializeComponent();



        }

        public long AccountID
        {
            get
            {
                return long.Parse(TargetAccountID.Text);
            }
            set
            {
                TargetAccountID.Text = value.ToString();
            }
        }


        public event EventHandler<IPage> ChangePage;

        private async void AddTransaction(object sender, RoutedEventArgs e)
        {
            await DialogHostT.ShowDialog(DialogHostT.Content);
        }

        private void AddTransactionDT(object sender, RoutedEventArgs e)
        {
            DialogHostT.IsOpen = false;
        }
    }
}
