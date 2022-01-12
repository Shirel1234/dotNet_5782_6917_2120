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
        /// <summary>
        /// open the window of adding base station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new AddBaseStationWindow(bll).ShowDialog();
                //when you back to this window, check which viewing to show
                if (rdbByAvailableChargingSlots.IsChecked == true)
                    this.GroupingByAvailableChargingSlots(sender, e);
                else
                    if (lsvStations.ItemsSource == bll.GetAllBaseStationsWithChargePositions())
                        lsvStations.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
                    else
                        lsvStations.ItemsSource = bll.GetBaseStations();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// view of the list with grouping by number of available charging slots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// enable to open the station window from the stations list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowThisStation(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //only if there are items in the list
                if(!lsvStations.Items.IsEmpty)
                    new AddBaseStationWindow(bll, (BaseStationForList)lsvStations.SelectedItem).ShowDialog();
                //when you back to this window, show the updated last view
                if (rdbByAvailableChargingSlots.IsChecked == true)
                    this.GroupingByAvailableChargingSlots(sender, e);
                else
                    if (lsvStations.ItemsSource == bll.GetAllBaseStationsWithChargePositions())
                         lsvStations.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
                    else
                         lsvStations.ItemsSource = bll.GetBaseStations();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// view of the list of stations with available charging slots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// refresh the view of the list to be the original
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
