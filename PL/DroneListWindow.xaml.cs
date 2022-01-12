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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        BlApi.IBL bll;
        bool if_selected_changed = false; //a flag to check if there is any viewing selection
        #region constructor
        public DroneListWindow(BlApi.IBL bl)
        {
            try
            {
                InitializeComponent();
                bll = bl;
                cmbStatuses.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
                cmbWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                lsvDrones.ItemsSource = bll.GetDrones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        #endregion
        #region buttons and clicks events
        /// <summary>
        /// change the view according to the value in the combo box of Statuses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
                this.if_selected_changed = true;
                cmbWeight.SelectedIndex = -1; 
                rdbByStatus.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// change the view according to the value in the combo box of Weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
                this.if_selected_changed = true;
                cmbStatuses.SelectedIndex = -1;
                rdbByStatus.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// open the window of adding base drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                new AddDroneWindow(bll).ShowDialog();
                if (this.if_selected_changed)
                {
                    if ((cmbWeight.SelectedIndex != -1))
                        lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
                    else
                        if ((cmbStatuses.SelectedIndex != -1))
                            lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
                        else
                             if (rdbByStatus.IsChecked == true)
                                this.GroupingByStatus(sender, e);
                }
                else
                    lsvDrones.ItemsSource = bll.GetDrones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// enable to open the drone window from the drones list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenShowDrone(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //only if there are items in the list
                if (!lsvDrones.Items.IsEmpty)
                {
                    Drone d = bll.GetDrone(((DroneForList)lsvDrones.SelectedItem).Id);
                    new AddDroneWindow(bll, d).ShowDialog();
                }
                //when you back to this window, show the updated last view
                if (this.if_selected_changed)
                {
                    if((cmbWeight.SelectedIndex != -1))
                        lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
                    else
                        if ((cmbStatuses.SelectedIndex != -1))
                            lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
                        else
                             if (rdbByStatus.IsChecked == true)
                                this.GroupingByStatus(sender, e);
                }
                else
                    lsvDrones.ItemsSource = bll.GetDrones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// view of the list with grouping by statuses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupingByStatus(object sender, RoutedEventArgs e)
        {
            try
            {
                if_selected_changed = true;
                cmbWeight.SelectedIndex = -1;
                cmbStatuses.SelectedIndex = -1; ;
                lsvDrones.ItemsSource = bll.GetDrones();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvDrones.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatus");
                view.GroupDescriptions.Add(groupDescription);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        /// <summary>
        /// refresh the view of the list to be the original
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lsvDrones.ItemsSource = bll.GetDrones();
                //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvDrones.ItemsSource);
                //PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatus");
                //view.GroupDescriptions.Remove(groupDescription);
                cmbWeight.SelectedIndex = -1;
                cmbStatuses.SelectedIndex = -1;
                rdbByStatus.IsChecked = false;
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
