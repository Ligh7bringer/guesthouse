using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2_cs.Classes
{
    class CarHire : Extra
    {
        private DateTime startdate;
        private DateTime enddate;
        private string driver;
        DbConnection con = new DbConnection();

        public DateTime StartDate
        {
            get { return startdate; }
            set { startdate = value; }
        }

        public DateTime EndDate
        {
            get { return enddate; }
            set { enddate = value; }
        }

        public string Driver
        {
            get { return driver; }
            set { driver = value; }
        }

        public CarHire()
        {
            this.Type = "Car hire";
        }

        public override void AddToDB()
        {
            string query = "INSERT INTO extra_carhires (start_date, end_date, bookingref, driver) VALUES (@startd, @endd, @bref, @driver)";
            try
            {
                con.OpenConnection();
                SqlCommand qInsert = new SqlCommand(query, con.Con);
                qInsert.Parameters.AddWithValue("startd", startdate);
                qInsert.Parameters.AddWithValue("endd", enddate);
                qInsert.Parameters.AddWithValue("bref", this.BookingRef);
                qInsert.Parameters.AddWithValue("driver", driver);
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

        public override void Update()
        {
            string query = "UPDATE extra_carhires SET start_date=@stdate, end_date=@enddate, driver=@driver WHERE id=@id";
            con.OpenConnection();
            try
            {
                SqlCommand qUpdate = new SqlCommand(query, con.Con);
                qUpdate.Parameters.AddWithValue("stdate", startdate);
                qUpdate.Parameters.AddWithValue("enddate", enddate);
                qUpdate.Parameters.AddWithValue("driver", driver);
                qUpdate.Parameters.AddWithValue("id", this.Id);
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
            
        public override int GetDays()
        {
            return (enddate.Date - startdate.Date).Days;
        }
    }
}
