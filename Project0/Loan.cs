using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public class Loan:IAccount
    {
        public double InterestRate { get; set; }
        public decimal Balance { get; set; }
        public int AccountNumber { get; set; }
        public List<Transaction> Transactions { get; set; }
        
    }
}
