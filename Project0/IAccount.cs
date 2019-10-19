using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public interface IAccount
    {
        double InterestRate { get; set; }
        decimal Funds { get; set; }
        int AccountNumber { get; set; }
        void Deposit(decimal amt);
        void Withdraw(decimal amt);

    }
}
