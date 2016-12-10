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
        int reference = 0;
        public AddExtrasWindow(int t, int bref_)
        {
            if(t == 1)
            {
                test = 1; 
            }
            if(bref_ != 0)
            {
                reference = bref_;
            }
            InitializeComponent();
        }

        Booking b = new Booking();
        List<Booking> bookings = new List<Booking>();
       
        private void cbox_booking_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cbox_booking.LoadBookings();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (test == 1)
            {
                cbox_booking.SelectedIndex = cbox_booking.Items.Count - 1;
            }
            if(reference != 0)
            {
                int index = cbox_booking.FindSubStringIndex(reference.ToString());
                cbox_booking.SelectedIndex = index;
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
            int bref = cbox_booking.FindId();
            if (cbox_type.SelectedIndex == 0 || cbox_type.SelectedIndex == 1)
            {
                try
                {
                    Meal meal = new Meal();
                    meal.Type = cbox_type.SelectedValue.ToString();
                    meal.DietReq = txtbox_mealreq.Text;
                    meal.BookingRef = bref;
                    b.AddExtra(meal);
                    meal.AddToDB();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch(ArgumentException ex)
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
                    if (carhire.StartDate.Date < b.ArrivalDate.Date)
                    {
                        MessageBox.Show("Car hire start date must be later than the booking's arrival date.");
                        return;
                    }          
                    if (carhire.EndDate.Date > b.DepartDate.Date)
                    {
                        MessageBox.Show("Car hire end date must be earlier than the booking's departure date.");
                        return;
                    }
                    b.AddExtra(carhire);
                    carhire.AddToDB();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter valid values for start and end date.");
                    return;
                }
                MessageBox.Show("Extra added successfully.");             
            }
            MessageBoxResult result = MessageBox.Show("Would you like to add any more extras?", "Add more extras", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                AddExtrasWindow addExtras = new AddExtrasWindow(0, bref);
                this.Close();
                addExtras.ShowDialog();
            }
            else
            {
                this.Close();
            }
        }

        private void cbox_booking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int bref = cbox_booking.FindId();
            bookings = b.GetBookings();
            b = bookings.Find(x => x.RefNum == bref);
        }
    }
}
