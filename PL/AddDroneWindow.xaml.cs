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
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class AddDroneWindow : Window
    {
        BlApi.IBL bll;
        Drone newDrone;
        public AddDroneWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bll = bl;
            //newDrone = new();
            DataContext = newDrone;
            cmbWeightDrone.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            cmbIdStation.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
            grdShowDrone.Visibility = Visibility.Hidden;
            grdUpdating.Visibility = Visibility.Hidden;
        }
        public AddDroneWindow(BlApi.IBL bl, Drone drone)
        {
            InitializeComponent();
            lblTitleDrone.Content = "Update Drone:";
            bll = bl;
            newDrone = drone; //new Drone() { Id= droneList.Id, ModelDrone=droneList.ModelDrone, MaxWeight=droneList.Weight};
            DataContext = newDrone;
            cmbWeightDrone.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            cmbIdStation.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
            cmbStatus.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            btnAdd.Visibility = Visibility.Hidden;
            //txtIdDrone.Text = newDrone.Id.ToString();
            txtIdDrone.IsEnabled = false;
           // txtModelDrone.Text = newDrone.ModelDrone;
           // cmbWeightDrone.SelectedValue = newDrone.MaxWeight;
            lblIdStation.Visibility = Visibility.Hidden;
            cmbIdStation.Visibility = Visibility.Hidden;
            cmbIdStation.IsEnabled = false;
            if (newDrone.DroneStatus == DroneStatuses.free)
            {
                btnSendForCharging.Visibility = Visibility.Visible;
                btnSchedulingForSending.Visibility = Visibility.Visible;
            }
            else
                if (newDrone.DroneStatus == DroneStatuses.maintenace)
                btnReleaseDroneCharging.Visibility = Visibility.Visible;
            else
            //in sending
            {
                if (bl.GetParcel(newDrone.ParcelInWay.Id).PickedUp == null)
                    btnPickUpSending.Visibility = Visibility.Visible;
                else 
                    btnDelivered.Visibility = Visibility.Hidden;
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtIdDrone.Text == "0" || txtIdDrone.Text == "" || txtModelDrone.Text == "0" || txtModelDrone.Text == ""||cmbWeightDrone.Text==""||cmbIdStation.Text=="")
                    MessageBox.Show("One or more of the fields are empty. Please complete the missing information.", "Error");
                else
                {
                    BaseStationForList b = (BaseStationForList)cmbIdStation.SelectedItem;
                    bll.AddDrone(newDrone, b.Id);
                    MessageBox.Show("The new Drone was successfully added", "Done");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message , "The new Drone wasn't added");
            }

        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdateDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtModelDrone.Text == "0" || txtModelDrone.Text == "")
                    MessageBox.Show("No field updated.", "Error");
                else
                {
                    bll.UpdateDrone(newDrone.Id, newDrone.ModelDrone);
                    MessageBox.Show("The new Drone was successfully updated", "Done");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "The new Drone could not updated");
            }
        }

        private void btnSendForCharging_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.UpdateSendingDroneToCharge(Convert.ToInt32(txtIdDrone.Text));
                MessageBox.Show("The charging was successfully done", "Done");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "The charging wasn't updated");

            }

        }
        private void btnSchedulingForSending_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.UpdateParcelToDrone(Convert.ToInt32(txtIdDrone.Text));
                MessageBox.Show("The scheduling was successfully done", "Done");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "The charging wasn't updated");

            }
        }

        private void btnDelivered_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.UpdateDeliveredParcelByDrone(Convert.ToInt32(txtIdDrone.Text));
                MessageBox.Show("The delivering was successfully done", "Done");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "The charging wasn't updated");
            }
        }

        private void btnPickUpSending_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.UpdateParcelPickedUpByDrone(Convert.ToInt32(txtIdDrone.Text));
                MessageBox.Show("The picking up was successfully done", "Done");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "The charging wasn't updated"); }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.UpdateReleasingDroneFromCharge(Convert.ToInt32(txtIdDrone.Text));
                MessageBox.Show("The releasing was successfully done", "Done");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "The charging wasn't updated"); }
        }

        private void ShowThisParcel(object sender, MouseButtonEventArgs e)
        {
            Parcel p = bll.GetParcel(((ParcelInCustomer)lsbParcelInWay.SelectedItem).Id);
            new AddParcelWindow(bll, p).ShowDialog();
        }
        private void integrityInputCheck(KeyEventArgs e)
        {
            // Allow errows, Back and delete keys:
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Decimal
                ||e.Key==Key.CapsLock|| e.Key == Key.NumLock|| e.Key == Key.LeftAlt|| e.Key == Key.RightAlt||
                e.Key == Key.LeftShift|| e.Key == Key.RightShift || e.Key==Key.NumPad0)
                return;
            // Allow only digits:
            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (txtModelDrone.IsFocused)
                if (char.IsLetterOrDigit(c))
                    return;
                else
                {
                    e.Handled = true;
                    MessageBox.Show("Only letters or digits alowed!");
                    return;
                }
            if (char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return;
            //for longitude and latitude of location, allow a point to be only once and not in the beginning of the number
            if (txtLongitude.IsFocused || txtLatitude.IsFocused)
                if (txtLongitude.IsFocused)
                {
                    if (!txtLongitude.Text.Contains("."))
                        if (e.Key == Key.OemPeriod && !(txtLongitude.Text.Equals("0") || txtLongitude.Text.Equals("")))
                            return;
                }
                else
                    if (!txtLatitude.Text.Contains("."))
                    if (e.Key == Key.OemPeriod && !(txtLatitude.Text.Equals("0") || txtLatitude.Text.Equals("")))
                        return;
            e.Handled = true;
            MessageBox.Show("Only digits alowed!");
        }

        private void txtIdDrone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtModelDrone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtLongitude_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtLatitude_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtLongitude_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!btnClose.IsKeyboardFocused && !txtLongitude.Text.Equals(""))
                if (Convert.ToDouble(txtLongitude.Text) < 29.3 || Convert.ToDouble(txtLongitude.Text) > 33.7)
                {
                    MessageBox.Show("The longitude must be between 29.3 to 33.7", "Error");
                    txtLongitude.Clear();
                }
        }

        private void txtLatitude_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!btnClose.IsKeyboardFocused && !txtLatitude.Text.Equals(""))
                if (Convert.ToDouble(txtLatitude.Text) < 33.5 || Convert.ToDouble(txtLatitude.Text) > 36.3)
                {
                    MessageBox.Show("The latitude must be between 33.5 to 36.3", "Error");
                    txtLatitude.Clear();
                }
        }
    }
}
