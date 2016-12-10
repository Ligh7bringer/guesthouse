using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2_cs
{
    // Author: Svetlozar Georgiev
    // Date of last modification: 8/12/2016
    // Purpose: Allows the creation of Customer objects, changing their properties. Implements 
    // the ability to add and remove customers from the database
    class Customer
    {
        // private properties
        private String name; // the name of a customer
        private String address; // the address of a customer
        private int refnumber; // the reference number of a customer
        private List<Customer> customers = new List<Customer>(); // list in which all customers will be stored later
        private DbConnection con = new DbConnection(); // database connection object

        // constructors
        public Customer() { }

        public Customer(String nm, String addr)
        {
            name = nm;
            address = addr;
        }
        
        // accessors for the private properties
        public String Name
        {
            get { return name; }
            set
            {
                // if the value we're trying to assign is not null or empty
                if(!String.IsNullOrEmpty(value))
                {
                    name = value; // assign it as the customer's name
                }
                else
                {
                    // if not, throw an exception
                    ArgumentException ex = new ArgumentException("Please enter the customer's name.");
                    throw ex;
                }
            }
        }

        public String Address
        {
            get { return address; }
            set
            {
                // if the value we're trying to assign is not null, assign it 
                if (!String.IsNullOrEmpty(value))
                {
                    address = value;
                }
                else
                {
                    // if it's not - throw an exception
                    ArgumentException ex = new ArgumentException("Please enter the customer's address.");
                    throw ex;
                }
            }
        }

        public int Refnumber
        {
            get { return refnumber; }
            set { refnumber = value; }
        }

        // inserts a customer's details into the database 
        public void AddToDB()
        {
            // create a string containing the sql query
            string query = "INSERT INTO customer (name, address) VALUES (@name, @address)";            
            con.OpenConnection();
            try
            {
                SqlCommand qInsert = new SqlCommand(query, con.Con); // create a command with the query
                qInsert.Parameters.AddWithValue("name", name); // replace the placeholders with values
                qInsert.Parameters.AddWithValue("address", address);
                qInsert.ExecuteNonQuery(); // execute the command
            } 
            catch (SqlException ex) // rethrow any exceptions
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        // removes a customer's details frm the database
        public void RemoveFromDB()
        {
            // create a string containing the sql query
            string query = "DELETE FROM customer WHERE reference_num=@ref";
            con.OpenConnection();
            try
            {
                SqlCommand qDelete = new SqlCommand(query, con.Con); // create a command with the query
                qDelete.Parameters.AddWithValue("ref", refnumber); // replace placeholders
                qDelete.ExecuteNonQuery(); // execute the command
            }
            catch (SqlException ex) // rethrow caught exceptions
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        // updates a customer's details in the database
        public void UpdateCustomer()
        {
            // create a string containing the sql query
            string query = "UPDATE customer SET name=@name, address=@address WHERE reference_num=@refnum";
            con.OpenConnection();
            try
            {
                SqlCommand qUpdate = new SqlCommand(query, con.Con); // create a command with the query
                qUpdate.Parameters.AddWithValue("name", name); // replace placeholders with values
                qUpdate.Parameters.AddWithValue("address", address);
                qUpdate.Parameters.AddWithValue("refnum", refnumber);
                qUpdate.ExecuteNonQuery(); // execute the command 
            }
            catch (SqlException ex) // rethrow any exceptions
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        // returns a list of all customers in the database
        public List<Customer> GetCustomers()
        {
            con.OpenConnection();
            try
            {
                // create a string containing the sql query
                String query = "SELECT * FROM customer"; 
                SqlDataReader sdr = con.DataReader(query); // create a datareader and use the databaseconnection method which executes the datareader and returns the results 
                while (sdr.Read()) // read one row at a time                                             
                {
                    Customer c = new Customer(); // create a customer object and assign values from the reader to it's properties
                    c.Name = sdr["name"].ToString();
                    c.Address = sdr["address"].ToString();
                    c.Refnumber = Int32.Parse(sdr["reference_num"].ToString());
                    customers.Add(c); //finally add the customer to the list of customers
                }
                sdr.Close();
            }
            catch (SqlException ex) // rethrow exceptions
            {
                throw ex;
            }
            finally
            {
                con.CloseConnection();
            }
            return customers; // finally return the list of all customers
        }

        // overrides the ToString method for objects of type customer.
        // will be used in gui classes 
        public override string ToString()
        {
            return refnumber + ": " + name + ", " + address;
        }

    }
}
