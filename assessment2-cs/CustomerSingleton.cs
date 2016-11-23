using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw2_csharp
{
    class CustomerSingleton
    {
        private static CustomerSingleton _instance;

        private CustomerSingleton() { }

        private String name;
        private String address;
        private int refnumber;

        private static CustomerSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CustomerSingleton();
                }

                return _instance;
            }
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
    }
}
