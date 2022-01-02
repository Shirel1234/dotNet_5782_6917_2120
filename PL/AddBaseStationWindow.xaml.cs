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
    /// Interaction logic for AddBaseStationWindow.xaml
    /// </summary>
    public partial class AddBaseStationWindow : Window
    {
        BlApi.IBL bll;
        BaseStation newStation;
        public AddBaseStationWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bll = bl;
            newStation = new();
            DataContext = newStation;
            btnUpdateStation.Visibility = Visibility.Hidden;
        }
        public AddBaseStationWindow(BlApi.IBL bl,BaseStationForList bs)
        {
            InitializeComponent();
            lblWindowTitle.Content = "Update A Base Station:";
            bll = bl;
            newStation = bll.GetBaseStation(bs.Id);
            DataContext = newStation;
            btnAddStation.Visibility = Visibility.Hidden;
            //txtIdStation.Text = newStation.Id.ToString();
            //txtNameStation.Text = newStation.Name.ToString();
            //txtLongitudeStation.Text = newStation.Location.Longitude.ToString();
            //txtLatitudeStation.Text = newStation.Location.Latitude.ToString();
            //txtAvailableChargeSlotsOfStation.Text = newStation.ChargeSlots.ToString();
            lsvDronesListOfStation.ItemsSource = newStation.ListDroneCharge.ToList();
            txtIdStation.IsEnabled = false;
            txtLongitudeStation.IsEnabled = false;
            txtLatitudeStation.IsEnabled = false;
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.AddBaseStation(newStation);
                MessageBox.Show("The new base station was successfully added", "Done");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "The base station could not be added");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.UpdateBaseStation(newStation.Id, newStation.Name, newStation.ChargeSlots);
                MessageBox.Show("The base station was successfully updated", "Done");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "The base station could not be updated");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowThisDrone(object sender, MouseButtonEventArgs e)
        {
            Drone d = bll.GetDrone(((DroneInCharge)lsvDronesListOfStation.SelectedItem).Id);
            new AddDroneWindow(bll, d).ShowDialog();
        }

        private void lsvDronesListOfStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
