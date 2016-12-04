using assessment2_cs.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TO DO: CLASS DESCRIPTION 
namespace assessment2_cs
{
    class Booking
    {
        private int refnum;
        private DateTime arrivaldate;
        private DateTime departdate;
        private int bookingref;
        private int custref;
        private Customer hasCustomer;
        private List<Guest> guests = new List<Guest>();
        private List<Extra> extras = new List<Extra>();
        private DbConnection con = new DbConnection();

        public Booking() { }

        public List<Extra> Extras
        {
            get { return extras; }
            set { extras = value; }
        }

        public Booking(DateTime arrd, DateTime depd)
        {
            arrivaldate = arrd;
            departdate = depd;
        }

        public Booking(DateTime arrd, DateTime depd, Customer c) : this(arrd, depd)
        {
            this.hasCustomer = c;
        }

        public List<Guest> Guests
        {
            set { guests = value; }
            get { return guests; }
        }

        public int RefNum
        {
            get { return refnum; }
            set { refnum = value; }
        }

        public DateTime ArrivalDate
        {
            get { return arrivaldate; }
            set { arrivaldate = value; }
        }

        public DateTime DepartDate
        {
            get { return departdate; }
            set { departdate = value; }
        }

        public int BookingRef
        {
            get { return bookingref; }
            set { bookingref = value; }
        }

        public int CustRef
        {
            get { return custref; }
            set { custref = value; }
        }

        public void AddToDB()
        {
            string query = "INSERT INTO booking (arrival_date, departure_date, cust_ref) OUTPUT Inserted.reference_num VALUES (@arrivald, @departd, @cust_ref)";
            con.OpenConnection();
            try
            {
                SqlCommand qInsert = new SqlCommand(query, con.Con);
                qInsert.Parameters.AddWithValue("arrivald", arrivaldate);
                qInsert.Parameters.AddWithValue("departd", departdate);
                qInsert.Parameters.AddWithValue("cust_ref", hasCustomer.Refnumber);
                var id = qInsert.ExecuteScalar();
                foreach (Guest guest in guests)
                {
                    guest.BookingRef = Convert.ToInt32(id);
                    guest.AddToDB();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public void AddCustomer(Customer c)
        {
            this.hasCustomer = c;
        }

        public void AddGuest(Guest g)
        {
            if (guests.Count > 3)
            {
                ArgumentException ex = new ArgumentException("Only 4 guests are allowed per booking");
                throw ex;
            }
            this.guests.Add(g);
        }

        public string GetCustomerName()
        {
            return this.hasCustomer.Name;
        }

        public List<Booking> GetBookings()
        {
            List<Booking> bookings = new List<Booking>();
            string query = "SELECT booking.reference_num, arrival_date, departure_date, cust_ref, name, address FROM booking JOIN customer ON booking.cust_ref=customer.reference_num";
            try
            {
                con.OpenConnection();
                SqlDataReader sdr = con.DataReader(query);
                while (sdr.Read())
                {
                    Booking b = new Booking();
                    Customer c = new Customer();
                    Guest g = new Guest();
                    Extra e = new Extra();
                    c.Name = sdr["name"].ToString();
                    c.Address = sdr["address"].ToString();
                    c.Refnumber = Int32.Parse(sdr["cust_ref"].ToString());
                    b.AddCustomer(c);
                    b.ArrivalDate = Convert.ToDateTime(sdr["arrival_date"]);
                    b.DepartDate = Convert.ToDateTime(sdr["departure_date"]);
                    b.RefNum = Int32.Parse(sdr["reference_num"].ToString());                    
                    foreach (var guest in g.GetGuests(b.RefNum))
                    {
                        b.AddGuest(guest);
                    }
                    foreach (var extra_ in e.GetExtras(b.RefNum))
                    {
                        b.AddExtra(extra_);
                    }
                    bookings.Add(b);
                }
                sdr.Close();

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
            return bookings;
        }

        public void Update()
        {
            string query = "UPDATE booking SET arrival_date=@arrd, departure_date=@depd WHERE reference_num=@refnum";
            con.OpenConnection();
            try
            {
                foreach (var guest in guests)
                {
                    guest.UpdateGuest();
                }
                SqlCommand qUpdate = new SqlCommand(query, con.Con);
                qUpdate.Parameters.AddWithValue("arrd", arrivaldate);
                qUpdate.Parameters.AddWithValue("depd", departdate);
                qUpdate.Parameters.AddWithValue("refnum", refnum);
                qUpdate.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public void RemoveFromDB()
        {
            string query = "DELETE FROM booking WHERE reference_num=@refnum";
            con.OpenConnection();
            try
            {
                foreach (var guest in guests)
                {
                    guest.RemoveFromDB();
                }
                SqlCommand qUpdate = new SqlCommand(query, con.Con);
                qUpdate.Parameters.AddWithValue("refnum", refnum);
                qUpdate.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public void AddExtra(Extra e_)
        {
            this.extras.Add(e_);
        }

        public override string ToString()
        {
            return refnum + ": " + hasCustomer.Name + " " + arrivaldate.ToString("dd/MM/yyyy") + "-" + departdate.Date.ToString("dd/MM/yyyy");
        }
    }
}
