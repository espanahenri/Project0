using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<IAccount> Accounts = new List<IAccount>();
        public List<Loan> Loans { get; set; }
        public decimal OverdraftFacilityDue { get; set; }
        

        public void Transfer(IAccount acc1, IAccount acc2, decimal amt)
        {
           // acc1.Withdraw(amt);
            //acc2.Deposit(amt);
            var transaction = new Transaction()
            {
                TransactionID = UserInterface._TransactionID,
                AccountFrom = acc1,
                AccountTo = acc2
            };
            acc1.Transactions.Add(transaction);
            acc2.Transactions.Add(transaction);
        }
    }
}
