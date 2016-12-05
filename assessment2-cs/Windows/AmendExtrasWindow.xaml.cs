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
                cbox_booking.LoadBookings();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbox_booking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int search = cbox_booking.FindId();
            b = b.GetBookings().Find(x => x.RefNum == search);
            
            foreach (var extra_ in b.Extras)
            {
                if (extra_.Type != "Car hire")
                {
                    cbox_meals.Items.Add(extra_.Type);
                }
                 else
                {
                    cbox_carhires.Items.Add(extra_.Type);                    
                }
            }
            lbl_extrasnum.Content = "This booking has " + b.Extras.Count + " extra(s).";
        }

        Meal meal = new Meal();
        private void cbox_meals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbox_meals.SelectedIndex == -1)
            {
                return;
            }
            meal =  (Meal)b.Extras.Find(x => x.Type == cbox_meals.SelectedValue.ToString());
            txtbox_dietaryreqs.Text = meal.DietReq;
        }

        CarHire carhire = new CarHire();
        private void cbox_carhires_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbox_carhires.SelectedIndex == -1)
            {
                return;
            }
            carhire = (CarHire)b.Extras.Find(x => x.Type == cbox_carhires.SelectedValue.ToString());
            txtbox_stdate.Text = carhire.StartDate.ToString("dd/MM/yyyy");
            txtbox_enddate.Text = carhire.EndDate.ToString("dd/MM/yyyy");
            txtbox_driver.Text = carhire.Driver;
        }

        List<Meal> DeletedMeals = new List<Meal>();
        private void btn_delmeal_Click(object sender, RoutedEventArgs e)
        {
            b.Extras.Remove(meal);
            cbox_meals.SelectedIndex = -1;
            cbox_meals.Items.Remove(meal.Type);
            cbox_meals.Items.Refresh();
            DeletedMeals.Add(meal);
            txtbox_dietaryreqs.Clear();
        }

        List<CarHire> DeletedCarhires = new List<CarHire>();
        private void btn_delcarhire_Click(object sender, RoutedEventArgs e)
        {
            b.Extras.Remove(carhire);
            cbox_carhires.SelectedIndex = -1;
            cbox_carhires.Items.Remove(carhire.Type);
            cbox_carhires.Items.Refresh();
            DeletedCarhires.Add(carhire);
            txtbox_driver.Clear();
            txtbox_enddate.Clear();
            txtbox_stdate.Clear();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var meal_ in DeletedMeals)
                {
                    meal_.RemoveFromDB();
                }
                foreach (var carhire_ in DeletedCarhires)
                {
                    carhire_.RemoveFromDB();
                }
                foreach (var extra_ in b.Extras)
                {
                    extra_.Update();
                }
            } 
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Extra(s) successfully amended.");
            this.Close();
        }

        private void btn_amendcarhire_Click(object sender, RoutedEventArgs e)
        {
            b.Extras.Remove(carhire);
            carhire.StartDate = Convert.ToDateTime(txtbox_stdate.Text);
            carhire.EndDate = Convert.ToDateTime(txtbox_enddate.Text);
            carhire.Driver = txtbox_driver.Text;
            b.AddExtra(carhire);
            cbox_carhires.SelectedIndex = -1;
            txtbox_stdate.Clear();
            txtbox_enddate.Clear();
            txtbox_driver.Clear();
            MessageBox.Show("Details of extra successfully amended.");
        }

        private void btn_amendmeal_Click(object sender, RoutedEventArgs e)
        {
            b.Extras.Remove(meal);
            meal.DietReq = txtbox_dietaryreqs.Text;
            b.AddExtra(meal);
            cbox_meals.SelectedIndex = -1;
            txtbox_dietaryreqs.Clear();
            MessageBox.Show("Details of extra successfully amended.");
        }
    }
}
