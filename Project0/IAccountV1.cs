using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public interface IAccountV1:IAccount
    {
        void Deposit(decimal amt);
        void Withdraw(decimal amt);
    }
}
