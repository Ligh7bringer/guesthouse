using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using assessment2_cs;
using assessment2_cs.Classes;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddGuestTest()
        {
            // setup
            Booking b_ = new Booking();
            Guest g1 = new Guest();
            Guest g2 = new Guest();
            int expectedCount = 2;

            // act
            b_.AddGuest(g1);
            b_.AddGuest(g2);

            //assert
            Assert.AreEqual(b_.Guests.Count, expectedCount, "AddGuest() method does not appear to work.");
        }

        [TestMethod]
        public void AddCustomerTest()
        {
            //setup
            Booking b_ = new Booking();
            Customer c_ = new Customer();
            c_.Name = "foo bar";
            c_.Address = "123 asd street";
            c_.Refnumber = 1;

            //act 
            b_.AddCustomer(c_);

            //assert
            Assert.AreSame(b_.HasCustomer, c_, "AddCustomer() doesn't appear to work");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BookingDatesTest()
        {
            //setup
            Booking b_ = new Booking();
            DateTime arrivald = Convert.ToDateTime("5/1/2016");
            DateTime departd = Convert.ToDateTime("1/1/2016");

            //act 
            b_.ArrivalDate = arrivald;
            b_.DepartDate = departd;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GuestsNumTest()
        {
            //setup
            Booking b_ = new Booking();
            Guest g_ = new Guest();

            //act 
            b_.AddGuest(g_);
            b_.AddGuest(g_);
            b_.AddGuest(g_);
            b_.AddGuest(g_);
            b_.AddGuest(g_);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtrasOfSameTypeTest()
        {
            //setup
            Booking b_ = new Booking();
            Meal m = new Meal();
            m.Type = "Breakfast";

            //act
            b_.AddExtra(m);
            b_.AddExtra(m);
        }

        [TestMethod]
        public void GenerateInvoiceTest()
        {
            //set up
            Customer c_ = new Customer("foo bar", "123 test rd");
            Guest g_ = new Guest("guest one", 22);
            Guest g2_ = new Guest("guest two", 12);
            Booking b_ = new Booking(Convert.ToDateTime("1/1/2017"), Convert.ToDateTime("3/1/2017"), c_);
            b_.AddGuest(g_);
            b_.AddGuest(g2_);
            Meal m = new Meal();
            m.Type = "Evening meals";
            b_.AddExtra(m);

            double expectedCost = 350;

            //test
            double actualCost = b_.GenerateInvoice();

            //assert
            Assert.AreEqual(expectedCost, actualCost, "Expected cost doesn't match actual cost.");
        }

        [TestMethod]
        public void GetNightsTest()
        {
            //setup
            Booking b_ = new Booking(Convert.ToDateTime("1/1/2017"), Convert.ToDateTime("3/1/2017"));
            int expected = 2;

            //act
            int actual = b_.GetNights();

            //assert
            Assert.AreEqual(expected, actual, "Expected number of nights doesn't match actual");
        }
    }
}
