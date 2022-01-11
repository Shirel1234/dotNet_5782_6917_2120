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
        bool if_selected_changed = false;
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

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                new AddDroneWindow(bll).ShowDialog();
                if (this.if_selected_changed)
                {
                    if ((cmbWeight.SelectedIndex != -1))
                        lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
                    if ((cmbStatuses.SelectedIndex != -1))
                        lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
                }
                else
                    lsvDrones.ItemsSource = bll.GetDrones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void OpenShowDrone(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Drone d = bll.GetDrone(((DroneForList)lsvDrones.SelectedItem).Id);
                new AddDroneWindow(bll, d).ShowDialog();
                if (this.if_selected_changed)
                {
                    if((cmbWeight.SelectedIndex != -1))
                        lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
                    if((cmbStatuses.SelectedIndex != -1))
                      lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
                }
                else
                    lsvDrones.ItemsSource = bll.GetDrones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void GroupingByStatus(object sender, RoutedEventArgs e)
        {
            try
            {
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

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbWeight.SelectedIndex = -1;
                cmbStatuses.SelectedIndex = -1;
                lsvDrones.ItemsSource = bll.GetDrones();
                rdbByStatus.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
    }
}
