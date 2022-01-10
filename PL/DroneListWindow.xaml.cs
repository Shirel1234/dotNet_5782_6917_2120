﻿using BO;
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
            InitializeComponent();
            bll = bl;
            cmbStatuses.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            cmbWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            try
            {
                lsvDrones.ItemsSource = bll.GetDrones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void cmbStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
            this.if_selected_changed = true;
        }

        private void cmbWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
            this.if_selected_changed = true;
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new AddDroneWindow(bll).ShowDialog();
            lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
            lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
        }

        private void OpenShowDrone(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Drone d = bll.GetDrone(((DroneForList)lsvDrones.SelectedItem).Id);
                new AddDroneWindow(bll, d).ShowDialog();
                if (this.if_selected_changed)
                {
                    lsvDrones.ItemsSource = bll.GetDronesByWeight(Convert.ToInt32(cmbWeight.SelectedItem));
                    lsvDrones.ItemsSource = bll.GetDronesByStatus(Convert.ToInt32(cmbStatuses.SelectedItem));
                }
                else
                    lsvDrones.ItemsSource = bll.GetDrones();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void GroupingByStatus(object sender, RoutedEventArgs e)
        {
            lsvDrones.ItemsSource = bll.GetDrones();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvDrones.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatus");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            lsvDrones.ItemsSource = bll.GetDrones();
            rdbByStatus.IsChecked = false;
        }
    }
}
