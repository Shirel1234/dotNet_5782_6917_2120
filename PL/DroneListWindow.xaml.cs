using IBL.BO;
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
        IBL.IBl bl;
        public DroneListWindow(IBL.IBl bll)
        {
            InitializeComponent();
            bl = bll;
            DroneListView.ItemsSource = bl.GetDrones();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new AddDrone(bl).ShowDialog();
        }

        private void btnCloseDrones_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneListView.ItemsSource=bl.GetDronesByStatus((int)StatusSelector.SelectedItem);
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneListView.ItemsSource = bl.GetDronesByWeight((int)WeightSelector.SelectedItem);

        }

        private void openDroneActive(object sender, MouseButtonEventArgs e)
        {
            new DroneActive(bl).ShowDialog();
        }
    }
}
