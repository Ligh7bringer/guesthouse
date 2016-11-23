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


namespace cw2_csharp
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

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
           //TO DO
        }

        private void cbox_cust_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Svetlozar Georgiev\Desktop\assessment2-cs\database\DB.mdf;Integrated Security=True;Connect Timeout=30");
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
    }
}
