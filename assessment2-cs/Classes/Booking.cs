using assessment2_cs.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Author: Svetlozar Georgiev
// Date of last modification: 10/12/2016
// Purpose: Allows the creation of booking objects and implements methods 
// which manipulate a booking's properties, customer and guests. Allows fetching and adding bookings
// to the database
namespace assessment2_cs
{
    class Booking
    {
        // properties of a booking 
        private int refnum; // reference number of a booking. Used to uniquely identify a booking
        private DateTime arrivaldate; // the check in date for the guests of the booking
        private DateTime departdate; // check out date for guests
        private Customer hasCustomer; // adds a relationship with the customer who created the booking
        private List<Guest> guests = new List<Guest>(); // list which will contain all guests of a booking
        private List<Extra> extras = new List<Extra>(); // list which will contain all extras of a booking
        private DbConnection con = new DbConnection(); // database connection which will be used for queries to fetch
                                                       // guests, extras and bookings


        // chained constructors which should make creating new objects faster. 
        public Booking() { }

        public Booking(DateTime arrd, DateTime depd)
        {
            arrivaldate = arrd;
            departdate = depd;
        }

        public Booking(DateTime arrd, DateTime depd, Customer c) : this(arrd, depd)
        {
            this.hasCustomer = c;
        }

        //accessor for all the private properties listed above
        public List<Extra> Extras
        {
            get { return extras; }
            set { extras = value; }
        }

        public Customer HasCustomer
        {
            get { return hasCustomer; }
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
            set
            {
                arrivaldate = value;
            }
        }

        public DateTime DepartDate
        {
            get { return departdate; }
            set
            {
                if (value.Date < arrivaldate)
                {
                    ArgumentException ex = new ArgumentException("A booking's departure date must be later than its arrival date.");
                    throw ex;
                }
                departdate = value;
            }
        }

        // allows inserting a booking into the database
        public void AddToDB()
        {
            //create a query to be used in a sqlcommand
            string query = "INSERT INTO booking (arrival_date, departure_date, cust_ref) OUTPUT Inserted.reference_num VALUES (@arrivald, @departd, @cust_ref)";
            con.OpenConnection(); //open the connection 
            try
            {
                SqlCommand qInsert = new SqlCommand(query, con.Con); //create a command using the query above
                qInsert.Parameters.AddWithValue("arrivald", arrivaldate); // replace all placeholders in the command with actual values
                qInsert.Parameters.AddWithValue("departd", departdate);
                qInsert.Parameters.AddWithValue("cust_ref", hasCustomer.Refnumber);
                // using execute scalar should return the last inserted id in the database because of OUTPUT Inserted.reference_num 
                var id = qInsert.ExecuteScalar(); 
                foreach (Guest guest in guests) // add all guests stored in the guests list to the database as well
                {
                    // set their bookingref to the booking ref of the booking which we just inserted
                    guest.BookingRef = Convert.ToInt32(id); 
                    guest.AddToDB();
                }
            }
            catch (SqlException ex) // if an exception is caught just rethrow it 
            {                       // so it can be handled in the gui classes
                throw ex;
            }
            finally
            {
                con.CloseConnection(); // close the connection
            }
        }

        // set the customer of a booking to the one passed to this method
        public void AddCustomer(Customer c)
        {
            this.hasCustomer = c;
        }

        // add a guest to the guests list of a booking
        public void AddGuest(Guest g)
        {
            // before adding the guest passed to this method check the number of guests the booking currently has
            if (guests.Count > 3)
            {
                //throw an exception if another guest can't be added. it will be handled in the gui classes
                ArgumentException ex = new ArgumentException("Only 4 guests are allowed per booking");
                throw ex;
            }
            //if an exception is not thrown, add the guest
            this.guests.Add(g);
        }

        // returns a list of all bookings in the database
        public List<Booking> GetBookings()
        {
            List<Booking> bookings = new List<Booking>(); // create a list in which bookings will be stored
            // query to select all bookings 
            string query = "SELECT booking.reference_num, arrival_date, departure_date, cust_ref, name, address FROM booking JOIN customer ON booking.cust_ref=customer.reference_num";
            try
            {
                con.OpenConnection();
                SqlDataReader sdr = con.DataReader(query); // get a datareader with the results of the query
                while (sdr.Read())
                {
                    // create objects which will be needed
                    Booking b = new Booking();
                    Customer c = new Customer();
                    Guest g = new Guest();
                    Meal m_ = new Meal();
                    //assign the values from the data reader to the object properties
                    c.Name = sdr["name"].ToString();
                    c.Address = sdr["address"].ToString();
                    c.Refnumber = Int32.Parse(sdr["cust_ref"].ToString());
                    b.AddCustomer(c);
                    b.ArrivalDate = Convert.ToDateTime(sdr["arrival_date"]);
                    b.DepartDate = Convert.ToDateTime(sdr["departure_date"]);
                    b.RefNum = Int32.Parse(sdr["reference_num"].ToString());                    
                    foreach (var guest in g.GetGuests(b.RefNum)) // get all the guests for the current booking
                    {
                        b.AddGuest(guest); // add them to bookings list of guests
                    }
                    foreach (var extra_ in m_.GetExtras(b.RefNum)) //get all the extras as well
                    {
                        b.AddExtra(extra_); // add them to the list of extras
                    }
                    bookings.Add(b); //finally add the booking to the list
                }
                sdr.Close(); //close the data reader
            }
            catch (SqlException ex) // if an exception was caught
            {
                throw ex; //rethrow it as it will be handled in the gui classes
            }
            finally
            {
                con.CloseConnection(); //close the connection
            }
            return bookings; // finally return the list of all bookings 
        }

        // updates a booking's details in the database
        public void Update()
        {
            // query to update a booking
            string query = "UPDATE booking SET arrival_date=@arrd, departure_date=@depd WHERE reference_num=@refnum";
            con.OpenConnection();
            try
            {
                foreach (var guest in guests) // call the UpdateGuest() method on every guest the booking has
                {
                    guest.UpdateGuest();
                }
                SqlCommand qUpdate = new SqlCommand(query, con.Con); // create a command to update the booking
                qUpdate.Parameters.AddWithValue("arrd", arrivaldate); // relace placeholders with actual values
                qUpdate.Parameters.AddWithValue("depd", departdate);
                qUpdate.Parameters.AddWithValue("refnum", refnum);
                qUpdate.ExecuteNonQuery(); // execute command
            }
            catch (SqlException ex) // rethrow caught exception as it will be handled in the gui classes
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection(); // close connection
            }
        }

        // deletes a booking from the database 
        public void RemoveFromDB()
        {
            // query to delete from DB
            string query = "DELETE FROM booking WHERE reference_num=@refnum";
            con.OpenConnection();
            try
            {
                // before actually deleting the booking we must delete any related extras and guests
                // because of foreign key constraints
                foreach (var extra_ in extras) //call the RemoveFromDB() method on every extra the booking contains
                {
                    extra_.RemoveFromDB();
                }
                foreach (var guest in guests) //call the RemoveFromDB() method on every guest the booking contains
                {
                    guest.RemoveFromDB();
                }
                SqlCommand qUpdate = new SqlCommand(query, con.Con); // create a command
                qUpdate.Parameters.AddWithValue("refnum", refnum); // add values to it
                qUpdate.ExecuteNonQuery(); // run the query
            }
            catch (SqlException ex) // rethrow caught exceptions
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection(); // close the connection
            }
        }    

        // calculates the total cost of a booking
        public double GenerateInvoice()
        {
            int nights = this.GetNights(); // get the number of nights of the booking
            double cost = 50 * nights; // because of customer. assuming a customer's age is always over 18
            foreach (var guest_ in guests) // add the cost for each guest based on their age
            {
                if(guest_.Age < 18) // if under 18, add 30 for every night to the total cost
                {
                    cost += 30 * nights;
                }
                else // if over 18, add 50 instead
                {
                    cost += 50 * nights;
                }
            }
            //calculate extras costs and add them to the total cost
            cost += this.GetMealsCost(); 
            cost += this.GetCarhiresCost();
            return cost; // return the cost
        }

        // adds an extra to a booking
        public void AddExtra(Extra e_)
        {
            // we have to make sure the booking doesn't contain extra of the same type already
            // so compare the type of the extra we're trying to add with the type of the extras the booking already has
            foreach (Extra extra_ in extras)
            {
                if(e_.Type == extra_.Type) // if an extra of the same type is found
                {
                    // throw an exception
                    ArgumentException ex = new ArgumentException("This booking already has an extra of this type.");
                    throw ex;
                }
            }
            // else we can add the extra to the booking
            extras.Add(e_);
        }

        // calculates the difference between arrival date and departure date
        // i.e. the number of nights for a booking
        public int GetNights()
        {
            return (departdate.Date - arrivaldate.Date).Days;
        }

        // calculates the cost of a carhire 
        public double GetCarhiresCost()
        {
            double cost = 0;
            foreach (Extra extra_ in extras) // find the carhire
            {
                if (extra_.Type == "Carhire")
                { 
                    cost += extra_.Cost * extra_.GetDays(); // calculate the cost
                }
            }            
            return cost; // return total cost, if the booking doesn't have a car hire this will just return 0
        }

        // calculate the cost of meal extras of a booking
        public double GetMealsCost()
        {
            double cost = 0;
            foreach (Extra extra_ in extras) // check if the booking has any extras which are meals
            {
                if (extra_.Type != "Carhire")
                {
                    cost += extra_.Cost * (guests.Count + 1) * this.GetNights(); // calculate the cost
                }
            }
            return cost; // return the total cost
        }
        
        // overrides the object's ToString() method, this will be used in the gui classes
        // it will return a string like 1: Foo Bar, 1/1/2012-3/1/2012
        public override string ToString()
        {
            return refnum + ": " + hasCustomer.Name + " " + arrivaldate.ToString("dd/MM/yyyy") + "-" + departdate.Date.ToString("dd/MM/yyyy");
        }
    }
}
