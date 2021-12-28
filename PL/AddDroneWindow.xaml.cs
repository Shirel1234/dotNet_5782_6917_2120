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
            newDrone = new();
            DataContext = newDrone;
            cmbWeightDrone.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            cmbIdStation.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
            grdUpdating.Visibility = Visibility.Hidden;
        }
        public AddDroneWindow(BlApi.IBL bl, Drone drone)
        {
            InitializeComponent();
            bll = bl;
            newDrone = drone; //new Drone() { Id= droneList.Id, ModelDrone=droneList.ModelDrone, MaxWeight=droneList.Weight};
            DataContext = newDrone;
            cmbWeightDrone.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            cmbIdStation.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
            btnAdd.Visibility = Visibility.Hidden;
            txtIdDrone.Text = newDrone.Id.ToString();
            txtIdDrone.IsEnabled = false;
            txtModelDrone.Text = newDrone.ModelDrone;
            cmbWeightDrone.SelectedValue = newDrone.MaxWeight;
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
            //newDrone = new Drone();
            //if (IsFillObject())
            //{
                BaseStationForList b = (BaseStationForList)cmbIdStation.SelectedItem;
                bll.AddDrone(newDrone, b.Id);
                MessageBox.Show("The new Drone was successfully added", "Done");
                this.Close();
            //}
            //else
            //    MessageBox.Show("The new Drone wasn't added", "Error");

        }
        //private bool IsFillObject()
        //{
        //    bool valid = true;
        //    try
        //    {
        //        if (Convert.ToInt32(txtIdDrone.Text) < 0)
        //            throw new Exception("Id can't be negetive number");
        //        newDrone.Id = Convert.ToInt32(txtIdDrone.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        valid = false;
        //    }
        //    try
        //    {
        //        newDrone.ModelDrone = txtModelDrone.Text;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        valid = false;
        //    }
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        valid = false;
        //    }
        //    newDrone.MaxWeight = (WeightCategories)cmbWeightDrone.SelectedItem;

        //    return valid;
        //}

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdateDrone_Click(object sender, RoutedEventArgs e)
        {
            //newDrone = new Drone();
            //if (IsFillObject())
            //{
                bll.UpdateDrone(newDrone.Id, newDrone.ModelDrone);
                MessageBox.Show("The new Drone was successfully updated", "Done");
                this.Close();
            //}
            //else
            //    MessageBox.Show("The new Drone wasn't update", "Error");

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
    }
}
