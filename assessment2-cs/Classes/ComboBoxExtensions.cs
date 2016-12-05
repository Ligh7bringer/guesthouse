using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace assessment2_cs.Classes
{
    public static class ComboBoxExtensions
    {
        public static int FindSubStringIndex(this ComboBox combo, string subString, StringComparison comparer = StringComparison.CurrentCulture)
        {
            // Sanity check parameters
            if (combo == null) throw new ArgumentNullException("combo");
            if (subString == null)
            {
                return -1;
            }

            for (int index = 0; index < combo.Items.Count; index++)
            {
                object obj = combo.Items[index];
                if (obj == null) continue;
                string item = Convert.ToString(obj, CultureInfo.CurrentCulture);
                if (string.IsNullOrWhiteSpace(item) && string.IsNullOrWhiteSpace(subString))
                    return index;
                int indexInItem = item.IndexOf(subString, comparer);
                if (indexInItem >= 0)
                    return index;
            }

            return -1;
        }

        public static int FindId(this ComboBox combo)
        {
            // Sanity check parameters
            if (combo == null) throw new ArgumentNullException("combo");

            int search = 0;
            string s = combo.SelectedValue.ToString();
            var result = s.Split(new char[] { ':' });
            search = Convert.ToInt32(result[0]);
            return search;
        }

        public static void LoadBookings(this ComboBox combo)
        {
            Booking b = new Booking();
            List<Booking> bookings = new List<Booking>();
            Customer c = new Customer();
            try
            {
                bookings = b.GetBookings();
                foreach (var booking in bookings)
                {
                    combo.Items.Add(booking.ToString());
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
