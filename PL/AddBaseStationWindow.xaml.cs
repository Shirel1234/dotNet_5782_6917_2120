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
            //newStation = new();
            //newStation.Location = new Location(0, 0);
            DataContext = newStation;
            txtIdStation.Clear();
            //txtNameStation.Clear();
            //txtLongitudeStation.Clear();
            //txtLatitudeStation.Clear();
            //txtAvailableChargeSlotsOfStation.Clear();
            btnUpdateStation.Visibility = Visibility.Hidden;
        }
        public AddBaseStationWindow(BlApi.IBL bl,BaseStationForList bs)
        {
            InitializeComponent();
            lblWindowTitle.Content = "Update A Base Station:";
            bll = bl;
            newStation = bll.GetBaseStation(bs.Id);
            newStation.Location = new Location(0, 0);
            DataContext = newStation;
            btnAddStation.Visibility = Visibility.Hidden;
            lsvDronesListOfStation.ItemsSource = newStation.ListDroneCharge.ToList();
            txtIdStation.IsEnabled = false;
            txtLongitudeStation.IsEnabled = false;
            txtLatitudeStation.IsEnabled = false;
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtIdStation.Text == "" || txtNameStation.Text == "" || txtLongitudeStation.Text == "" || txtLatitudeStation.Text == "" || txtAvailableChargeSlotsOfStation.Text == "")
                    MessageBox.Show("One or more of the fields are empty. Please complete the missing information.", "Error");
                
                else
                {
                    bll.AddBaseStation(newStation);
                    MessageBox.Show("The new base station was successfully added", "Done");
                    this.Close();
                }
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
                BaseStation b = bll.GetBaseStation(newStation.Id);
                if (txtNameStation.Text == Convert.ToString(b.Name) && txtAvailableChargeSlotsOfStation.Text == Convert.ToString(b.ChargeSlots))
                    MessageBox.Show("No field updated.", "Error");
                else
                {
                    bll.UpdateBaseStation(newStation.Id, newStation.Name, newStation.ChargeSlots);
                    MessageBox.Show("The base station was successfully updated", "Done");
                    this.Close();
                }
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

        private void txtIdStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        private void integrityInputCheck(KeyEventArgs e)
        {
            //e.Handled = false;
            //txtIdComments.Visibility = Visibility.Collapsed;
            //btnAddStation.IsEnabled = true;
            // Allow errows, Back and delete keys:
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Decimal)
                return;
            // Allow only digits:
            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return;
            //for longitude and latitude of location, allow a point to be only once and not in the beginning of the number
            if (txtLongitudeStation.IsFocused || txtLatitudeStation.IsFocused)
                if (txtLongitudeStation.IsFocused)
                {
                    if (!txtLongitudeStation.Text.Contains("."))
                        if (e.Key == Key.OemPeriod && !(txtLongitudeStation.Text.Equals("0") || txtLongitudeStation.Text.Equals("")))
                            return;
                }
                else
                    if (!txtLatitudeStation.Text.Contains("."))
                        if (e.Key == Key.OemPeriod && !(txtLatitudeStation.Text.Equals("0") || txtLatitudeStation.Text.Equals("")))
                            return;
            e.Handled = true;
                    //txtIdComments.Text = "*only digits alowed";
                    //txtIdComments.Visibility = Visibility.Visible;
                    //btnAddStation.IsEnabled = false;
            MessageBox.Show("Only digits alowed!");
        }

        private void txtNameStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtAvailableChargeSlotsOfStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtLongitudeStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtLatitudeStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtLongitudeStation_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!btnCloseWindow.IsKeyboardFocused &&!txtLongitudeStation.Text.Equals("")) 
                if(Convert.ToDouble(txtLongitudeStation.Text) < 29.3 || Convert.ToDouble(txtLongitudeStation.Text) > 33.7)
            {
                MessageBox.Show("The longitude must be between 29.3 to 33.7", "Error");
                txtLongitudeStation.Clear();
            }
        }

        private void txtLatitudeStation_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!btnCloseWindow.IsKeyboardFocused && !txtLatitudeStation.Text.Equals(""))
                if (Convert.ToDouble(txtLatitudeStation.Text) < 33.5 || Convert.ToDouble(txtLatitudeStation.Text) > 36.3)
                {
                    MessageBox.Show("The latitude must be between 33.5 to 36.3", "Error");
                    txtLatitudeStation.Clear();
                }
        }
    }
}
