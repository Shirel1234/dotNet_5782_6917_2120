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
            try
            {
                lsvStations.ItemsSource = bll.GetBaseStations();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error");
            }
            //DataContext = bll.GetBaseStations();
        }

        private void btnAddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            new AddBaseStationWindow(bll).ShowDialog();
            lsvStations.ItemsSource = bll.GetBaseStations();
            rdbByAvailableChargingSlots.IsChecked = false;
        }

        private void GroupingByAvailableChargingSlots(object sender, RoutedEventArgs e)
        {
            //DataContext= bll.GetParcels();
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);

            lsvStations.ItemsSource = bll.GetBaseStations();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvStations.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ChargeSlotsFree");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void ShowThisStation(object sender, MouseButtonEventArgs e)
        {
            new AddBaseStationWindow(bll,(BaseStationForList)lsvStations.SelectedItem).ShowDialog();
            lsvStations.ItemsSource = bll.GetBaseStations();
            rdbByAvailableChargingSlots.IsChecked = false;
        }

        private void btnShowStatinsWithChargeSlots_Click(object sender, RoutedEventArgs e)
        {
            lsvStations.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
            rdbByAvailableChargingSlots.IsChecked = false;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            lsvStations.ItemsSource = bll.GetBaseStations();
            rdbByAvailableChargingSlots.IsChecked = false;
        }
    }
}
