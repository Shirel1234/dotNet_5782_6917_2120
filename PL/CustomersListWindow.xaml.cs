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
    /// Interaction logic for CustomersListWindow.xaml
    /// </summary>
    public partial class CustomersListWindow : Window
    {
        BlApi.IBL bll;
        public CustomersListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bll = bl;
            lsvCustomers.ItemsSource = bll.GetCustomers();
        }

        private void ShowThisCustomer(object sender, MouseButtonEventArgs e)
        {
            Customer c=bll.GetCustomer(((CustomerForList)lsvCustomers.SelectedItem).Id);
            new AddCustomerWindow(bll,c).ShowDialog();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            new AddCustomerWindow(bll, true).ShowDialog();
        }

        private void lsvCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
