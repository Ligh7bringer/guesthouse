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
using System.Data.SqlClient;
using assessment2_cs;
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

        DbConnection con = new DbConnection();

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            con.OpenConnection();
            String refnum = "";
            int result;

            try
            {
                String query = "SELECT * FROM customer WHERE name='" + cbox_cust.SelectedValue + "'";
                string queryInsert = "INSERT INTO booking (arrival_date, departure_date, cust_ref) VALUES (@arrivald, @departd, @cust_ref)";
                SqlDataReader sdr = con.DataReader(query);
                while (sdr.Read())
                {
                    refnum = sdr["reference_num"].ToString();
                }
                sdr.Close();

                SqlCommand qInsert = new SqlCommand(queryInsert, con.Con);
                qInsert.Parameters.AddWithValue("arrivald", txtbox_arrivald.Text);
                qInsert.Parameters.AddWithValue("departd", txtbx_dapartd.Text);
                qInsert.Parameters.AddWithValue("cust_ref", refnum);

                result = qInsert.ExecuteNonQuery();
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

            MessageBox.Show("Booking added successfully.");
        }

        private void cbox_cust_Loaded(object sender, RoutedEventArgs e)
        {
            String query = "SELECT name FROM customer";
            con.OpenConnection();
            try
            {
                SqlDataReader sdr = con.DataReader(query);
                while (sdr.Read())
                {
                    this.cbox_cust.Items.Add(sdr["name"]);
                }
                con.CloseConnection();
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
    }
}
