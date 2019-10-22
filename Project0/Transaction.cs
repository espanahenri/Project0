using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public string TypeOfTransaction { get; set; }
        public IAccount AccountFrom { get; set; }
        public IAccount AccountTo { get; set; }
    }
}
