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
        }
        public AddParcelWindow(BlApi.IBL bl, Parcel p)
        {
            InitializeComponent();
            bll = bl;
            //newParcel = new Parcel { CodeParcel = p.Id, SenderCustomer = bll.GetParcels().ToList().Find(parcel => parcel.NameSender == p.NameSender) };
            newParcel = p;
            DataContext = newParcel;

        }

        private void txtIdParcel_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtSchedule_TextChanged(object sender, TextChangedEventArgs e)
        {

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
    }
}
