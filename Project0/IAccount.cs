using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    interface IAccount
    {
        decimal InterestRate { get; set; }
        decimal Funds { get; set; }
        int AccountNumber { get; set; }
        void Deposit(decimal amt);
        void Withdrawl(decimal amt);

    }
}
