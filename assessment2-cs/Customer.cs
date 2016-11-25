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

        }

       
    }
}
