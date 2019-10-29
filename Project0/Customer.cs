using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isActive { get; set; }
        public List<IAccount> Accounts = new List<IAccount>();
        public decimal OverdraftFacilityDue { get; set; }
        
    }
}
