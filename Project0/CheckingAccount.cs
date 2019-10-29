using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project0
{
    class CheckingAccount : IAccountV1
    {
        public double InterestRate { get; set; }
        public decimal Balance { get; set; }
        public int AccountNumber { get; set; }
        public bool isActive { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int CustomerID { get; set; }
        public void Deposit(decimal amt)
        {
            Balance += amt;
        }
        public void Withdraw(decimal amt)
        {
            if (Balance >= amt)
            {
                Balance -= amt;
            }
            else
            {
                Console.WriteLine("Sorry not enought funds to complete transaccion.");
                Console.WriteLine("Press enter to go back home.");
                Console.ReadLine();
                UserInterface.CustomerHomeScreen();
            }
        }
    }
}
