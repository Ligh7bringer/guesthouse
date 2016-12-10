using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2_cs
{
    // Author: Svetlozar Georgiev
    // Date of last modification: 24/11/2016
    // Purpose: Should make connecting to the database much easier and 
    // use much less code. Also implements methods which should execute commands
    public class DbConnection 
    {
        // connection string for the database containing a relative path to the DB file 
        // so in theory it should work on any computer ?
        String ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\database\DB.mdf;Integrated Security=True;Connect Timeout=30";

        private SqlConnection con; // this returns the actual connection as it is sometimes used in SqlCommands

        //accessor for con
        public SqlConnection Con { get { return con; } }

        // used to open the connection
        public void OpenConnection()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }

        // closes the database connection
        public void CloseConnection()
        {
            con.Close();
        }

        // executes a query which was passed 
        public int ExecuteQueries(string Query_)
        {
            SqlCommand cmd = new SqlCommand(Query_, con);
            int result = cmd.ExecuteNonQuery();
            return result;
        }

        // simplifies the use of datareaders
        // when passed a select query it returns the data reader containg the results of the query
        public SqlDataReader DataReader(string Query_)
        {
            SqlCommand cmd = new SqlCommand(Query_, con);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }

    }
}
