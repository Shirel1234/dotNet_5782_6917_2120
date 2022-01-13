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
    /// Interaction logic for AddParcelWindow.xaml
    /// </summary>
    public partial class AddParcelWindow : Window
    {
        BlApi.IBL bll;
        Parcel myParcel;
        #region constructors
        /// <summary>
        /// ctor for adding
        /// </summary>
        /// <param name="bl"></param>
        public AddParcelWindow(BlApi.IBL bl)
        {
            try
            {
                InitializeComponent();
                bll = bl;
                myParcel = new();
                DataContext = myParcel;
                grdForUpdateParcel.Visibility = Visibility.Hidden;
                grdShowParcel.Visibility = Visibility.Hidden;
                lblIdParcel.Visibility = Visibility.Hidden;
                txtIdParcel.Visibility = Visibility.Hidden;
                //btnRemoveParcel.Visibility = Visibility.Hidden;
                cmbPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
                cmbWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                cmbSenders.ItemsSource = bll.GetCustomers();
                cmbTargets.ItemsSource = bll.GetCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// ctor for showing the parcel
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="p"></param>
        public AddParcelWindow(BlApi.IBL bl, Parcel p)
        {
            try
            {
                InitializeComponent();
                lblWindowTitle.Content = "View a Parcel:";
                bll = bl;
                myParcel = p;
                DataContext = myParcel;
                grdShowParcel.Visibility = Visibility.Visible;
                grdCmbAddParcel.Visibility = Visibility.Hidden;
                cmbPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
                cmbWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                btnAdd.Visibility = Visibility.Hidden;
                if (myParcel.Scheduled == null)
                    btnRemoveParcel.Visibility = Visibility.Visible;
                if (myParcel.DroneInParcel.Id != 0)
                    btnOpenDroneWindow.Visibility = Visibility.Visible;
                else
                    txtDroneInParcel.Text = "The parcel has not yet been scheduled";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
        #region buttons and clicks events
        private void ShowSenderCustomer(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Customer c = bll.GetCustomer(((CustomerInParcel)lblShowSender.Content).Id);
                new AddCustomerWindow(bll, c).ShowDialog();
            }
             catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void ShowTargetCustomer(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Customer c = bll.GetCustomer(((CustomerInParcel)lblShowTarget.Content).Id);
                new AddCustomerWindow(bll, c).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                if(cmbSenders.SelectedIndex==-1 || cmbTargets.SelectedIndex==-1)
                     MessageBox.Show("One or more of the details are empty. Please complete the missing information.", "ERROR");
                else
                {
                    myParcel.SenderCustomer = new CustomerInParcel { Id = ((CustomerForList)cmbSenders.SelectedItem).Id, Name = ((CustomerForList)cmbSenders.SelectedItem).Name };
                    myParcel.TargetCustomer = new CustomerInParcel { Id = ((CustomerForList)cmbTargets.SelectedItem).Id, Name = ((CustomerForList)cmbTargets.SelectedItem).Name };
                    bll.AddParcel(myParcel);
                    MessageBox.Show("The new parcel was successfully added", "Done");
                    this.Close();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message , "ERROR");
            }
        }

        private void btnRemoveParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.RemoveParcel(myParcel.CodeParcel);
                MessageBox.Show("The parcel was successfully removed", "Done");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void btnOpenDroneWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myParcel.DroneInParcel.Id != 0)
                {
                    Drone d = bll.GetDrone(myParcel.DroneInParcel.Id);
                    new AddDroneWindow(bll, d).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
    }
}
