﻿using System;
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
    /// Interaction logic for AmendCustomerWindow.xaml
    /// </summary>
    public partial class AmendCustomerWindow : Window
    {
        public AmendCustomerWindow()
        {
            InitializeComponent();
        }

        DbConnection con = new DbConnection();

        private void cbox_cust_Loaded(object sender, RoutedEventArgs e)
        {
            Customers c = new Customers();
            List<Customer> customers = new List<Customer>();
            try
            {
                customers = c.GetCustomerNames();
                for (int i = 0; i < customers.Count; i++)
                {
                    cbox_cust.Items.Add(customers[i].Name);
                }
            } 
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();                               
            } 
        }

        String refnum;
        private void cbox_cust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            String query = "SELECT * FROM customer WHERE name='" + cbox_cust.SelectedValue + "'";
            con.OpenConnection();
            try
            {
                SqlDataReader sdr = con.DataReader(query);
                while (sdr.Read())
                {
                    this.txtbox_name.Text = sdr["name"].ToString();
                    this.txtbx_address.Text = sdr["address"].ToString();
                    refnum = sdr["reference_num"].ToString();
                }
                sdr.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
            }
            finally
            {
                con.CloseConnection();
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            int result;
            String query = "UPDATE customer SET name=@name, address=@address WHERE reference_num=@refnum";
            con.OpenConnection();
            try
            {
                SqlCommand com = new SqlCommand(query, con.Con);
                com.Parameters.AddWithValue("name", txtbox_name.Text);
                com.Parameters.AddWithValue("address", txtbx_address.Text);
                com.Parameters.AddWithValue("refnum", refnum);          
                result = com.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
                return;
            }
            finally
            {                
                con.CloseConnection();
                this.Close();
            }

            MessageBox.Show("Details for user successfully amended.");
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            String query = "DELETE FROM customer WHERE reference_num=@refnum";
            con.OpenConnection();
            try
            {
                SqlCommand com = new SqlCommand(query, con.Con);
                com.Parameters.AddWithValue("refnum", refnum);            
                com.ExecuteNonQuery();                
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
                return;
            }
            finally
            {
                con.CloseConnection();
                this.Close();
            }

            MessageBox.Show("Customer successfully deleted.");

            /*if (result != 0) // TO DO: AND THEY HAVE NO BOOKINGS
            {
                MessageBox.Show("Customer successfully deleted.");
            }
            else
            {
                MessageBox.Show("Something went wrong.");
            }*/
        }
    }
}
