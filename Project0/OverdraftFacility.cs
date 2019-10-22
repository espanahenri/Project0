using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public class OverdraftFacility
    {
        public int OverdraftFacilityID { get; set; }
        public double OverdraftInterestRate { get; private set; }
        public decimal OverdraftAmount { get; set; }
        public decimal OverdraftPayment { get; set; }
        public OverdraftFacility()
        {
            OverdraftInterestRate = 1.0;
        }
        public void CalculateOverdraftPayment()
        {
           OverdraftPayment = (decimal)((double)OverdraftAmount + ((double)OverdraftAmount * 0.1));
        }
    }
}
