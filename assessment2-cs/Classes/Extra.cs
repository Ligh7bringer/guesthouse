using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace assessment2_cs.Classes
{
    class Extra
    {
        private int id;
        private string type;
        private int bookingref;
        DbConnection con = new DbConnection();
        List<Extra> extras = new List<Extra>();
        
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

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

            string selectExtras = "SELECT * FROM extra_meals";
            try
            {
                SqlDataReader sdr = con.DataReader(selectExtras);
                while (sdr.Read())
                {
                    if (Convert.ToInt32(sdr["bookingref"]) == bref)
                    {
                        Meal meal = new Meal();
                        meal.Type = sdr["type"].ToString();
                        meal.Id = Convert.ToInt32(sdr["id"]);
                        meal.BookingRef = Convert.ToInt32(sdr["bookingref"]);
                        meal.DietReq = sdr["dietary_requirements"].ToString();
                        extras.Add(meal);
                    }           
                }
                sdr.Close();

                string selectCarhires = "SELECT * FROM extra_carhires";
                sdr = con.DataReader(selectCarhires);
                while(sdr.Read())
                {
                    if (Convert.ToInt32(sdr["bookingref"]) == bref)
                    {
                        CarHire carhire = new CarHire();
                        carhire.BookingRef = Convert.ToInt32(sdr["bookingref"]);
                        carhire.StartDate = Convert.ToDateTime(sdr["start_date"]);
                        carhire.EndDate = Convert.ToDateTime(sdr["end_date"]);
                        carhire.Driver = sdr["driver"].ToString();
                        carhire.Id = Convert.ToInt32(sdr["id"]);
                        extras.Add(carhire);
                    }
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
            return extras;
        }

        public void RemoveFromDB()
        {
            string query;
            if (type == "Evening meals" || type == "Breakfast meals")
            {
                query = "DELETE FROM extras_meals WHERE id=@id";
            } 
            else
            {
                query = "DELETE FROM extras_carhires WHERE id=@id";
            }
            con.OpenConnection();
            try
            {
                SqlCommand qDelete = new SqlCommand(query, con.Con);
                qDelete.Parameters.AddWithValue("id", id);
                qDelete.ExecuteNonQuery();
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

        public virtual void Update()
        {
            //to be overriden
        }
    }
}
