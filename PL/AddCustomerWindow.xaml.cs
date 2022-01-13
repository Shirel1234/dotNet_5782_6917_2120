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
        #region constructors
        /// <summary>
        /// adding ctor
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="IsWorker">according to this, open or hide this check box</param>
        public AddCustomerWindow(BlApi.IBL bl, bool IsWorker)
        {
            try
            {
                InitializeComponent();
                bll = bl;
                newCustomer = new();
                newCustomer.Location = new Location(0, 0);
                DataContext = newCustomer;
                btnUpdateCustomer.Visibility = Visibility.Hidden;
                grdShowCustomer.Visibility = Visibility.Hidden;
                if (IsWorker)
                {
                    //ckbIsWorker.IsChecked = false;
                    ckbIsWorker.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        ///constructor for updating or showing a customer
        public AddCustomerWindow(BlApi.IBL bl,Customer c)
        {
            try
            {
                InitializeComponent();
                lblWindowTitle.Content = "Update a Customer:";
                bll = bl;
                newCustomer = c;
                DataContext = newCustomer;
                btnAddCustomer.Visibility = Visibility.Hidden;
                if (c.IsWorker)
                    ckbIsWorker.Visibility = Visibility.Visible;
                lsvSentParcels.ItemsSource = newCustomer.SendParcels.ToList();
                lsvAcceptedParcels.ItemsSource = newCustomer.TargetParcels.ToList();
                txtIdCustomer.IsEnabled = false;
                txtLongitudeCustomer.IsEnabled = false;
                txtLatitudeCustomer.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        #endregion
        #region buttons and clicks events
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                if(txtNameCustomer.Text == c.Name && txtPhoneCustomer.Text == c.Phone && ckbIsWorker.IsChecked.Value)
                    MessageBox.Show("No field updated.", "Error");
                else
                {
                    bll.UpdateCustomer(newCustomer.Id, newCustomer.Name, newCustomer.Phone,newCustomer.IsWorker);
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
            try
            {
                if(!lsvSentParcels.Items.IsEmpty)
                {
                    Parcel p = bll.GetParcel(((ParcelInCustomer)lsvSentParcels.SelectedItem).Id);
                    new AddParcelWindow(bll, p).ShowDialog();
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void ShowThisParcelFromAccepted(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if(!lsvAcceptedParcels.Items.IsEmpty)
                {
                    Parcel p = bll.GetParcel(((ParcelInCustomer)lsvAcceptedParcels.SelectedItem).Id);
                    new AddParcelWindow(bll, p).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void ckbIsWorker_Checked(object sender, RoutedEventArgs e)
        {
            newCustomer.IsWorker = true;
        }
        #endregion
        #region checking input integrity
        /// <summary>
        /// check the basic integrity of the input
        /// </summary>
        /// <param name="e">the value was entered by the keyboard</param>
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
                        if (e.Key == Key.OemPeriod && !txtLongitudeCustomer.Text.Equals(""))
                            return;
                }
                else
                    if (!txtLatitudeCustomer.Text.Contains("."))
                        if (e.Key == Key.OemPeriod && !txtLatitudeCustomer.Text.Equals(""))
                            return;
            e.Handled = true;
            MessageBox.Show("Only digits alowed!");
        }
        /// <summary>
        /// check the integrity of the input before sending the customer for adding by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIdCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check the integrity of the input before sending the customer for adding or updating by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNameCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check the integrity of the input before sending the customer for adding or updating by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPhoneCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check the integrity of the input before sending the customer for adding by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLongitudeCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check the integrity of the input before sending the customer for adding by the function 'integrityInputCheck'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLatitudeCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
        /// <summary>
        /// check if the ID number contains 9 digits before sending the customer for adding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIdCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!btnCloseWindow.IsKeyboardFocused)
                    if (!txtIdCustomer.Text.Equals("") && txtIdCustomer.Text.Length < 9)
                    {
                        MessageBox.Show("The id must contain 9 digits", "ERROR");
                        txtIdCustomer.Clear();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        /// <summary>
        ///  check the range of the longitude before sending the customer for adding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLongitudeCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!btnCloseWindow.IsKeyboardFocused && !txtLongitudeCustomer.Text.Equals(""))
                    if (Convert.ToDouble(txtLongitudeCustomer.Text) < 29.3 || Convert.ToDouble(txtLongitudeCustomer.Text) > 33.7)
                    {
                        MessageBox.Show("The longitude must be between 29.3 to 33.7", "Error");
                        txtLongitudeCustomer.Clear();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        /// <summary>
        /// check the range of the latitude before sending the customer for adding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLatitudeCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (!btnCloseWindow.IsKeyboardFocused && !txtLatitudeCustomer.Text.Equals(""))
                    if (Convert.ToDouble(txtLatitudeCustomer.Text) < 33.5 || Convert.ToDouble(txtLatitudeCustomer.Text) > 36.3)
                    {
                        MessageBox.Show("The latitude must be between 33.5 to 36.3", "Error");
                        txtLatitudeCustomer.Clear();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        /// <summary>
        /// check if the phone number contains 10 digits and starts with '05' before sending the customer for adding or updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPhoneCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            try
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
             catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        /// <summary>
        /// clear from the textbox the zero of the null value of an empty customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIdCustomer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtIdCustomer.Text == "0")
                txtIdCustomer.Clear();
        }
        /// <summary>
        /// clear from the textbox the zero of the null value of an empty customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLongitudeCustomer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtLongitudeCustomer.Text == "0")
                txtLongitudeCustomer.Clear();
        }
        /// <summary>
        /// clear from the textbox the zero of the null value of an empty customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLatitudeCustomer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtLatitudeCustomer.Text == "0")
                txtLatitudeCustomer.Clear();
        }
        #endregion
    }
}
