using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public string TypeOfTransaction { get; set; }
        public decimal Amount { get; set; }
        public DateTime TimeOfTransaction { get; set; }
        public int AccountNumber { get; set; }
    }
}
