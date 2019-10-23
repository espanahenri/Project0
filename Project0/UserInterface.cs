using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
namespace Project0
{
    public static class UserInterface
    {
        public static int _TransactionID = 5000;
        public static int _LoanID = 4000; 
        private static int _CustomerID = 1000;
        private static List<Customer> _Customers = new List<Customer>();
        public static List<IAccount> transferlist = new List<IAccount>();
        private static int _CheckingAccountNumber = 2000;
        private static int _BusinessAccountNumber = 3000;
        public static Customer _CurrentCustomer = new Customer();
        enum _OpenAccountSelector { Checking=1, Business=2 };
        enum _HomeScreenSelector 
        { 
            Register = 1,
            OpenNewAccount = 2,
            CloseAccount = 3,
            Deposit = 4,
            Transfer = 6,
            Withdraw = 5,
            ListAllAccounts = 7,
            DisplayTransaccions = 8,
            Exit = 9
        };

        #region Register Page
        public static void RegisterScreen()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Revature Bank");
            Console.WriteLine("Please Register Now: ");
            Console.WriteLine("First Name: ");
            var firstname = Console.ReadLine();
            Console.WriteLine("Last Name: ");
            var lastname = Console.ReadLine();
            Console.WriteLine("Thank you for Registering, Welcome to our bank!");
            var customer = new Customer()
            {
                ID = _CustomerID,
                FirstName = firstname,
                LastName = lastname
            };
            _Customers.Add(customer);
            _CurrentCustomer = customer;
            _CustomerID++;
            OpenAccountScreen();
            //CustomerHomeScreen(_CurrentCustomer);
        }
        #endregion
        public static void CustomerHomeScreen()
        {
            
            Console.Clear();
            Console.WriteLine($"Welcome to Revature Bank { _CurrentCustomer.FirstName } { _CurrentCustomer.LastName }!");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Open new account");
            Console.WriteLine("3. Close account");
            Console.WriteLine("4. Deposit");
            Console.WriteLine("5. Withdraw");
            Console.WriteLine("6. Transfer");
            Console.WriteLine("7. List of all my accounts");
            Console.WriteLine("8. Display Transaccions for an account");
            Console.WriteLine("9. Exit");
            //var selection = Convert.ToInt32(Console.ReadLine());
            int selection;
            var input = Console.ReadLine();
            bool checknumber = Int32.TryParse(input, out selection);
            if (!checknumber)
            {
                selection = 30;
            }
            switch (selection)
            {
                case (int)_HomeScreenSelector.Register:
                    RegisterScreen();
                    break;
                case (int)_HomeScreenSelector.OpenNewAccount:
                    OpenAccountScreen();
                    break;
                case (int)_HomeScreenSelector.CloseAccount:
                    CloseAccountScreen();
                    break;
                case (int)_HomeScreenSelector.Deposit:
                    DepositScreen();
                    break;
                case (int)_HomeScreenSelector.Withdraw:
                    WithdrawScreen();
                    break;
                case (int)_HomeScreenSelector.ListAllAccounts:
                    DisplayAllCustomerAccountsScreen();
                    break;
                case (int)_HomeScreenSelector.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Not valid option please try again.");
                    Thread.Sleep(1000);
                    CustomerHomeScreen();
                    break;
            }

        }
        public static void OpenAccountScreen()
        {
            Console.Clear();
            Console.WriteLine("What type of account would you like to open?");
            Console.WriteLine("Checking Account - 1");
            Console.WriteLine("Business Account - 2");
            int selection;
            var input = Console.ReadLine();
            bool checknumber = Int32.TryParse(input, out selection);
            if (!checknumber)
            {
                selection = 30;
            }
            switch (selection)
            {
                case (int)_OpenAccountSelector.Checking:
                    OpenCheckingAccount();
                    break;
                
                case (int)_OpenAccountSelector.Business:
                    OpenBusinessAccount();
                    break;
                default:
                    Console.WriteLine("Not valid option please try again.");
                    Thread.Sleep(1000);
                    OpenAccountScreen();
                    break;
            }
        }
        public static void DepositScreen()
        {
            Console.Clear();
            Console.WriteLine("Select account you want to deposit into:");
            DisplayAllCustomerAccounts();
            Console.WriteLine("Please type in account number you want to deposit in to.");
            var accountnumber = Convert.ToInt32(Console.ReadLine());
            var account = AccountSelector(accountnumber);
            Console.WriteLine("How much do you want to deposit?");
            var amt = Convert.ToDecimal(Console.ReadLine());
            DepositAccount(account, amt);
            Console.WriteLine($"Thank you, your deposit of ${amt} for account: {account.AccountNumber } was successful");
            Console.WriteLine("Please press enter to go back home.");
            Console.ReadLine();
            CustomerHomeScreen();     
        }
        public static void WithdrawScreen()
        {
            Console.Clear();
            
            decimal amount = 0.0m;
            int accountnumber;

            CheckAccountsExist();
            Console.WriteLine(" +++WITHDRAW+++");
            Console.WriteLine("Select the account");
                DisplayAllCustomerAccounts();
                Console.WriteLine("Type the number of the account.");
                var input = Console.ReadLine();
                bool checknumber = Int32.TryParse(input, out accountnumber);
                if (!checknumber)
                {
                    Console.WriteLine("Incorrect input type try again!");
                    Thread.Sleep(1000);
                    WithdrawScreen();
                }
                else 
                {
                    if (AccountNumberVerifier(accountnumber))
                    {
                        Console.WriteLine("How much would you like to withdraw?");
                        input = Console.ReadLine();
                        if(VerifyAmount(input, out amount))
                        {
                            AccountSelector(accountnumber).Withdraw(amount);
                            Console.WriteLine("Withdraw was successful!");
                            Console.WriteLine("Press enter to go back home.");
                            Console.ReadLine();
                            CustomerHomeScreen();
                        }
                        Console.WriteLine("Not valid amount.");
                        Thread.Sleep(1000);
                        WithdrawScreen();
                        
                    }
                    else
                    {
                        Console.WriteLine("Sorry account number not valid. Try again");
                        Thread.Sleep(1000);
                        WithdrawScreen();
                    }
                }        
        }
        
        public static void CloseAccountScreen()
        {
            Console.Clear();
            Console.WriteLine("Select account you want to close:");
            DisplayAllCustomerAccounts();
            var accountnumber = Convert.ToInt32(Console.ReadLine());
            CloseAccount(accountnumber);
            CustomerHomeScreen();
        }
        //public static void TransferScreen()
        //{
        //    if (_CurrentCustomer.CheckingAccounts.Count >= 2 || _CurrentCustomer.BusinessAccounts.Count >= 2
        //         || (_CurrentCustomer.CheckingAccounts.Count + _CurrentCustomer.BusinessAccounts.Count) >= 2)
        //    {
        //        Console.WriteLine("What type of account are you trying to transfer from?");
        //        Console.WriteLine("1. Checking Account");
        //        Console.WriteLine("2. Business Account");
        //        var selection = Convert.ToInt32(Console.ReadLine());
        //        switch (selection)
        //        {
        //            case (int)_OpenAccountSelector.Checking:
        //                if (_CurrentCustomer.CheckingAccounts != null)
        //                {
        //                    Console.WriteLine("Checking Accounts:");
        //                    foreach (var account in _CurrentCustomer.CheckingAccounts)
        //                    {
        //                        Console.WriteLine($"Account Number: { account.AccountNumber }| Balance: {account.Funds}");
        //                    }
        //                    Console.WriteLine("What is the account number?");
        //                }
        //                else
        //                {
        //                    Console.WriteLine("You dont have any Checking Account!");
        //                    Thread.Sleep(1000);
        //                    TransferScreen();
        //                }
        //                break;
        //        }
        //        //Console.Clear();
        //        //Console.WriteLine("+++Transfer Request+++");
        //        //Console.WriteLine("Please select the account you want to transfer from.");
        //        //DisplayAllCustomerAccounts();
        //        //Console.WriteLine("Please enter Account Number: ");
        //        //var selection = Convert.ToInt32(Console.ReadLine());
        //        //_CurrentCustomer.
        //        ////transferlist.Add()

        //        //// foreach ()
        //    }
        //    else
        //    {
        //        Console.WriteLine("Sorry you do not have enough accounts to start a transfer!");
        //        Thread.Sleep(1000);
        //        CustomerHomeScreen();
        //    }

        //}
        public static void DisplayAllCustomerAccountsScreen()
        {
            Console.Clear();
            Console.WriteLine($"{ _CurrentCustomer.FirstName }, these are your accounts.");
            
            Console.WriteLine("         +++Checking Accounts+++");
            foreach (var account in _CurrentCustomer.Accounts.OfType<CheckingAccount>())
            {
                Console.WriteLine($"Account Number: { account.AccountNumber } Funds: { account.Funds } " +
                                  $"Current Interest Rate: { account.InterestRate }");
            }
            Console.WriteLine("         +++Business Accounts+++");
            foreach (var account in _CurrentCustomer.Accounts.OfType<BusinessAccount>())
            {
                Console.WriteLine($"Account Number: { account.AccountNumber } Funds: { account.Funds } " +
                                  $"Current Interest Rate: { account.InterestRate }");
            }
            Console.WriteLine("Press enter to go back to home screen.");
            Console.ReadLine();
            CustomerHomeScreen();
        }
        private static void OpenBusinessAccount()
        {
            var account = new BusinessAccount()
            {
                AccountNumber = _BusinessAccountNumber,
                Funds = 0.0M,
                InterestRate = 0.0
            };
            //_CurrentCustomer.BusinessAccounts.Add(account);
            _CurrentCustomer.Accounts.Add(account);
            _BusinessAccountNumber++;
            CustomerHomeScreen();
        }
        private static void OpenCheckingAccount()
        {
            var account = new CheckingAccount()
            {
                AccountNumber = _CheckingAccountNumber,
                Funds = 0.0M,
                InterestRate = 0.0
            };
           // _CurrentCustomer.CheckingAccounts.Add(account);
            _CurrentCustomer.Accounts.Add(account);
            _CheckingAccountNumber++;
            CustomerHomeScreen();
        }
        private static void CloseAccount(int accountnumber)
        {
            var account = AccountSelector(accountnumber);
            _CurrentCustomer.Accounts.Remove(account);
        }
        public static void ProcessTransfer()
        { 
            
        }
        public static void DisplayAllCustomerAccounts()
        {
            
            Console.WriteLine($"{ _CurrentCustomer.FirstName }, these are your accounts.");
                
                Console.WriteLine("         +++Checking Accounts+++");
                foreach (var account in _CurrentCustomer.Accounts.OfType<CheckingAccount>())
                {
                    Console.WriteLine($"Account Number: { account.AccountNumber } Funds: { account.Funds } " +
                                      $"Current Interest Rate: { account.InterestRate }");
                }
                Console.WriteLine("         +++Business Accounts+++");
                foreach (var account in _CurrentCustomer.Accounts.OfType<BusinessAccount>())
                {
                    Console.WriteLine($"Account Number: { account.AccountNumber } Funds: { account.Funds } " +
                                      $"Current Interest Rate: { account.InterestRate }");
                }
            
        }
        public static IAccount AccountSelector(int accountnumber)
        {
            var account = _CurrentCustomer.Accounts.First(account => account.AccountNumber == accountnumber);
            return account;
        }
        public static void DepositAccount(IAccount account, decimal amt)
        {
            account.Deposit(amt);
        }
        private static bool HaveAccounts()
        {
            var result = false;
            if (_CurrentCustomer.Accounts.Count > 0)
            {
                result = true;
            }
            return result;
        }
        private static void CheckAccountsExist()
        {
            if (!HaveAccounts())
            {
                Console.WriteLine("No accounts found create an account. Try again");
                Thread.Sleep(2000);
                CustomerHomeScreen();
            }
        }
        private static bool VerifyAmount(string input, out decimal amount)
        {
           
            //bool checknumber = Int32.TryParse(input, out selection);
            //input = Console.ReadLine();
            bool checknumber = Decimal.TryParse(input, out amount);
            return checknumber;
        }
        public static bool AccountNumberVerifier(int accountnumber)
        {
            var status = false;
            status = _CurrentCustomer.Accounts.Any(account => account.AccountNumber == accountnumber);
            return status;
            
        }
    }
}
