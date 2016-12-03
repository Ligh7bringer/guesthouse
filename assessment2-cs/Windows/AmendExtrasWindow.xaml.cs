using assessment2_cs.Classes;
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
    /// Interaction logic for AmendExtrasWindow.xaml
    /// </summary>
    public partial class AmendExtrasWindow : Window
    {
        public AmendExtrasWindow()
        {
            InitializeComponent();
        }

        Booking b = new Booking();
        List<Booking> bookings = new List<Booking>();
       

        private void cbox_booking_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bookings = b.GetBookings();
                foreach (var booking in bookings)
                {
                    cbox_booking.Items.Add(booking.ToString());
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbox_booking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int search = 0;
            string s = cbox_booking.SelectedValue.ToString();
            var result = s.Split(new char[] { ':' });
            search = Convert.ToInt32(result[0]);
            b = b.GetBookings().Find(x => x.RefNum == search);
            //foreach (var tmp in b.Extras)
            //{
            //    cbox_meals.Items.Add(tmp.Type);
           // }
        }
    }
}
