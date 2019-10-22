using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project0
{
    class CheckingAccount : IAccount
    {
        public double InterestRate { get; set; }
        public decimal Funds { get; set; }
        public int AccountNumber { get; set; }
        public List<Transaction> Transactions { get; set; }
        public void Deposit(decimal amt)
        {
            Funds += amt;
        }
        public void Withdraw(decimal amt)
        {
            if (Funds > amt)
            {
                Funds -= amt;
            }
            else
            {
                Console.WriteLine("Sorry not enought funds to complete transaccion.");
                Thread.Sleep(1000);
            }
        }
    }
}
