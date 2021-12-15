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
    /// Interaction logic for AddDrone.xaml
    /// </summary>
    public partial class AddDrone : Window
    {
        IBL.IBl bl;
        private IBL.BO.Drone drone;
        public AddDrone(IBL.IBl bll)
        {
            InitializeComponent();
            bl = bll;
            cmbWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            cmbFreeStations.ItemsSource = bl.GetAllBaseStationsWithChargePositions();
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            drone = new IBL.BO.Drone();
            if (IsFillObject())
                bl.AddDrone(drone,Convert.ToInt32(cmbFreeStations.SelectedValue));
        }
        private bool IsFillObject()
        {
            bool valid = true;
            try
            {
                drone.Id = Convert.ToInt32(txtIdDrone.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                valid = false;
            }
            try
            {
                drone.ModelDrone = txtModelDrone.Text.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                valid = false;
            }
            drone.MaxWeight = (IBL.BO.WeightCategories)Convert.ToInt32(cmbWeight.SelectedValue);
            return valid;
        }
    }
}
