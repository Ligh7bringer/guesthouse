using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2_cs.Classes
{
    // Author: Svetlozar Georgiev
    // Date of last modification: 8/12/2016
    // Purpose: Allows the creation of carhire objects. Implements methods
    // which allow the addition and deletion of a car hire extra from the database
    class CarHire : Extra
    {
        // properties
        private DateTime startdate; // start date of the extra
        private DateTime enddate; // end date of the extra
        private string driver; // name of the driver 
        DbConnection con = new DbConnection(); // connection to the database

        // accessors for the private properties
        public DateTime StartDate
        {
            get { return startdate; }
            set { startdate = value; }
        }

        public DateTime EndDate
        {
            get { return enddate; }
            set
            {
                // we have to make sure end date isn't before start date
                if (value.Date < startdate)
                {
                    ArgumentException ex = new ArgumentException("The end date of a car hire must be later than its start date");
                    throw ex;
                }
                enddate = value;
            }
        }

        public string Driver
        {
            get { return driver; }
            set { driver = value; }
        }

        // when an object of this type is created set it's type to car hire as there isn't any other type of this extra
        public CarHire()
        {
            this.Type = "Car hire";
        }

        // inserts a car hire in the database
        public override void AddToDB()
        {
            // create a string containing the query 
            string query = "INSERT INTO extra_carhires (start_date, end_date, bookingref, driver) VALUES (@startd, @endd, @bref, @driver)";
            try
            {
                con.OpenConnection();
                SqlCommand qInsert = new SqlCommand(query, con.Con); // create a command with the query
                qInsert.Parameters.AddWithValue("startd", startdate); // add actual values in it
                qInsert.Parameters.AddWithValue("endd", enddate);
                qInsert.Parameters.AddWithValue("bref", this.BookingRef);
                qInsert.Parameters.AddWithValue("driver", driver);
                qInsert.ExecuteNonQuery(); // execute the command
            }
            catch (SqlException ex) // check for exceptions and rethrow them
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        // updates the details of a car hire in the database
        public override void Update()
        {
            // create a string containg the query
            string query = "UPDATE extra_carhires SET start_date=@stdate, end_date=@enddate, driver=@driver WHERE id=@id";
            con.OpenConnection();
            try
            {
                SqlCommand qUpdate = new SqlCommand(query, con.Con); // create a command with the query string
                qUpdate.Parameters.AddWithValue("stdate", startdate); // and replace placeholders with actual values
                qUpdate.Parameters.AddWithValue("enddate", enddate);
                qUpdate.Parameters.AddWithValue("driver", driver);
                qUpdate.Parameters.AddWithValue("id", this.Id);
                qUpdate.ExecuteNonQuery(); // execute command
            }
            catch (SqlException ex) // rethrow any exceptions which were caught
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection(); // close the connection
            }
        }
            
        // calculates the dates of a car hire
        // will be used in invoices
        public override int GetDays()
        {
            return (enddate.Date - startdate.Date).Days;
        }
    }
}
