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
    public partial class Materials : UserControl, IPage
    {

        public Materials()
        {
            InitializeComponent();

            var src = new ObservableCollection<Models.Material> {
               new Models.Material(1, "Bottle", 900),
               new Models.Material(2, "Paper", 900),
               new Models.Material(3, "Plant Waste", 900){ Biodegradable = true},
               new Models.Material(4, "Plastic", 900),
            };

            MaterialsTable.ItemsSource = src;

        }

        public event EventHandler<IPage> ChangePage;

    }
}
