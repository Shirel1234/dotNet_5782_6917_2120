using BlApi;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        public Login()
        {
            InitializeComponent();
        }

        private void btnPressCustomer_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(bl).ShowDialog();
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            // new AddCustomer(bl).ShowDialog();
        }
    }
}

