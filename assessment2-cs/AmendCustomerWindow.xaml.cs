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
    /// Interaction logic for AmendCustomerWindow.xaml
    /// </summary>
    public partial class AmendCustomerWindow : Window
    {
        public AmendCustomerWindow()
        {
            InitializeComponent();
        }

        private void cbox_cust_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\database\DB.mdf;Integrated Security=True;Connect Timeout=30");
            con.Open();
            SqlCommand com = new SqlCommand(
                "SELECT name FROM customer", con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                this.cbox_cust.Items.Add(sdr["name"]);
            }
            sdr.Close();
        }

        String refnum;
        private void cbox_cust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            String query = "SELECT * FROM customer WHERE name='" + cbox_cust.SelectedValue + "'";
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\database\DB.mdf;Integrated Security=True;Connect Timeout=30");
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                this.txtbox_name.Text = sdr["name"].ToString();
                this.txtbx_address.Text = sdr["address"].ToString();
                refnum = sdr["reference_num"].ToString();
            }
            sdr.Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            String query = "UPDATE customer SET name=@name, address=@address WHERE reference_num=" + refnum;
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\database\DB.mdf;Integrated Security=True;Connect Timeout=30");
            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.AddWithValue("name", txtbox_name.Text);
            com.Parameters.AddWithValue("address", txtbx_address.Text);
            con.Open();
            int result = com.ExecuteNonQuery();
            con.Close();
            if(result != 0)
            {
                MessageBox.Show("Details for user successfully amended.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Something went wrong.");                
            }
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            String query = "DELETE FROM customer WHERE reference_num='" + refnum + "'";
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\database\DB.mdf;Integrated Security=True;Connect Timeout=30");
            SqlCommand com = new SqlCommand(query, con);
            con.Open();
            int result = com.ExecuteNonQuery();
            con.Close();
            if(result != 0) // TO DO: AND THEY HAVE NO BOOKINGS
            {
                MessageBox.Show("Customer successfully deleted.");
            }
            else
            {
                MessageBox.Show("Something went wrong.");
            }            
            this.Close();
        }
    }
}
