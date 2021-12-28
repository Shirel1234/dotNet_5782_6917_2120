using BO;
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
            lsvStations.ItemsSource = bll.GetBaseStations();
            //DataContext = bll.GetBaseStations();
        }

        private void btnAddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            new AddBaseStationWindow(bll).ShowDialog();
            lsvStations.ItemsSource = bll.GetBaseStations();
        }

        private void GroupingByAvailableChargingSlots(object sender, RoutedEventArgs e)
        {
            //DataContext= bll.GetParcels();
            lsvStations.ItemsSource = bll.GetBaseStations();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvStations.ItemsSource);
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ChargeSlotsFree");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void ShowThisStation(object sender, MouseButtonEventArgs e)
        {
            new AddBaseStationWindow(bll,(BaseStationForList)lsvStations.SelectedItem).ShowDialog();
            lsvStations.ItemsSource = bll.GetBaseStations();
        }

        private void btnShowStatinsWithChargeSlots_Click(object sender, RoutedEventArgs e)
        {
            lsvStations.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            lsvStations.ItemsSource = bll.GetBaseStations();
            rdbByAvailableChargingSlots.IsChecked = false;
        }
    }
}
