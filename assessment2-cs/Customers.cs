using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2_cs
{
    class Customers
    {
        private List<Customer> customers = new List<Customer>();
        private DbConnection con = new DbConnection();

        public List<Customer> GetCustomerNames()
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
    }
}
