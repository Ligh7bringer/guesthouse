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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace assessment2_cs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_addbooking_Click(object sender, RoutedEventArgs e)
        {
            AddBookingWindow bWindow = new AddBookingWindow();
            bWindow.ShowDialog();
        }

        private void btn_amendcust_Click(object sender, RoutedEventArgs e)
        {
            AmendCustomerWindow amendCust = new AmendCustomerWindow();
            amendCust.ShowDialog();
        }

        private void btn_addcust_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerWindow addCust = new AddCustomerWindow();
            addCust.ShowDialog();
        }

        private void btn_amenbooking_Copy_Click(object sender, RoutedEventArgs e)
        {
            AmendBookingWindow amendBooking = new AmendBookingWindow();
            amendBooking.ShowDialog();
        }

        private void btn_addextras_Click(object sender, RoutedEventArgs e)
        {
            AddExtrasWindow addExtrasWin = new AddExtrasWindow(0);
            addExtrasWin.ShowDialog();
        }

        private void btn_amendextras_Click(object sender, RoutedEventArgs e)
        {
            AmendExtrasWindow amendExtrasWind = new AmendExtrasWindow();
            amendExtrasWind.ShowDialog();
        }
    }
}
