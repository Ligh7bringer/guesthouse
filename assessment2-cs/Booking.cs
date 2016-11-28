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
        private String arrivaldate;
        private String departdate;
        private int bookingref;
        private int custref;
        private DbConnection con = new DbConnection();
        
        public String ArrivalDate
        {
            get { return arrivaldate; }
            set { arrivaldate = value; }
        }

        public String DepartDate
        {
            get { return departdate; }
            set
            {
                departdate = value; // parse to date ??
            }
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
            string query = "INSERT INTO booking (arrival_date, departure_date, cust_ref) VALUES (@arrivald, @departd, @cust_ref)";
            con.OpenConnection();
            try
            {
                SqlCommand qInsert = new SqlCommand(query, con.Con);
                qInsert.Parameters.AddWithValue("arrivald",arrivaldate);
                qInsert.Parameters.AddWithValue("departd", departdate);
                qInsert.Parameters.AddWithValue("cust_ref", custref);
                qInsert.ExecuteNonQuery();
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
    }
}


