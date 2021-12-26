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
            InitializeComponent();
            bll = bl;
            lsvParcels.ItemsSource = bll.GetParcels();
            cmbStatuses.ItemsSource = Enum.GetValues(typeof(ParcelStatuses));
        }
        
        private void GroupingBySender(object sender, RoutedEventArgs e)
        {
            lsvParcels.ItemsSource = bll.GetParcels();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lsvParcels.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("NameSender");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void GroupingByTarget(object sender, RoutedEventArgs e)
        {
            listParcels.ItemsSource = bll.GetParcels();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listParcels.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("NameTarget");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void cmbStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lsvParcels.ItemsSource = bll.GetParcels().ToList().FindAll(p=>p.Status== (ParcelStatuses)cmbStatuses.SelectedItem);
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            new AddParcelWindow(bll).ShowDialog();
        }

        private void ShowThisParcel(object sender, MouseButtonEventArgs e)
        {
            new ListParcels(bll, (ParcelForList)lsvParcels.SelectedItem).ShowDialog();
        }
    }
}
