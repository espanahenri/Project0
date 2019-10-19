using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<IAccount> Accounts { get; set; }
    }
}
