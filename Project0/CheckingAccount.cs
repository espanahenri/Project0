using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    class CheckingAccount : IAccount
    {
        public decimal InterestRate { get; set; }
        public decimal Funds { get; set; }
        public int AccountNumber { get; set; }
        public void Deposit(decimal amt)
        {
            Funds += amt;
        }
        public void Withdraw(decimal amt)
        {
            Funds -= amt;
        }
    }
}
