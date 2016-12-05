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
using assessment2_cs.Classes;

namespace assessment2_cs.Windows
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window
    {
        public InvoiceWindow()
        {
            InitializeComponent();
        }

        Booking b = new Booking();
        List<Booking> bookings = new List<Booking>();


        private void cbox_booking_Loaded(object sender, RoutedEventArgs e)
        {
            cbox_booking.LoadBookings();
        }

        private void cbox_booking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bookings = b.GetBookings();
            int id = cbox_booking.FindId();
            b = bookings.Find(x => x.RefNum == id);
            int nights = b.GetNights();
            double cost = b.GenerateInvoice();
            lbl_invoice.Content = "Extras: \n";
            if(b.Extras.Count == 0 )
            {
                lbl_invoice.Content = "This booking doesn't have any extras.\n";
            }
            foreach (var extra_ in b.Extras)
            {
                if (extra_.Type == "Evening meals" || extra_.Type == "Breakfast meals")
                {
                    double extracost = extra_.Cost * (b.Guests.Count + 1) * nights;     
                    lbl_invoice.Content += extra_.Type + ": £" + extracost + "\n";
                }
                else if(extra_.Type == "Car hire")
                {
                    double extracost = extra_.Cost * extra_.GetDays();
                    lbl_invoice.Content += extra_.Type + ": £" + extracost + "\n";
                }
            }
            lbl_invoice.Content += "\nTotal cost per night: £" + cost / nights;
            lbl_invoice.Content += "\n-------------------------";
            lbl_invoice.Content += "\nTotal cost: £" + cost;
        }
    }
}
