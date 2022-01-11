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
        #region constructors
        /// <summary>
        /// adding ctor
        /// </summary>
        /// <param name="bl"></param>
        public AddBaseStationWindow(BlApi.IBL bl)
        {
            try
            {
                InitializeComponent();
                newStation = new();
                newStation.Location = new Location(0,0);
                bll = bl;
                DataContext = newStation;
                btnUpdateStation.Visibility = Visibility.Hidden;
                lblDronesListOfStation.Visibility = Visibility.Hidden;
                lsvDronesListOfStation.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }
        }
        /// <summary>
        /// updating or showing ctor
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="bs"></param>
        public AddBaseStationWindow(BlApi.IBL bl,BaseStationForList bs)
        {
            try
            {
                InitializeComponent();
                lblWindowTitle.Content = "Update A Base Station:";
                bll = bl;
                newStation = bll.GetBaseStation(bs.Id);
                DataContext = newStation;
                btnAddStation.Visibility = Visibility.Hidden;
                lsvDronesListOfStation.ItemsSource = newStation.ListDroneCharge.ToList();
                txtIdStation.IsEnabled = false;
                txtLongitudeStation.IsEnabled = false;
                txtLatitudeStation.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        #endregion
        #region buttons and clicks events
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
        /// <summary>
        /// open the drone window when double clicking on one drone of the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowThisDrone(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ///only if there are drones in the list
                if(newStation.ListDroneCharge.Count() != 0)
                {
                    Drone d = bll.GetDrone(((DroneInCharge)lsvDronesListOfStation.SelectedItem).Id);
                    new AddDroneWindow(bll, d).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        #endregion
        #region checking input integrity
        /// <summary>
        /// a special function for checking the input integrity
        /// </summary>
        /// <param name="e">the value was entered by the keyboard</param>
        private void integrityInputCheck(KeyEventArgs e)
        {
            // Allow errows, Back and delete keys:
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Decimal
               || e.Key == Key.CapsLock || e.Key == Key.NumLock || e.Key == Key.LeftAlt || e.Key == Key.RightAlt ||
               e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 ||
               e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4 || e.Key == Key.NumPad5 ||
               e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9 || e.Key == Key.Enter)
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
                        if (e.Key == Key.OemPeriod && !txtLongitudeStation.Text.Equals(""))
                            return;
                }
                else
                    if (!txtLatitudeStation.Text.Contains("."))
                        if (e.Key == Key.OemPeriod && !txtLatitudeStation.Text.Equals(""))
                            return;
            e.Handled = true;
            MessageBox.Show("Only digits alowed!");
        }
        /// <summary>
        /// check the integrity of the input before sending the station for adding or updating by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIdStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check the integrity of the input before sending the station for adding or updating by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNameStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check the integrity of the input before sending the station for adding or updating by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAvailableChargeSlotsOfStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check the integrity of the input before sending the station for adding or updating by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLongitudeStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check the integrity of the input before sending the station for adding or updating by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLatitudeStation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check the range of the longitude before sending the station for adding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLongitudeStation_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!btnCloseWindow.IsKeyboardFocused && !txtLongitudeStation.Text.Equals(""))
                    if (Convert.ToDouble(txtLongitudeStation.Text) < 29.3 || Convert.ToDouble(txtLongitudeStation.Text) > 33.7)
                    {
                        MessageBox.Show("The longitude must be between 29.3 to 33.7", "Error");
                        txtLongitudeStation.Clear();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        /// <summary>
        /// check the range of the latitude before sending the station for adding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLatitudeStation_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!btnCloseWindow.IsKeyboardFocused && !txtLatitudeStation.Text.Equals(""))
                    if (Convert.ToDouble(txtLatitudeStation.Text) < 33.5 || Convert.ToDouble(txtLatitudeStation.Text) > 36.3)
                    {
                        MessageBox.Show("The latitude must be between 33.5 to 36.3", "Error");
                        txtLatitudeStation.Clear();
                    }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        /// <summary>
        /// clear from the textbox the zero of the null value of an empty station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIdStation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtIdStation.Text == "0")
                txtIdStation.Clear();
        }
        /// <summary>
        /// clear from the textbox the zero of the null value of an empty station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNameStation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtNameStation.Text == "0")
                txtNameStation.Clear();
        }
        /// <summary>
        /// clear from the textbox the zero of the null value of an empty station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLongitudeStation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtLongitudeStation.Text == "0")
                txtLongitudeStation.Clear();
        }
        /// <summary>
        /// clear from the textbox the zero of the null value of an empty station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLatitudeStation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtLatitudeStation.Text == "0")
                txtLatitudeStation.Clear();
        }
        /// <summary>
        /// clear from the textbox the zero of the null value of an empty station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAvailableChargeSlotsOfStation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtAvailableChargeSlotsOfStation.Text == "0")
                txtAvailableChargeSlotsOfStation.Clear();
        }
        #endregion
    }
}