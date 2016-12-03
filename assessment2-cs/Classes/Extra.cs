using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace assessment2_cs.Classes
{
    abstract class Extra
    {

        private string type;
        private int bookingref;
        DbConnection con = new DbConnection();
        List<Extra> extras = new List<Extra>();

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        
        public int BookingRef
        {
            get { return bookingref; }
            set { bookingref = value; }
        }

        public virtual void AddToDB()
        {
            // to be overriden by children classes
        }

        public List<Extra> GetExtras(int bref)
        {
            con.OpenConnection();

            string selectGuest = "SELECT * FROM extra_meals; SELECT * FROM extra_carhires";
            try
            {
                SqlDataReader sdr = con.DataReader(selectGuest);
                while (sdr.Read() && Convert.ToInt32(sdr["booking_ref"]) == bref)
                {
                    if(sdr["type"].ToString() == "Evening meals" || sdr["type"].ToString() == "Breakfast meals")
                    {
                        Meal meal = new Meal();
                        meal.Type = sdr["type"].ToString();
                        meal.Id = Convert.ToInt32(sdr["id"]);
                        meal.BookingRef = Convert.ToInt32(sdr["bookingref"]);
                        meal.DietReq = sdr["dietary_requirements"].ToString();
                        extras.Add(meal);
                    }
                    else
                    {
                        CarHire carhire = new CarHire();
                        carhire.Id = Convert.ToInt32(sdr["id"]);
                        carhire.StartDate = Convert.ToDateTime(sdr["start_date"]);
                        carhire.EndDate = Convert.ToDateTime(sdr["end_date"]);
                        carhire.Driver = sdr["driver"].ToString();
                        extras.Add(carhire);
                    }
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
            return extras;
        }
    }
}
