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
        #region constructor
        public BaseStationsListWindow(BlApi.IBL bl)
        {
            try
            {
                InitializeComponent();
                bll = bl;
                lsvStations.ItemsSource = bll.GetBaseStations();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
        #region buttons and clicks events
        private void btnAddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new AddBaseStationWindow(bll).ShowDialog();
                lsvStations.ItemsSource = bll.GetBaseStations();
                rdbByAvailableChargingSlots.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void GroupingByAvailableChargingSlots(object sender, RoutedEventArgs e)
        {
            try
            {
                lsvStations.ItemsSource = bll.GetBaseStations();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvStations.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("ChargeSlotsFree");
                view.GroupDescriptions.Add(groupDescription);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void ShowThisStation(object sender, MouseButtonEventArgs e)
        {
            try
            {
                new AddBaseStationWindow(bll, (BaseStationForList)lsvStations.SelectedItem).ShowDialog();
                lsvStations.ItemsSource = bll.GetBaseStations();
                rdbByAvailableChargingSlots.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void btnShowStatinsWithChargeSlots_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lsvStations.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
                rdbByAvailableChargingSlots.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lsvStations.ItemsSource = bll.GetBaseStations();
                rdbByAvailableChargingSlots.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
    }
}
