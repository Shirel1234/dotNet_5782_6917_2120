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
            cmbStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            cmbWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            DataContext= bll.GetDrones();

        }

        private void cmbStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstDroneListView.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatusSelector.SelectedItem));
        }

        private void cmbWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstDroneListView.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeightSelector.SelectedItem));
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new AddDroneWindow(bll).ShowDialog();
            lstDroneListView.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeightSelector.SelectedItem));
            lstDroneListView.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatusSelector.SelectedItem));
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
