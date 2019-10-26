using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public class Loan:IAccount
    {
        public double InterestRate { get; set; }
        public decimal Balance { get; set; }
        public decimal FullBalance { get; set; }
        public decimal InstallmentAmount { get; set; }
        public int AccountNumber { get; set; }
        public int Term { get; set; }
        public List<Transaction> Transactions { get; set; }
        public bool isActive { get; set; }
        public void PayLoanInstallment(decimal amount)
        {
            FullBalance -= amount;  
        }
        
    }
}
