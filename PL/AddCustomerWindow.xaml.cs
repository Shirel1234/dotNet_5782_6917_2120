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
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        BlApi.IBL bll;
        Customer newCustomer;
        public AddCustomerWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bll = bl;
            newCustomer = new();
            DataContext = newCustomer;
            btnUpdateCustomer.Visibility = Visibility.Hidden;
            grdShowCustomer.Visibility = Visibility.Hidden;

        }
        public AddCustomerWindow(BlApi.IBL bl,Customer c)
        {
            InitializeComponent();
            bll = bl;
            newCustomer = c;
            DataContext = newCustomer;
            btnAddCustomer.Visibility = Visibility.Hidden;
            //txtIdCustomer.Text = newCustomer.Id.ToString();
            //txtNameCustomer.Text = newCustomer.Name.ToString();
            //txtPhoneCustomer.Text = newCustomer.Phone.ToString();
            //txtLongitudeCustomer.Text = newCustomer.Location.Longitude.ToString();
            //txtLatitudeCustomer.Text = newCustomer.Location.Latitude.ToString();
            lsvSentParcels.ItemsSource = newCustomer.SendParcels.ToList();
            lsvAcceptedParcels.ItemsSource = newCustomer.TargetParcels.ToList();
            txtIdCustomer.IsEnabled = false;
            txtLongitudeCustomer.IsEnabled = false;
            txtLatitudeCustomer.IsEnabled = false;
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.AddCustomer(newCustomer);
                MessageBox.Show("The new Customer was successfully added", "Done");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "The Customer could not be added");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bll.UpdateCustomer(newCustomer.Id, newCustomer.Name, newCustomer.Phone);
                MessageBox.Show("The customer was successfully updated", "Done");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "The customer could not be updated");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowThisParcelFromSent(object sender, MouseButtonEventArgs e)
        {
            Parcel p = bll.GetParcel(((ParcelInCustomer)lsvSentParcels.SelectedItem).Id);
            new AddParcelWindow(bll, p).ShowDialog();
        }
        private void ShowThisParcelFromAccepted(object sender, MouseButtonEventArgs e)
        {
            Parcel p = bll.GetParcel(((ParcelInCustomer)lsvAcceptedParcels.SelectedItem).Id);
            new AddParcelWindow(bll, p).ShowDialog();
        }
    }
}
