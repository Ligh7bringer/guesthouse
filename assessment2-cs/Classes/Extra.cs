using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace assessment2_cs.Classes
{
    // Author: Svetlozar Georgiev
    // Date of last change: 7/12/2016
    // Purpose: implements the common properties for extras. Specific 
    // extra classes will inherit from this class
    abstract class Extra // make it abstract because we don't want objects of type extra
    {
        // private properties
        private int id; // id of an extra. this is mainly used for the database
        private string type; // specifies the type of extra (car hire, breakfast or evening meals)
        private int bookingref; // this links the extra to the booking it is associated with
        private double cost; // the cost of an extra per day
        DbConnection con = new DbConnection(); // connection to the database 
        List<Extra> extras = new List<Extra>(); // list which will later be used to return all extras from the database

        // accessors for the private properties
        public double Cost
        {
            // returns the cost depending on the extra type
            get
            {
                if (type == "Evening meals")
                {
                    return 15;
                }
                else if(type == "Breakfast meals")
                {
                    return 5;
                }
                else
                {
                    return 50;
                }
            }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Type
        {
            get { return type; }
            set
            {
                // we have to check if the value we're trying to assign is null or empty
                // if it is - throw an exception
                if(String.IsNullOrEmpty(value))
                {
                    ArgumentException ex = new ArgumentException("Please select extra type.");
                    throw ex;
                }
                type = value; // if it's not - assign it
            }
        }
        
        public int BookingRef
        {
            get { return bookingref; }
            set { bookingref = value; }
        }

        // virtual class which will be overriden 
        public virtual void AddToDB()
        {
            // to be overriden by children classes
        }

        // returns a list of all extras for the booking which has booking reference bref
        public List<Extra> GetExtras(int bref)
        {
            con.OpenConnection();
            // create a string with the query
            string selectExtras = "SELECT * FROM extra_meals"; // get the meals first
            try
            {
                SqlDataReader sdr = con.DataReader(selectExtras); // get the datareader with the results from the query from the connection object 
                while (sdr.Read()) // read one row at a time
                {
                    if (Convert.ToInt32(sdr["bookingref"]) == bref) // we have to make sure we are only looking at extras
                    {                                               // for the right booking
                        Meal meal = new Meal();                     // create a meal object
                        meal.Type = sdr["type"].ToString();         // assign the properties
                        meal.Id = Convert.ToInt32(sdr["id"]);
                        meal.BookingRef = Convert.ToInt32(sdr["bookingref"]);
                        meal.DietReq = sdr["dietary_requirements"].ToString();
                        extras.Add(meal); // finally add the meal to meals list
                    }           
                }
                sdr.Close();

                string selectCarhires = "SELECT * FROM extra_carhires"; // now select all car hire extras
                sdr = con.DataReader(selectCarhires);
                while(sdr.Read())
                {
                    if (Convert.ToInt32(sdr["bookingref"]) == bref) // get only the ones for the right booking
                    {
                        CarHire carhire = new CarHire(); // create an object and assign the values from the datareader
                        carhire.BookingRef = Convert.ToInt32(sdr["bookingref"]); // to its properties
                        carhire.StartDate = Convert.ToDateTime(sdr["start_date"]);
                        carhire.EndDate = Convert.ToDateTime(sdr["end_date"]);
                        carhire.Driver = sdr["driver"].ToString();
                        carhire.Id = Convert.ToInt32(sdr["id"]);
                        extras.Add(carhire); // the the object to the list
                    }
                }
            }
            catch (SqlException ex) //rethrow any exceptions
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
            return extras;
        }

        // deletes an extra from the database
        public void RemoveFromDB()
        {
            string query; // this will contain the query 
            // assign the right query depending on the type of the extra
            if (type == "Evening meals" || type == "Breakfast meals")
            {
                query = "DELETE FROM extra_meals WHERE id=@id";
            } 
            else
            {
                query = "DELETE FROM extra_carhires WHERE id=@id";
            }
            con.OpenConnection();
            try
            {
                SqlCommand qDelete = new SqlCommand(query, con.Con); // create a command with the query
                qDelete.Parameters.AddWithValue("id", id);
                qDelete.ExecuteNonQuery(); // execute the query
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

        // this will be overriden
        public virtual void Update()
        {
            //to be overriden by children classes
        }

        // this will be overriden
        public virtual int GetDays()
        {
            //to be overridden by children classes
            return 0;
        }
    }
}

