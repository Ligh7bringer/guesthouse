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

        Customer c = new Customer();
        List<Customer> customers = new List<Customer>();

        DbConnection con = new DbConnection();
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {  
            Booking b = new Booking();
            try
            {
                customers = c.GetCustomers();
                c = customers.Find(x => x.Name == cbox_cust.SelectedValue.ToString());
                b.CustRef = c.Refnumber;
                b.ArrivalDate = txtbox_arrivald.Text;
                b.DepartDate = txtbx_dapartd.Text;
                b.AddToDB();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
                return;
            }
            finally
            {                
                this.Close();
            }

            MessageBox.Show("Booking added successfully.");
        }

        private void cbox_cust_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                customers = c.GetCustomers();
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
    }
}
