using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public class TermDeposit:IAccountV1
    {
        public double InterestRate { get; set; }
        public DateTime Maturity { get; set; }
        public int Term { get; set; }
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal FullBalance { get; set; }
        public bool isActive { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int CustomerID { get; set; }
        public void Deposit(decimal amount)
        {
            FullBalance += (decimal)((double)amount * (InterestRate * Term));
            
        }
        public void Withdraw(decimal amount)
        {
            if (Maturity < DateTime.Now && amount == FullBalance)
            {
                
                FullBalance -= amount;
            }
            else
            {
                Console.WriteLine($"Sorry you cannot withdraw your CD, it has to mature and you have to withdraw the full CD.");
                Console.WriteLine("Press enter to go back home.");
                Console.ReadLine();
                UserInterface.CustomerHomeScreen();
            }
        }
    }
}
