using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
namespace Project0
{
    public delegate void ScreenDelegate();
    /// <summary>
    /// User interface controls and handles all banking operations for a single customer at a time.
    /// Which include keeping track of current customer and actions of current customer.
    /// Data validation is also handled in this class depending on the request a specific
    /// function is called.
    /// </summary>
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
            PayOverdraft = 9,
            Exit = 10
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
                LastName = lastname,
                isActive = true
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
            Console.WriteLine("9. Pay Overdraft Facility");
            Console.WriteLine("10. Exit");
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
                case (int)_HomeScreenSelector.DisplayTransaccions:
                    DisplayTransactionAccountScreen();
                    break;
                case (int)_HomeScreenSelector.PayOverdraft:
                    PayOverdraftScreen();
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
            var del = new ScreenDelegate(DepositScreen);
            decimal amount;
            int accountnumber;
            CheckAccountsExist();
            Console.WriteLine("+++Deposit+++");
            Console.WriteLine("Select account you want to deposit into:");
            DisplayAllCustomerAccounts();
            Console.WriteLine("Please type in account number you want to deposit in to.");
            var input = Console.ReadLine();
            AccountNumberVerifier(input, out accountnumber,del);
            AccountNumberExist(accountnumber, del);
            var account = (IAccountV1)AccountSelector(accountnumber);
            Console.WriteLine("How much do you want to deposit?");
            input = Console.ReadLine();
            VerifyAmount(input, out amount, del);
            
            DepositAccount(account, amount);
            var trans = new Transaction()
            {
                TransactionID = _TransactionID,
                TimeOfTransaction = DateTime.Now,
                Amount = amount,
                TypeOfTransaction = "Deposit",
                AccountNumber = account.AccountNumber
            };
            if (account.Transactions == null)
            {
                account.Transactions = new List<Transaction>();
            }
            account.Transactions.Add(trans);
            _TransactionID++;
            Console.WriteLine($"Thank you, your deposit of ${amount} for account: {account.AccountNumber } was successful");
            Console.WriteLine("Please press enter to go back home.");
            Console.ReadLine();
            CustomerHomeScreen();     
        }
        public static void WithdrawScreen()
        {
            Console.Clear();

            var del = new ScreenDelegate(WithdrawScreen);
            decimal amount = 0.0m;
            int accountnumber;

            CheckAccountsExist();
            Console.WriteLine(" +++WITHDRAW+++");
            Console.WriteLine("Select the account");
            DisplayAllCustomerAccounts();
            Console.WriteLine("Type the number of the account.");
            var input = Console.ReadLine();
            AccountNumberVerifier(input, out accountnumber, del);
            AccountNumberExist(accountnumber, del);
            var account = (IAccountV1)AccountSelector(accountnumber);
            Console.WriteLine("How much would you like to withdraw?");
            input = Console.ReadLine();
            VerifyAmount(input, out amount, del);
            account.Withdraw(amount);
            Console.WriteLine($"Thank you, your withdraw of ${amount} for account: {account.AccountNumber } was successful");
            Console.WriteLine("Please press enter to go back home.");
            Console.ReadLine();
            CustomerHomeScreen();
        }
        public static void CloseAccountScreen()
        {
            Console.Clear();
            int accountnumber;
            var del = new ScreenDelegate(CloseAccountScreen);
            CheckAccountsExist();
            Console.WriteLine("+++Close Account+++");
            DisplayAllCustomerAccounts();
            Console.WriteLine("Type in the account number you want to close");
            var input = Console.ReadLine();
            AccountNumberVerifier(input, out accountnumber, del);
            AccountNumberExist(accountnumber, del);
            CloseAccount(accountnumber);
            Console.WriteLine("You have successfully closed your account.");
            Console.WriteLine("Press Enter to go home.");
            Console.ReadLine();
            CustomerHomeScreen();
        }
        public static void PayOverdraftScreen()
        {
            Console.Clear();
            decimal amount;
            var del = new ScreenDelegate(PayOverdraftScreen);
            if (_CurrentCustomer.OverdraftFacilityDue > 0) 
            {
                Console.WriteLine("+++Pay Overdraft Facility+++");
                Console.WriteLine($"This is how much you owe: ${ _CurrentCustomer.OverdraftFacilityDue}");
                Console.WriteLine("How much would you like to pay?");
                var input = Console.ReadLine();
                VerifyAmount(input, out amount, del);
                if (amount > _CurrentCustomer.OverdraftFacilityDue)
                {
                    Console.WriteLine("Can't pay more than what you owe!");
                    Console.WriteLine("Press enter to continue.");
                    Console.ReadLine();
                    del();
                }
                _CurrentCustomer.OverdraftFacilityDue -= amount;
                Console.WriteLine("Thank you for your payment!");
                Console.WriteLine("Press enter to go back home");
                Console.ReadLine();
                CustomerHomeScreen();
            }
            Console.WriteLine("You dont owe any Overdraft Facilities.");
            Console.WriteLine("Press enter to go back home");
            Console.ReadLine();
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
                Console.WriteLine($"Account Number: { account.AccountNumber } Balance: ${ account.Balance } " +
                                  $"Current Interest Rate: { account.InterestRate }");
            }
            Console.WriteLine("         +++Business Accounts+++");
            foreach (var account in _CurrentCustomer.Accounts.OfType<BusinessAccount>())
            {
                Console.WriteLine($"Account Number: { account.AccountNumber } Balance: ${ account.Balance } " +
                                  $"Current Interest Rate: { account.InterestRate }");
            }
            Console.WriteLine("Press enter to go back to home screen.");
            Console.ReadLine();
            CustomerHomeScreen();
        }
        public static void DisplayTransactionAccountScreen()
        {
            Console.Clear();
            var del = new ScreenDelegate(DisplayTransactionAccountScreen);
            int accountnumber;
            Console.WriteLine("+++Transactions+++");
            DisplayAllCustomerAccounts();
            Console.WriteLine("Please select the account you want to see the transactions for.");
            var input = Console.ReadLine();
            AccountNumberVerifier(input,out accountnumber, del);
            var account = AccountSelector(accountnumber);
            foreach (var transaction in account.Transactions)
            {
                Console.WriteLine($"Transaction Id: {transaction.TransactionID} Type of Transaction: {transaction.TypeOfTransaction} " +
                                  $"Time: {transaction.TimeOfTransaction} Amount: ${transaction.Amount}");
            }
            Console.WriteLine("Please press enter to go back home.");
            Console.ReadLine();
            CustomerHomeScreen();
        }
        private static void OpenBusinessAccount()
        {
            var account = new BusinessAccount()
            {
                AccountNumber = _BusinessAccountNumber,
                Balance = 0.0M,
                InterestRate = 0.0,
                isActive = true
            };
            
            _CurrentCustomer.Accounts.Add(account);
            _BusinessAccountNumber++;
            CustomerHomeScreen();
        }
        private static void OpenCheckingAccount()
        {
            var account = new CheckingAccount()
            {
                AccountNumber = _CheckingAccountNumber,
                Balance = 0.0M,
                InterestRate = 0.0,
                isActive = true
            };
            _CurrentCustomer.Accounts.Add(account);
            _CheckingAccountNumber++;
            CustomerHomeScreen();
        }
        private static void CloseAccount(int accountnumber)
        {
            var account = AccountSelector(accountnumber);
            _CurrentCustomer.Accounts.Remove(account);
        }
        public static void DisplayAllCustomerAccounts()
        {
            
            Console.WriteLine($"{ _CurrentCustomer.FirstName }, these are your accounts.");
                
                Console.WriteLine("         +++Checking Accounts+++");
                foreach (var account in _CurrentCustomer.Accounts.OfType<CheckingAccount>())
                {
                    Console.WriteLine($"Account Number: { account.AccountNumber } Balance: ${ account.Balance } " +
                                      $"Current Interest Rate: { account.InterestRate }");
                }
                Console.WriteLine("         +++Business Accounts+++");
                foreach (var account in _CurrentCustomer.Accounts.OfType<BusinessAccount>())
                {
                    Console.WriteLine($"Account Number: { account.AccountNumber } Balance: ${ account.Balance } " +
                                      $"Current Interest Rate: { account.InterestRate }");
                }
            
        }
        public static IAccount AccountSelector(int accountnumber)
        {
            var account = _CurrentCustomer.Accounts.First(account => account.AccountNumber == accountnumber);
            return account;
        }
        public static void DepositAccount(IAccountV1 account, decimal amt)
        {
            account.Deposit(amt);
        }
        private static void CheckAccountsExist()
        {
            if (!(_CurrentCustomer.Accounts.Count > 0))
            {
                Console.WriteLine("No accounts found please create an account. Try again.");
                Thread.Sleep(2000);
                CustomerHomeScreen();
            }
        }
        private static void VerifyAmount(string input, out decimal amount,ScreenDelegate del)
        {
            bool checknumber = Decimal.TryParse(input, out amount);
            if (amount < 0)
            {
                Console.WriteLine("Amount cannot be less than zero.");
                Thread.Sleep(2000);
                del();
            }
            if (checknumber == false)
            {
                Console.WriteLine("Not a correct value. Try again.");
                Thread.Sleep(2000);
                del();
            }
        }
        private static void AccountNumberVerifier(string input, out int accountnumber, ScreenDelegate del)
        {
            bool checknumber = int.TryParse(input, out accountnumber);
            if (checknumber == false )
            {
                Console.WriteLine("Not a correct value. Try again");
                Thread.Sleep(2000);
                del();
            }
        }
        public static void AccountNumberExist(int accountnumber, ScreenDelegate del)
        {
                
            if (!(_CurrentCustomer.Accounts.Any(account => account.AccountNumber == accountnumber)))
            {
                Console.WriteLine("Account number not found. Try again.");
                Thread.Sleep(2000);
                del();
            }
        }
    }
}
