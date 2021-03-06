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
using assessment2_cs.Classes;
using assessment2_cs.Windows;

namespace assessment2_cs
{

    /// TODO: Add a guest to an existing booking
    public partial class AmendBookingWindow : Window
    {
        int bref = 0;
        public AmendBookingWindow(int bref_)
        {
            if (bref_ != 0)
            {
                bref = bref_;
            }
            InitializeComponent();
        }

        Booking b = new Booking();
        List<Booking> bookings = new List<Booking>();
        Customer c = new Customer();

        private void cbox_booking_Loaded(object sender, RoutedEventArgs e)
        { 
            try
            {
                cbox_booking.LoadBookings();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            if(bref != 0)
            {
                int index = cbox_booking.FindSubStringIndex(bref.ToString());
                cbox_booking.SelectedIndex = index;
            }
        }

        private int search;
        private void cbox_booking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtbox_age.Clear();
            txtbox_guestname.Clear();
            txtbx_passno.Clear();
            cbox_guest.Items.Clear();

            search = cbox_booking.FindId();
            b = b.GetBookings().Find(x => x.RefNum == search);
            txtbox_arrivald.Text = b.ArrivalDate.ToString("dd/MM/yyyy");
            txtbx_dapartd.Text = b.DepartDate.ToString("dd/MM/yyyy");
            lbl_numofguests.Content = "Selected booking has " + b.Guests.Count + " guest(s).";            
            foreach (Guest tmp in b.Guests)
            {
                cbox_guest.Items.Add(tmp.Name);
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Guest guest_ in RemovedGuests)
                {
                    guest_.RemoveFromDB();
                }
                b.ArrivalDate = Convert.ToDateTime(txtbox_arrivald.Text);
                b.DepartDate = Convert.ToDateTime(txtbx_dapartd.Text);
                b.Update();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("Dates supplied are in a wrong format. Please use dd/MM/yyyy or yyyy-MM-dd");
                return;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Booking successfully amended.");
            this.Close();
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if (cbox_booking.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a booking.");
                return;
            }
            try
            {
                b.RemoveFromDB();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
                return;
            }
            MessageBox.Show("Booking successfully removed.");
            this.Close();
        }

        Guest selected = new Guest();
        private void cbox_guest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbox_guest.SelectedIndex == -1)
            {
                return;
            }
            selected = b.Guests.Find(x => x.Name == cbox_guest.SelectedValue.ToString());
            txtbox_guestname.Text = selected.Name;
            txtbx_passno.Text = selected.PassportNo;
            txtbox_age.Text = selected.Age.ToString();
        }

        private void btn_amendguest_Click(object sender, RoutedEventArgs e)
        {
            if (cbox_guest.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a guest.");
            }
            try
            {
                b.Guests.Remove(selected);
                cbox_guest.SelectedIndex = -1;
                cbox_guest.Items.Remove(selected.Name);
                selected.Name = txtbox_guestname.Text;
                selected.PassportNo = txtbx_passno.Text;
                selected.Age = Convert.ToInt32(txtbox_age.Text);
                b.Guests.Add(selected);
                cbox_guest.Items.Add(selected.Name);
                cbox_guest.Items.Refresh();
                txtbox_age.Clear();
                txtbox_guestname.Clear();
                txtbx_passno.Clear();
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
            MessageBox.Show("Guest details successfuly amended.");
        }

        List<Guest> RemovedGuests = new List<Guest>();
        private void btn_removeguest_Click(object sender, RoutedEventArgs e)
        {
            if (cbox_guest.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a guest");
                return;
            }
            b.Guests.Remove(selected);
            cbox_guest.SelectedIndex = -1;
            cbox_guest.Items.Remove(selected.Name);
            cbox_guest.Items.Refresh();
            RemovedGuests.Add(selected);
            txtbox_age.Clear();
            txtbox_guestname.Clear();
            txtbx_passno.Clear();
        }

        private void btn_addguest_Click(object sender, RoutedEventArgs e)
        {
            if (b.Guests.Count == 4)
            {
                MessageBox.Show("This booking already has 4 guests.");
                return;
            }
            if (search == 0)
            {
                MessageBox.Show("Please select a booking.");
                return;
            }
            AddGuestWindow addGuestWin = new AddGuestWindow(search);
            this.Close();
            addGuestWin.ShowDialog();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}