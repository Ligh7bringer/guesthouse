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
using assessment2_cs.Classes;

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

        Customer c = new Customer();
        List<Customer> customers = new List<Customer>();

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

        int refnum;
        private void cbox_cust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                customers = c.GetCustomers();
                int search = cbox_cust.FindId();
                c = customers.Find(x => x.Refnumber == search);
                this.txtbox_name.Text = c.Name;
                this.txtbx_address.Text = c.Address;
                refnum = c.Refnumber;
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (cbox_cust.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }
            try
            {
                customers = c.GetCustomers();
                c = customers.Find(x => x.Refnumber == refnum);
                c.Name = txtbox_name.Text;
                c.Address = txtbx_address.Text;
                c.UpdateCustomer();              
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
                return;
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Details for user successfully amended.");
            this.Close();
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {   
            if (cbox_cust.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }
            try
            {
                customers = c.GetCustomers();
                c = customers.Find(x => x.Refnumber == refnum);
                c.RemoveFromDB();
            }
            catch (SqlException)
            {
                MessageBox.Show("The selected customer has a booking associated with them and cannot be deleted.");
                return;
            }
            MessageBox.Show("Customer successfully deleted.");
            this.Close();
        }
    }
}
