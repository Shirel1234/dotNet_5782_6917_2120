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
        
        private void GroupingBySender(object sender, RoutedEventArgs e)
        {
            try
            {
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

        private void GroupingByTarget(object sender, RoutedEventArgs e)
        {
            try
            {
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

        private void cmbStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lsvParcels.ItemsSource = bll.GetParcels().ToList().FindAll(p => p.Status == (ParcelStatuses)cmbStatuses.SelectedItem);
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
                Parcel p = bll.GetParcel(((ParcelForList)lsvParcels.SelectedItem).Id);
                new AddParcelWindow(bll, p).ShowDialog();
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
                rdbBySender.IsChecked = false;
                rdbByTarget.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
    }
}
