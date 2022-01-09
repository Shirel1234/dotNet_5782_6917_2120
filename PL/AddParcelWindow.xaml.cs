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
        Parcel newParcel;
        public AddParcelWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bll = bl;
            newParcel = new();
            DataContext = newParcel;
            grdForUpdateParcel.Visibility = Visibility.Hidden;
            grdCmbAddParcel.Visibility = Visibility.Visible;
            cmbPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
            cmbWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            cmbSenders.ItemsSource = bll.GetCustomers();
            cmbTargets.ItemsSource = bll.GetCustomers();

        }
        public AddParcelWindow(BlApi.IBL bl, Parcel p)
        {
            InitializeComponent();
            lblWindowTitle.Content = "Update a Parcel:";
            bll = bl;
            //newParcel = new Parcel { CodeParcel = p.Id, SenderCustomer = bll.GetParcels().ToList().Find(parcel => parcel.NameSender == p.NameSender) };
            newParcel = p;
            DataContext = newParcel;
            grdShowParcel.Visibility = Visibility.Visible;
            if (newParcel.Scheduled == null)
                btnRemoveParcel.Visibility = Visibility.Visible;
        }
        private void ShowDrone(object sender, MouseButtonEventArgs e)
        {
            Drone d = bll.GetDrone(Convert.ToInt32(lsbDroneInParcel.SelectedItem));
            new AddDroneWindow(bll, d);
        }

        private void ShowSenderCustomer(object sender, MouseButtonEventArgs e)
        {
            Customer c = bll.GetCustomer(((CustomerInParcel)cmbSenders.SelectedValue).Id);
            new AddCustomerWindow(bll, c);
        }

        private void ShowTargetCustomer(object sender, MouseButtonEventArgs e)
        {
            Customer c = bll.GetCustomer(((CustomerInParcel)cmbTargets.SelectedValue).Id);
            new AddCustomerWindow(bll, c);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            newParcel.SenderCustomer = new CustomerInParcel { Id = ((CustomerForList)cmbSenders.SelectedItem).Id, Name = ((CustomerForList)cmbSenders.SelectedItem).Name };
            newParcel.TargetCustomer = new CustomerInParcel { Id = ((CustomerForList)cmbTargets.SelectedItem).Id, Name = ((CustomerForList)cmbTargets.SelectedItem).Name };
            try
            {
                bll.AddParcel(newParcel);
                MessageBox.Show("The new parcel was successfully added", "Done");
                this.Close();
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
                bll.RemoveParcel(newParcel.CodeParcel);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
    }
}
