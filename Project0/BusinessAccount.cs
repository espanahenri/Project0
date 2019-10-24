using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    class BusinessAccount : IAccountV1
    {
        public double InterestRate { get; set; }
        public decimal OverdraftAmount { get; set; }
        public decimal OverdraftFacility { get; set; }
        public decimal Balance { get; set; }
        public int AccountNumber { get; set; }
        public List<Transaction> Transactions { get; set; }
        public void Deposit(decimal amt)
        {
            Balance += amt;
        }
        public void Withdraw(decimal amt)
        {
            if (Balance < amt)
            {
                OverdraftAmount = amt - Balance;
                OverdraftFacility = OverdraftAmount + (amt * 0.1M);
                Balance = 0;
                UserInterface._CurrentCustomer.OverdraftFacilityDue = OverdraftFacility;
                Console.WriteLine($"Attention: You have added ${OverdraftFacility} to your overdraft facility");
            }
            else
            {
                Balance -= amt;
            }
        }
        
    }
}
