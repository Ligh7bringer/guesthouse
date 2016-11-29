using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace assessment2_cs
{
    /// <summary>
    /// Interaction logic for AmendBookingWindow.xaml
    /// </summary>
    public partial class AmendBookingWindow : Window
    {
        public AmendBookingWindow()
        {
            InitializeComponent();
        }

        Booking b = new Booking();       
        List<Booking> bookings = new List<Booking>();        
        Customer c = new Customer();

        private void cbox_booking_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bookings = b.GetBookings();
                for (int i = 0; i < bookings.Count; i++)
                {
                    cbox_booking.Items.Add(bookings[i].ToString());
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbox_booking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bookings = b.GetBookings();
            string s = cbox_booking.SelectedValue.ToString();
            var result = s.Split(new char[] { ' ', '-' });
            foreach (var search in bookings)
            {
                if(search.GetCustomerName() == result[0] + " " + result[1] && search.ArrivalDate.ToString("dd/MM/yyyy") == result[2] && search.DepartDate.ToString("dd/MM/yyyy") == result[3])
                {
                    b = search;
                }
            }
            txtbox_arrivald.Text = b.ArrivalDate.ToString("dd/MM/yyyy");
            txtbx_dapartd.Text = b.DepartDate.ToString("dd/MM/yyyy");
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                b.ArrivalDate = Convert.ToDateTime(txtbox_arrivald.Text);
                b.DepartDate = Convert.ToDateTime(txtbx_dapartd.Text);
                b.Update();
            } 
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Booking successfully amended.");
            this.Close();
        }
    }
}
