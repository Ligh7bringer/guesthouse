using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TO DO: CLASS DESCRIPTION 
namespace cw2_csharp
{
    class BookingSingleton
    {
        private static BookingSingleton _instance;

        private BookingSingleton()
        {
            bookingref = 0; 
        }

        private String arrivaldate;
        private String departdate;
        private int bookingref;
        private CustomerSingleton cust;
        private List<GuestSingleton> guests = new List<GuestSingleton>();

        public static BookingSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BookingSingleton();
                }

                return _instance;
            }
        }

        public String ArrivalDate
        {
            get { return arrivaldate; }
            set { arrivaldate = value; }
        }

        public String DepartDate
        {
            get { return departdate; }
            set { departdate = value; }
        }

        public int GenerateRef()
        {
            bookingref++;
            return bookingref;
        }
    }
}

        
