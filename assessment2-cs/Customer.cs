using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2_cs
{
    class Customer
    {
        private String name;
        private String address;
        private int refnumber;
        private List<Customer> customers = new List<Customer>();
        private DbConnection con = new DbConnection();

        public Customer() { }

        public Customer(String nm, String addr)
        {
            name = nm;
            address = addr;
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Address
        {
            get { return address; }
            set { address = value; }
        }

        public int Refnumber
        {
            get { return refnumber; }
            set { refnumber = value; }
        }

        public void AddToDB()
        {
            string query = "INSERT INTO customer (name, address) VALUES (@name, @address)";            
            con.OpenConnection();
            try
            {
                SqlCommand qInsert = new SqlCommand(query, con.Con);
                qInsert.Parameters.AddWithValue("name", name);
                qInsert.Parameters.AddWithValue("address", address);
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

        public void RemoveFromDB()
        {
            string query = "DELETE FROM customer WHERE reference_num=@ref";
            con.OpenConnection();
            try
            {
                SqlCommand qDelete = new SqlCommand(query, con.Con);
                qDelete.Parameters.AddWithValue("ref", refnumber);               
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

        public void UpdateCustomer()
        {            
            string query = "UPDATE customer SET name=@name, address=@address WHERE reference_num=@refnum";
            con.OpenConnection();
            try
            {
                SqlCommand qUpdate = new SqlCommand(query, con.Con);
                qUpdate.Parameters.AddWithValue("name", name);
                qUpdate.Parameters.AddWithValue("address", address);
                qUpdate.Parameters.AddWithValue("refnum", refnumber);
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

        public List<Customer> GetCustomers()
        {
            con.OpenConnection();
            try
            {
                String query = "SELECT * FROM customer";
                SqlDataReader sdr = con.DataReader(query);
                while (sdr.Read())
                {
                    Customer c = new Customer();
                    c.Name = sdr["name"].ToString();
                    c.Address = sdr["address"].ToString();
                    c.Refnumber = Int32.Parse(sdr["reference_num"].ToString());
                    customers.Add(c);
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
            return customers;
        }

        public override string ToString()
        {
            string s = name + "( " + address + ")";
            return s;
        }

    }
}
