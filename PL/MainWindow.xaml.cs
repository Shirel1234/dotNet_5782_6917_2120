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
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL bl;
        public MainWindow(BlApi.IBL bll)
        {
            InitializeComponent();
            bl = bll;
        }
        private void btnShowListDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).ShowDialog();
        }
        private void btnShowListStations_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationsListWindow(bl).ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ListParcels(bl).ShowDialog();
        }
    }
}