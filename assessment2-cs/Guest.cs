using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2_cs
{
    class Guest
    {
        //properties 
        private int bookingref;
        private String name;
        private string passportno;
        private int age;
        private DbConnection con = new DbConnection();

        public int BookingRef
        {
            get { return bookingref; }
            set { bookingref = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public string PassportNo
        {
            get { return passportno; }
            set { passportno = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public void AddToDB()
        {
            string query = "INSERT INTO guest (booking_ref, passport_num, age, name) VALUES (@bookingref, @passportno, @age, @name)";
            try
            {
                con.OpenConnection();
                SqlCommand qInsert = new SqlCommand(query, con.Con);
                qInsert.Parameters.AddWithValue("bookingref", bookingref);
                qInsert.Parameters.AddWithValue("name", name);
                qInsert.Parameters.AddWithValue("passportno", passportno);
                qInsert.Parameters.AddWithValue("age", age);
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

        public override string ToString()
        {
            return "Name: " + name + ", Age: " + age.ToString() + ", Passport number: " + passportno;
        }
    }
}
