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
        #region constructor
        public CustomersListWindow(BlApi.IBL bl)
        {
            try
            {
                InitializeComponent();
                bll = bl;
                lsvCustomers.ItemsSource = bll.GetCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
        #region buttons and clicks events
        private void ShowThisCustomer(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if(!lsvCustomers.Items.IsEmpty)
                {
                    Customer c = bll.GetCustomer(((CustomerForList)lsvCustomers.SelectedItem).Id);
                    new AddCustomerWindow(bll, c).ShowDialog();
                }
                lsvCustomers.ItemsSource = bll.GetCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new AddCustomerWindow(bll, true).ShowDialog();
                lsvCustomers.ItemsSource = bll.GetCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
    }
}
