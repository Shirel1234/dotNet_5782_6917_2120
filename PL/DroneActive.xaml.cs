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
    /// Interaction logic for DroneActive.xaml
    /// </summary>
    public partial class DroneActive : Window
    {
        IBL.IBl bl;
        public DroneActive(IBL.IBl bll)
        {
            InitializeComponent();
            bl = bll;
        }
    }
}
