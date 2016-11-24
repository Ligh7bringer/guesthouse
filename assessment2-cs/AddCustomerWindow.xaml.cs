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
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\database\DB.mdf;Integrated Security=True;Connect Timeout=30");
            string sqlQuery = "INSERT INTO customer (name, address) VALUES (@name, @address)";
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.Parameters.AddWithValue("name", txtbx_name.Text);
            cmd.Parameters.AddWithValue("address", txtbx_address.Text);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if(i != 0)
            {
                MessageBox.Show("Customer added successfully.");
            } else
            {
                MessageBox.Show("Something went wrong");
            }
            this.Close();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
