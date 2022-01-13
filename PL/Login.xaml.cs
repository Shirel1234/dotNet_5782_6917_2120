using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        
        internal readonly IBL bl = BlFactory.GetBl();
         public Login()
        {
            InitializeComponent();

        }

        private void btnPressCustomer_Click(object sender, RoutedEventArgs e)
        {
            new MainCustomerWindow(bl,Convert.ToInt32(txtId.Text)).ShowDialog();
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            new AddCustomerWindow(bl, false).ShowDialog();
            btnCustomer.Visibility = Visibility.Visible;
            btnNewCustomer.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(bl).ShowDialog();
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtId.Text.Length < 9)
                MessageBox.Show("The id must contain 9 digits", "ERROR");
            else
            {
                try
                {
                    Customer c = bl.GetCustomer(Convert.ToInt32(txtId.Text));
                   // if (c.IsWorker == true)
                    btnWorker.Visibility = Visibility.Visible;
                    btnCustomer.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "The Customer wasn't found");
                    btnNewCustomer.Visibility = Visibility.Visible;
                }
            }

        }
    }
}

