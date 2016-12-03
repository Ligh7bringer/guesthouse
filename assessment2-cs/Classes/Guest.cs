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
        private int id;
        private DbConnection con = new DbConnection();
        private List<Guest> guests = new List<Guest>();

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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public void AddToDB()
        {
            string query = "INSERT INTO guest (booking_ref, passport_num, age, guest_name) VALUES (@bookingref, @passportno, @age, @name)";
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

        public List<Guest> GetGuests(int bref)
        {
            con.OpenConnection();

            string selectGuest = "SELECT * FROM guest";
            try
            {
                SqlDataReader sdr = con.DataReader(selectGuest);
                while (sdr.Read() && Convert.ToInt32(sdr["booking_ref"]) == bref)
                {
                    Guest g = new Guest();
                    g.Name = sdr["guest_name"].ToString();
                    g.PassportNo = sdr["passport_num"].ToString();
                    g.Age = Convert.ToInt32(sdr["age"]);
                    g.BookingRef = Convert.ToInt32(sdr["booking_ref"]);
                    g.Id = Convert.ToInt32(sdr["id"]);
                    guests.Add(g);
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
            return guests;
        }

        public void UpdateGuest()
        {
            string query = "UPDATE guest SET guest_name=@name, passport_num=@passnum, age=@age WHERE id=@id";
            con.OpenConnection();
            try
            {
                SqlCommand qUpdate = new SqlCommand(query, con.Con);
                qUpdate.Parameters.AddWithValue("name", name);
                qUpdate.Parameters.AddWithValue("passnum", passportno);
                qUpdate.Parameters.AddWithValue("age", age);
                qUpdate.Parameters.AddWithValue("id", id);
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

        public void RemoveFromDB()
        {
            string query = "DELETE FROM guest WHERE id=@id";
            con.OpenConnection();
            try
            {
                SqlCommand qDelete = new SqlCommand(query, con.Con);
                qDelete.Parameters.AddWithValue("id", id);
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

        public override string ToString()
        {
            return "Name: " + name + ", Age: " + age.ToString() + ", Passport number: " + passportno;
        }
    }
}