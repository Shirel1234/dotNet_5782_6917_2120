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
    /// Interaction logic for MainCustomerWindow.xaml
    /// </summary>
    public partial class MainCustomerWindow : Window
    {
        BlApi.IBL bll;
        int idCustomer;
        public MainCustomerWindow(BlApi.IBL bl, int id)
        {
            InitializeComponent();
            bll = bl;
            idCustomer = id;
            Customer myCustomer = bll.GetCustomer(idCustomer);
            lsvParcelsSender.ItemsSource = bl.GetParcels().ToList().FindAll(p => p.NameSender == myCustomer.Name);
            lsvParcelsTarget.ItemsSource = bl.GetParcels().ToList().FindAll(p => p.NameTarget == myCustomer.Name);
            if (DateTime.Now.Hour > 16 || DateTime.Now.Hour < 5)
                lblTitleNight.Visibility = Visibility.Visible;
            else
                lblTitleDay.Visibility = Visibility.Visible;

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            new AddCustomerWindow(bll,bll.GetCustomer(idCustomer)).ShowDialog();
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            new AddParcelWindow(bll).ShowDialog();
        }

        private void btnShowSended_Click(object sender, RoutedEventArgs e)
        {
            lsvParcelsSender.Visibility = Visibility.Visible;
        }

        private void btnShowTargeted_Click(object sender, RoutedEventArgs e)
        {
            lsvParcelsTarget.Visibility = Visibility.Visible;

        }
    }
}
