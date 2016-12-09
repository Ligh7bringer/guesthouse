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

namespace assessment2_cs.Windows
{
    /// <summary>
    /// Interaction logic for AddGuestWindow.xaml
    /// </summary>
    public partial class AddGuestWindow : Window
    {
        int bref = 0;
        public AddGuestWindow(int b_)
        {
            bref = b_;
            InitializeComponent();
        }

        Booking b = new Booking();
        List<Booking> bookings = new List<Booking>();
        Guest g = new Guest();

        private void btn_addguest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (b == null)
                {
                    ArgumentException ex = new ArgumentNullException("b is null?");
                    throw ex;                        
                }
                if(g == null)
                {
                    ArgumentException ex = new ArgumentNullException("g is null?");
                    throw ex;
                }
                bookings = b.GetBookings();
                if (bookings == null)
                {
                    InvalidOperationException ex = new InvalidOperationException("bookings is null after getting booking?");
                    throw ex;
                }
                b = bookings.Find(x => x.RefNum == bref);
                if(b == null)
                {
                    InvalidOperationException ex = new InvalidOperationException("b is null after searching?");
                    throw ex;
                }
                g.BookingRef = bref;
                g.Name = txtbx_guestname.Text.ToString();
                g.PassportNo = txtbx_guestpass.Text.ToString();
                g.Age = Convert.ToInt32(txtbx_guestage.Text);            
                b.AddGuest(g);
                g.AddToDB();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Guest added succesfully.");
            MessageBoxResult result = MessageBox.Show("Would you like to add any more guests?", "Add more guests", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            { 
                AddGuestWindow addGuestsWin = new AddGuestWindow(bref);
                this.Close();
                addGuestsWin.ShowDialog();
            }
            else
            {
                this.Close();
            }
        }
    }
}
