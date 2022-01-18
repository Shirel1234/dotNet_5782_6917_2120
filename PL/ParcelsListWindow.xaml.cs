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
    /// Interaction logic for ListParcels.xaml
    /// </summary>
    public partial class ListParcels : Window
    {
        BlApi.IBL bll;
        bool if_selected_changed = false;  //a flag to check if there is any viewing selection
        #region constructor
        public ListParcels(BlApi.IBL bl)
        {
            try
            {
                InitializeComponent();
                bll = bl;
                lsvParcels.ItemsSource = bll.GetParcels();
                cmbStatuses.ItemsSource = Enum.GetValues(typeof(ParcelStatuses));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
        #region buttons and clicks events
        /// <summary>
        /// view of the list with grouping by senders of parcels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupingBySender(object sender, RoutedEventArgs e)
        {
            try
            {
                if_selected_changed = true;
                cmbStatuses.SelectedIndex = -1;
                rdbByTarget.IsChecked = false;
                lsvParcels.ItemsSource = bll.GetParcels();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvParcels.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("NameSender");
                view.GroupDescriptions.Add(groupDescription);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// view of the list with grouping by targets of parcels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupingByTarget(object sender, RoutedEventArgs e)
        {
            try
            {
                if_selected_changed = true;
                cmbStatuses.SelectedItem = -1;
                rdbBySender.IsChecked = false;
                lsvParcels.ItemsSource = bll.GetParcels();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvParcels.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("NameTarget");
                view.GroupDescriptions.Add(groupDescription);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// change the view according to the value in the combo box of Statuses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbStatuses.SelectedIndex != -1)
                {
                    rdbBySender.IsChecked = false;
                    rdbByTarget.IsChecked = false;
                    lsvParcels.ItemsSource = bll.GetParcels().ToList().FindAll(p => p.Status == (ParcelStatuses)cmbStatuses.SelectedItem);
                    if_selected_changed = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new AddParcelWindow(bll).ShowDialog();
                //when you back to this window, show the updated last view
                if (if_selected_changed)
                {
                    if (cmbStatuses.SelectedIndex != -1)
                        lsvParcels.ItemsSource = bll.GetParcels().ToList().FindAll(p => p.Status == (ParcelStatuses)cmbStatuses.SelectedItem);
                    else
                     if (rdbBySender.IsChecked == true)
                        this.GroupingBySender(sender, e);
                    else
                        if (rdbByTarget.IsChecked == true)
                        this.GroupingByTarget(sender, e);
                }
                else
                    lsvParcels.ItemsSource = bll.GetParcels();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void ShowThisParcel(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!lsvParcels.Items.IsEmpty)
                {
                    Parcel p = bll.GetParcel(((ParcelForList)lsvParcels.SelectedItem).Id);
                    new AddParcelWindow(bll, p).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void btbRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lsvParcels.ItemsSource = bll.GetParcels();
                cmbStatuses.SelectedIndex = -1;
                rdbBySender.IsChecked = false;
                rdbByTarget.IsChecked = false;
                if_selected_changed = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
    }
}
