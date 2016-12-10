using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2_cs.Classes
{
    class Meal : Extra
    {
        private string dietreq;       
        private DbConnection con = new DbConnection();

        public string DietReq
        {
            get { return dietreq; }
            set
            {
                if(String.IsNullOrEmpty(value))
                {
                    ArgumentException ex = new ArgumentException("Please enter the dietary requirements.");
                    throw ex;
                }
                dietreq = value;
            }
        }

        public override void AddToDB()
        {
            string query = "INSERT INTO extra_meals VALUES (@type, @dreq, @bref)";
            try
            {
                con.OpenConnection();
                SqlCommand qInsert = new SqlCommand(query, con.Con);
                qInsert.Parameters.AddWithValue("type", this.Type);
                qInsert.Parameters.AddWithValue("dreq", dietreq);
                qInsert.Parameters.AddWithValue("bref", this.BookingRef);
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
            string query = "UPDATE extra_meals SET dietary_requirements=@dietreq WHERE id=@id";
            con.OpenConnection();
            try
            {
                SqlCommand qUpdate = new SqlCommand(query, con.Con);
                qUpdate.Parameters.AddWithValue("dietreq", dietreq);
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
    }    
}
