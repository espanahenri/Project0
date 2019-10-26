using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public interface IAccount
    {
        double InterestRate { get; set; }
        decimal Balance { get; set; }
        int AccountNumber { get; set; }
        public bool isActive { get; set; }
        public List<Transaction> Transactions { get; set; }   

    }
}
