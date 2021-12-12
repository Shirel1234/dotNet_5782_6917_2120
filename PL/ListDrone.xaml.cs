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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ListDrone.xaml
    /// </summary>
    public partial class ListDrone : Window
    {
        public ListDrone()
        {
            InitializeComponent();

        }
        private void ListDrone_Load(object sender, EventArgs e)
        {
            //dgrDrones.Items = DllNotFoundException..Table;
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
