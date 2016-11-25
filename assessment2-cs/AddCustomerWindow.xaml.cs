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
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            int result;
            DbConnection con = new DbConnection();
            con.OpenConnection();
            try
            {
                string query = "INSERT INTO customer(name, address) VALUES (@name, @address)";
                SqlCommand com = new SqlCommand(query, con.Con);
                com.Parameters.AddWithValue("name", txtbx_name.Text);
                com.Parameters.AddWithValue("address", txtbx_address.Text);
                result = com.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
            }
            finally
            {
                MessageBox.Show("Customer added successfully.");
                con.CloseConnection();
            }

            /*if (result != 0)
            {
                MessageBox.Show("Customer added successfully.");
            } else
            {
                MessageBox.Show("Something went wrong");
            } */
            this.Close();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
