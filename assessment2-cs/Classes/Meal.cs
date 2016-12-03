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
        private int id;
        private DbConnection con = new DbConnection();


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string DietReq
        {
            get { return dietreq; }
            set { dietreq = value; }
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
    }
    
}
