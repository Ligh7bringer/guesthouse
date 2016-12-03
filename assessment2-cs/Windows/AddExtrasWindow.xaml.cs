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
    /// Interaction logic for AddExtrasWindow.xaml
    /// </summary>
    public partial class AddExtrasWindow : Window
    {
        int test = 0;
        public AddExtrasWindow(int t)
        {
            if(t == 1)
            {
                test = 1; 
            }
            InitializeComponent();
        }

        Booking b = new Booking();
        List<Booking> bookings = new List<Booking>();
        int search = 0;

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
            if (test == 1)
            {
                cbox_booking.SelectedIndex = cbox_booking.Items.Count - 1;
            }
        }

        private void cbox_type_Loaded(object sender, RoutedEventArgs e)
        {
            cbox_type.Items.Add("Evening meals");
            cbox_type.Items.Add("Breakfast meals");
            cbox_type.Items.Add("Car hire");
        }

        private void cbox_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbl_mealreq.Visibility = System.Windows.Visibility.Hidden;
            txtbox_mealreq.Visibility = System.Windows.Visibility.Hidden;
            lbl_driver.Visibility = System.Windows.Visibility.Hidden;
            lbl_endd.Visibility = System.Windows.Visibility.Hidden;
            lbl_sdate.Visibility = System.Windows.Visibility.Hidden;
            txtbox_driver.Visibility = System.Windows.Visibility.Hidden;
            txtbox_startdate.Visibility = System.Windows.Visibility.Hidden;
            txtbox_enddate.Visibility = System.Windows.Visibility.Hidden;

            if (cbox_type.SelectedIndex == 0 || cbox_type.SelectedIndex == 1)
            {
                lbl_mealreq.Visibility = System.Windows.Visibility.Visible;
                txtbox_mealreq.Visibility = System.Windows.Visibility.Visible;
            }
            else if (cbox_type.SelectedIndex == 2)
            {
                lbl_driver.Visibility = System.Windows.Visibility.Visible;
                lbl_endd.Visibility = System.Windows.Visibility.Visible;
                lbl_sdate.Visibility = System.Windows.Visibility.Visible;
                txtbox_driver.Visibility = System.Windows.Visibility.Visible;
                txtbox_startdate.Visibility = System.Windows.Visibility.Visible;
                txtbox_enddate.Visibility = System.Windows.Visibility.Visible;
            }
            
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            int bref = Convert.ToInt32(cbox_booking.Text.ToString().Split(new char[] { ':' })[0]);
            if (cbox_type.SelectedIndex == 0 || cbox_type.SelectedIndex == 1)
            {
                MessageBox.Show(bref.ToString());
                try
                {
                    Meal meal = new Meal();
                    meal.Type = cbox_type.SelectedValue.ToString();
                    meal.DietReq = txtbox_mealreq.Text;
                    meal.BookingRef = bref;
                    MessageBox.Show(meal.BookingRef.ToString());
                    meal.AddToDB();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                MessageBox.Show("Extra added successfuly");
                this.Close();
            }
            else if (cbox_type.SelectedIndex == 2)
            {
                try
                {
                    CarHire carhire = new CarHire();
                    carhire.StartDate = Convert.ToDateTime(txtbox_startdate.Text);
                    carhire.EndDate = Convert.ToDateTime(txtbox_enddate.Text);
                    carhire.Driver = txtbox_driver.Text;
                    carhire.BookingRef = bref;
                    carhire.AddToDB();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                MessageBox.Show("Extra added successfully.");
                this.Close();
            }
        }

        private void cbox_booking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //asd
        }
    }
}
