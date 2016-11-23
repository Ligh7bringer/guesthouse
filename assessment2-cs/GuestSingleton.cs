using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw2_csharp
{
    class GuestSingleton
    {
        private static GuestSingleton _instance;

        private GuestSingleton() { }

        //properties 
        private String name;
        private int passportno;
        private int age;

        private static GuestSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GuestSingleton();
                }
                return _instance;
            }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Passportno
        {
            get { return passportno; }
            set { passportno = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }
    }
}
