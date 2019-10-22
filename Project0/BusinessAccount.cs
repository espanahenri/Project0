using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    class BusinessAccount : IAccount
    {
        public double InterestRate { get; set; }
        public decimal OverdraftAmount { get; set; }
        public decimal Funds { get; set; }
        public int AccountNumber { get; set; }
        public List<Transaction> Transactions { get; set; }
        public void Deposit(decimal amt)
        {
            Funds += amt;
        }
        public void Withdraw(decimal amt)
        {
            if (Funds < amt)
            {
                var _OverdraftFacility = new OverdraftFacility();
                OverdraftAmount = amt - Funds;
                _OverdraftFacility.OverdraftAmount = OverdraftAmount;
                _OverdraftFacility.CalculateOverdraftPayment();
                UserInterface._CurrentCustomer.OverdraftFacilities.Add(_OverdraftFacility);
                Funds = 0;
            }
            else
            {
                Funds -= amt;
            }
        }
    }
}
