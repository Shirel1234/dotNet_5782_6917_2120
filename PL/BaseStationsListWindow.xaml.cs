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
    /// Interaction logic for BaseStationsListWindow.xaml
    /// </summary>
    public partial class BaseStationsListWindow : Window
    {
        BlApi.IBL bll;
       
        public BaseStationsListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bll = bl;
            //lstStationsListView.ItemsSource = bll.GetBaseStations();
            DataContext = bll.GetBaseStations();
            //lstStationsListView.ItemsSource;
        }

        private void btnAddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            new AddBaseStationWindow(bll).ShowDialog();
        }
    }
}
