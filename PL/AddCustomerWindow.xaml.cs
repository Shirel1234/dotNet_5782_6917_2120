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
    ///constructor for adding a customer
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        BlApi.IBL bll;
        Customer newCustomer;
        public AddCustomerWindow(BlApi.IBL bl, bool IsWorker)
        {
            InitializeComponent();
            bll = bl;
            newCustomer = new();
            newCustomer.Location = new Location(0,0);
            DataContext = newCustomer;
            btnUpdateCustomer.Visibility = Visibility.Hidden;
            grdShowCustomer.Visibility = Visibility.Hidden;
            if (!IsWorker)
                ckbIsWorker.Visibility = Visibility.Hidden;
        }
        ///constructor for updating or showing a customer
        public AddCustomerWindow(BlApi.IBL bl,Customer c)
        {
            InitializeComponent();
            lblWindowTitle.Content = "Update a Customer:";
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
                //txtIdCustomer.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                if (txtIdCustomer.Text == "" || txtNameCustomer.Text == "" || txtPhoneCustomer.Text == "" || txtLongitudeCustomer.Text == "" || txtLatitudeCustomer.Text == "")
                    MessageBox.Show("One or more of the fields are empty. Please complete the missing information.", "Error");
                else
                {
                    bll.AddCustomer(newCustomer);
                    MessageBox.Show("The new Customer was successfully added.", "Done");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "The Customer could not be added.");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer c = bll.GetCustomer(newCustomer.Id);
                if(txtNameCustomer.Text == c.Name && txtPhoneCustomer.Text == c.Phone)
                    MessageBox.Show("No field updated.", "Error");
                else
                {
                    bll.UpdateCustomer(newCustomer.Id, newCustomer.Name, newCustomer.Phone);
                    MessageBox.Show("The customer was successfully updated", "Done");
                    this.Close();
                }
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

        private void txtIdCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        private void integrityInputCheck(KeyEventArgs e)
        {
            // Allow errows, Back and delete keys:
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Decimal)
                return;
            // Allow only digits:
            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (txtNameCustomer.IsFocused)
                if (char.IsWhiteSpace(c) || char.IsLetter(c))
                    return;
                else
                {
                    e.Handled = true;
                    MessageBox.Show("Only letters or spaces alowed!");
                    return;
                }
            if (char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return;
            //for longitude and latitude of location, allow a point to be only once and not in the beginning of the number
            if (txtLongitudeCustomer.IsFocused || txtLatitudeCustomer.IsFocused)
                if (txtLongitudeCustomer.IsFocused)
                {
                    if (!txtLongitudeCustomer.Text.Contains("."))
                        if (e.Key == Key.OemPeriod && !(txtLongitudeCustomer.Text.Equals("0") || txtLongitudeCustomer.Text.Equals("")))
                            return;
                }
                else
                    if (!txtLatitudeCustomer.Text.Contains("."))
                        if (e.Key == Key.OemPeriod && !(txtLatitudeCustomer.Text.Equals("0") || txtLatitudeCustomer.Text.Equals("")))
                            return;
            e.Handled = true;
            MessageBox.Show("Only digits alowed!");
        }

        private void txtIdCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!btnCloseWindow.IsKeyboardFocused)
                if (!txtIdCustomer.Text.Equals("") && txtIdCustomer.Text.Length < 9)
                {
                    MessageBox.Show("The id must contain 9 digits", "ERROR");
                    txtIdCustomer.Clear();
                }
        }

        private void txtNameCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        private void txtPhoneCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        private void txtLongitudeCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtLatitudeCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void txtLongitudeCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!btnCloseWindow.IsKeyboardFocused && !txtLongitudeCustomer.Text.Equals(""))
                if (Convert.ToDouble(txtLongitudeCustomer.Text) < 29.3 || Convert.ToDouble(txtLongitudeCustomer.Text) > 33.7)
                {
                    MessageBox.Show("The longitude must be between 29.3 to 33.7", "Error");
                    txtLongitudeCustomer.Clear();
                }
        }

        private void txtLatitudeCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!btnCloseWindow.IsKeyboardFocused && !txtLatitudeCustomer.Text.Equals(""))
                if (Convert.ToDouble(txtLatitudeCustomer.Text) < 33.5 || Convert.ToDouble(txtLatitudeCustomer.Text) > 36.3)
                {
                    MessageBox.Show("The latitude must be between 33.5 to 36.3", "Error");
                    txtLatitudeCustomer.Clear();
                }
        }
        /// <summary>
        /// check if the phone number contains 10 digits and starts with '05'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPhoneCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!btnCloseWindow.IsKeyboardFocused)
            {
                if (!txtPhoneCustomer.Text.Equals("") && txtPhoneCustomer.Text.Length < 10)
                {
                    MessageBox.Show("The phone number must contain 10 digits", "ERROR");
                    txtPhoneCustomer.Clear();
                }
                else
                {
                    if (!txtPhoneCustomer.Text.Equals("") && !txtPhoneCustomer.Text.StartsWith("05"))
                    {
                        MessageBox.Show("The phone number must start with '05'", "ERROR");
                        txtPhoneCustomer.Clear();
                    }
                }
            }
        }
    }
}
