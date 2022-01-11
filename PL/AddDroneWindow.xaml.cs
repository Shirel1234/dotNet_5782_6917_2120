using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        Drone myDrone;
        BackgroundWorker worker;
        bool Auto;
        public AddDroneWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bll = bl;
            myDrone = new();
            DataContext = myDrone;
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
            myDrone = drone; //new Drone() { Id= droneList.Id, ModelDrone=droneList.ModelDrone, MaxWeight=droneList.Weight};
            DataContext = myDrone;
            cmbWeightDrone.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            cmbIdStation.ItemsSource = bll.GetAllBaseStationsWithChargePositions();
            cmbStatus.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            btnAdd.Visibility = Visibility.Hidden;
            txtIdDrone.IsEnabled = false;
            lblIdStation.Visibility = Visibility.Hidden;
            cmbIdStation.Visibility = Visibility.Hidden;
            cmbIdStation.IsEnabled = false;
            if (myDrone.DroneStatus == DroneStatuses.free)
            {
                btnSendForCharging.Visibility = Visibility.Visible;
                btnSchedulingForSending.Visibility = Visibility.Visible;
            }
            else
                if (myDrone.DroneStatus == DroneStatuses.maintenace)
                btnReleaseDroneCharging.Visibility = Visibility.Visible;
            else
            //in sending
            {
                if (bl.GetParcel(myDrone.ParcelInWay.Id).PickedUp == null)
                    btnPickUpSending.Visibility = Visibility.Visible;
                else 
                    btnDelivered.Visibility = Visibility.Hidden;
            }
        }
        //private void Worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    bll.StartSimulator(myDrone.Id, update, stop);
        //    //Stopwatch stopwatch = new Stopwatch();
        //    //stopwatch.Start();
        //    //while (worker.WorkerReportsProgress)
        //    //{ 
        //    //// BackgroundWorker worker = sender as BackgroundWorker;
        //    //int length = (int)e.Argument;
        //    //    //the drone is free
        //    //    if (newDrone.DroneStatus == DroneStatuses.free)
        //    //    {
        //    //        //the drone send to charging
        //    //        if (newDrone.Battery <= 0)
        //    //        { 
        //    //            bll.UpdateSendingDroneToCharge(newDrone.Id);
        //    //             for (int i = 1; i <= length; i++)
        //    //              {
        //    //                 if (worker.CancellationPending == true)
        //    //                {
        //    //                e.Cancel = true;
        //    //                e.Result = stopwatch.ElapsedMilliseconds; // Unnecessary
        //    //                break;
        //    //                 }
        //    //                 else
        //    //                  {
        //    //                {

        //    //                    // Perform a time consuming operation and report progress.
        //    //                    System.Threading.Thread.Sleep(500);
        //    //                    worker.ReportProgress(i * 100 / length);
        //    //                }
        //    //            }
        //    //        }

        //    //    e.Result = stopwatch.ElapsedMilliseconds;
        //    //}
        //}
        //private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    int progress = e.ProgressPercentage;
        //    resultLabel.Content = (progress + "%");
        //   // resultProgressBar.Value = progress;
        //}
        //private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (e.Cancelled == true)
        //    {
        //        // e.Result throw System.InvalidOperationException
        //        resultLabel.Content = "Canceled!";
        //    }
        //    else if (e.Error != null)
        //    {
        //        // e.Result throw System.Reflection.TargetInvocationException
        //        resultLabel.Content = "Error: " + e.Error.Message; //Exception Message
        //    }
        //    else
        //    {
        //        long result = (long)e.Result;
        //        if (result < 1000)
        //            resultLabel.Content = "Done after " + result + " ms.";
        //        else
        //            resultLabel.Content = "Done after " + result / 1000 + " sec.";
        //    }
        //}
        /// <summary>
        /// the function update the drone view by ReportProgress
        /// </summary>
        private void updateDroneView() => worker.ReportProgress(0);

        private bool IsStop() { return worker.CancellationPending; }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtIdDrone.Text == "0" || txtIdDrone.Text == "" || txtModelDrone.Text == "0" || txtModelDrone.Text == ""||cmbWeightDrone.Text==""||cmbIdStation.Text=="")
                    MessageBox.Show("One or more of the fields are empty. Please complete the missing information.", "Error");
                else
                {
                    BaseStationForList b = (BaseStationForList)cmbIdStation.SelectedItem;
                    bll.AddDrone(myDrone, b.Id);
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
                    bll.UpdateDrone(myDrone.Id, myDrone.ModelDrone);
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

        private void btnSimulator_Click(object sender, RoutedEventArgs e)
        {
            Auto = true;
            worker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true, };
            worker.DoWork += (sender, args) => bll.StartSimulator((int)args.Argument, updateDroneView, IsStop);
            worker.ProgressChanged += (sender, args) => updateDroneView();
            worker.RunWorkerCompleted += (sender, args) => Auto = false;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync(myDrone.Id);
            //if (worker.IsBusy != true)
            //    // Start the asynchronous operation. 
            //    worker.RunWorkerAsync(12);

        }

        private void btnStopSimulator_Click(object sender, RoutedEventArgs e)
        {

            if (worker.WorkerSupportsCancellation == true)
                // Cancel the asynchronous operation.
                worker.CancelAsync();
        }
    }
}
