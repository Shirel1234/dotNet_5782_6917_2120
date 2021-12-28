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
        public DroneListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bll = bl;
            cmbStatuses.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            cmbWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            lsvDrones.ItemsSource= bll.GetDrones();
        }

        private void cmbStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
        }

        private void cmbWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new AddDroneWindow(bll).ShowDialog();
            lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
            lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
        }

        private void OpenShowDrone(object sender, MouseButtonEventArgs e)
        {
            Drone d = bll.GetDrone(((DroneForList)lstDroneListView.SelectedItem).Id);
            new AddDroneWindow(bll, d).ShowDialog();
            lstDroneListView.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeightSelector.SelectedItem));
            lstDroneListView.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatusSelector.SelectedItem));
        }

        private void lstDroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
