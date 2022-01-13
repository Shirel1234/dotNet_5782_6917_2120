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
            try
            {
                new MainCustomerWindow(bl, Convert.ToInt32(txtId.Text)).ShowDialog();
                btnWorker.Visibility = Visibility.Hidden;
                btnCustomer.Visibility = Visibility.Visible;
                btnNewCustomer.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new AddCustomerWindow(bl, false).ShowDialog();
                btnCustomer.Visibility = Visibility.Visible;
                btnNewCustomer.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void btnWorker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new MainWindow(bl).ShowDialog();
                btnWorker.Visibility = Visibility.Visible;
                btnCustomer.Visibility = Visibility.Visible;
                btnNewCustomer.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
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
                    if (c.IsWorker == true)
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

        private void txtId_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        private void integrityInputCheck(KeyEventArgs e)
        {
            // Allow errows, Back and delete keys:
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Decimal
                || e.Key == Key.CapsLock || e.Key == Key.NumLock || e.Key == Key.LeftAlt || e.Key == Key.RightAlt ||
                e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 ||
                e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4 || e.Key == Key.NumPad5 ||
                e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9 || e.Key == Key.Enter)

                return;
            // Allow only digits:
            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return;
            e.Handled = true;
            MessageBox.Show("Only digits alowed!");
        }

    }
}

