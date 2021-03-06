﻿using System;
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
using System.Data.SqlClient;
using assessment2_cs;
using assessment2_cs.Classes;
using System.Globalization;

namespace assessment2_cs
{
    /// <summary>
    /// Interaction logic for AddBookingWindow.xaml
    /// </summary>
    public partial class AddBookingWindow : Window
    {
        
        public AddBookingWindow()
        {
            InitializeComponent();
        }

        Customer c = new Customer();
        List<Customer> customers = new List<Customer>();
        Booking b = new Booking();
        DbConnection con = new DbConnection();

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                customers = c.GetCustomers();
                int search = cbox_cust.FindId();
                c = customers.Find(x => x.Refnumber == search);
                b.ArrivalDate = Convert.ToDateTime(txtbox_arrivald.Text);
                b.DepartDate = Convert.ToDateTime(txtbx_dapartd.Text);
                b.AddCustomer(c);
                b.AddToDB();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
                return;                
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a customer");
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("Entered dates are in a wrong format. Please use dd/MM/yyyy or yyyy-MM-dd.");
                return;
            }           
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Booking added successfully.");
            MessageBoxResult result = MessageBox.Show("Would you like to add any extras?", "Extras", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                AddExtrasWindow addExtras = new AddExtrasWindow(1, 0);
                this.Close();
                addExtras.ShowDialog();
            }
            else
            {
                this.Close();
            }
        }

        private void cbox_cust_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cbox_cust.LoadCustomers();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void btn_addguest_Click(object sender, RoutedEventArgs e)
        {
            Guest g = new Guest();
            try
            {
                g.Name = txtbx_guestname.Text;
                g.PassportNo = txtbx_guestpass.Text;
                g.Age = Int32.Parse(txtbx_guestage.Text);
                b.AddGuest(g);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a value for the guest's age.");
                return;
            }
            lstbx_guests.Items.Add(g.ToString());
            MessageBox.Show("Guest added successfully.");
            txtbx_guestname.Clear();
            txtbx_guestpass.Clear();
            txtbx_guestage.Clear();
        }
    }
}
