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
    /// Interaction logic for AddParcelWindow.xaml
    /// </summary>
    public partial class AddParcelWindow : Window
    {
        BlApi.IBL bll;
        Parcel newParcel;
        public AddParcelWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bll = bl;
            newParcel = new();
            DataContext = newParcel;
        }
        public AddParcelWindow(BlApi.IBL bl, ParcelForList p)
        {
            InitializeComponent();
            bll = bl;
            //newParcel = new Parcel { CodeParcel = p.Id, SenderCustomer = bll.GetParcels().ToList().Find(parcel => parcel.NameSender == p.NameSender) };
            DataContext = newParcel;

        }
    }
}
