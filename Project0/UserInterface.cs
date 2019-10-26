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
        private static int _TermDepositID = 6000;
        private static List<Customer> _Customers = new List<Customer>();
        public static List<IAccount> transferlist = new List<IAccount>();
        private static int _CheckingAccountNumber = 2000;
        private static int _BusinessAccountNumber = 3000;
        public static Customer _CurrentCustomer = new Customer();
        enum _OpenAccountSelector { Checking=1, Business=2, Loan=3,TermDeposit=4 };
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
            PayLoanInstallement = 10,
            Exit = 11
        };

        #region Register Page
        public static void RegisterScreen()
        {
            Console.Clear();    
            Console.WriteLine("Welcome to Revature Bank");
            Console.WriteLine("Please Register Now: ");
            Console.WriteLine("First Name: ");
            var firstname = Console.ReadLine();
            while (string.IsNullOrEmpty(firstname))
            {
                Console.WriteLine("Name can't be empty! Input your name once more");
                firstname = Console.ReadLine();
            }
            Console.WriteLine("Last Name: ");
            var lastname = Console.ReadLine();
            while (string.IsNullOrEmpty(lastname))
            {
                Console.WriteLine("Name can't be empty! Input your name once more");
                lastname = Console.ReadLine();
            }
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
            Console.WriteLine("10. Pay Loan Installment");
            Console.WriteLine("11. Exit");
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
                case (int)_HomeScreenSelector.Transfer:
                    TransferScreen();
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
                case (int)_HomeScreenSelector.PayLoanInstallement:
                    PayLoanInstallmentScreen();
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
            Console.WriteLine("Loan - 3");
            Console.WriteLine("Term Deposit (CD) - 4");
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
                case (int)_OpenAccountSelector.Loan:
                    OpenLoan();
                    break;
                case (int)_OpenAccountSelector.TermDeposit:
                    OpenTermDeposit();
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
            IntegerNumberVerifier(input, out accountnumber,del);
            AccountNumberExist(accountnumber, del);
            var account = (IAccountV1)AccountSelector(accountnumber);
            Console.WriteLine("How much do you want to deposit?");
            input = Console.ReadLine();
            VerifyAmount(input, out amount, del);

            account.Deposit(amount);
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
            IntegerNumberVerifier(input, out accountnumber, del);
            AccountNumberExist(accountnumber, del);
            var account = (IAccountV1)AccountSelector(accountnumber);
            Console.WriteLine("How much would you like to withdraw?");
            input = Console.ReadLine();
            VerifyAmount(input, out amount, del);
            account.Withdraw(amount);
            if (account is TermDeposit)
            {
                CloseAccount(accountnumber);
            }
            var trans = new Transaction()
            {
                TransactionID = _TransactionID,
                TimeOfTransaction = DateTime.Now,
                Amount = amount,
                TypeOfTransaction = "Withdraw",
                AccountNumber = account.AccountNumber
            };
            if (account.Transactions == null)
            {
                account.Transactions = new List<Transaction>();
            }
            account.Transactions.Add(trans);
            _TransactionID++;
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
            IntegerNumberVerifier(input, out accountnumber, del);
            AccountNumberExist(accountnumber, del);
            if (AccountSelector(accountnumber) is Loan)
            {
                if (((Loan)(AccountSelector(accountnumber))).FullBalance > 0)
                {
                    Console.WriteLine("You cant close a loan that you still owe money.");
                    Thread.Sleep(2000);
                    del();
                }
            }
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
        public static void PayLoanInstallmentScreen()
        {
            Console.Clear();
            CheckAccountsExist();
            CheckNumberOfLoanAccounts();
            var del = new ScreenDelegate(PayLoanInstallmentScreen);
            decimal amount = 0.0m;
            int accountnumber;
            Console.WriteLine("+++Loan Installment Payment+++");
            DisplayAllCustomerAccounts();
            Console.WriteLine("Type the account number of the loan you want to pay.");
            var input = Console.ReadLine();
            IntegerNumberVerifier(input, out accountnumber, del);
            AccountNumberExist(accountnumber, del);
            if (!(AccountSelector(accountnumber) is Loan))
            {
                Console.WriteLine("Sorry that is not a Loan Account. Try Again");
                Console.WriteLine("Press enter to try again.");
                Console.ReadLine();
                del();
            }
            Loan loanaccount = (Loan)AccountSelector(accountnumber);
            Console.WriteLine($"This is your monthly due: ${loanaccount.InstallmentAmount.ToString("F")}");
            Console.WriteLine("How much would you like to pay?");
            input = Console.ReadLine();
            VerifyAmount(input, out amount, del);
            if (amount > loanaccount.FullBalance)
            {
                Console.WriteLine("Cannot pay more than what you currently owe.");
                Thread.Sleep(2000);
                del();
            }
            if (amount == loanaccount.FullBalance)
            {
                Console.WriteLine("Congratulations you have paid your full loan.");
                Thread.Sleep(2000);
                loanaccount.isActive = false;
                CloseAccount(accountnumber);
            }
            loanaccount.FullBalance -= amount;
            var tran = new Transaction()
            {
                TransactionID = _TransactionID,
                TimeOfTransaction = DateTime.Now,
                Amount = amount,
                TypeOfTransaction = "Payment",
                AccountNumber = loanaccount.AccountNumber
            };
            loanaccount.Transactions.Add(tran);
            Console.WriteLine("Thank you for your payment!");
            Console.WriteLine("Press enter to go back to home screen.");
            Console.ReadLine();
            CustomerHomeScreen();
        }
        public static void TransferScreen()
        {
            Console.Clear();
            CheckAccountsExist();
            CheckNumberOfAccounts();
            var del = new ScreenDelegate(TransferScreen);
            decimal amount = 0.0m;
            int accountnumberfrom;
            int accountnumberto;
            Console.WriteLine("+++Transfer+++");
            DisplayAllCustomerAccounts();
            Console.WriteLine("Type the account number where you want to transfer from.");
            var input = Console.ReadLine();
            IntegerNumberVerifier(input, out accountnumberfrom, del);
            AccountNumberExist(accountnumberfrom, del);
            CheckLoanType(accountnumberfrom, del);
            Console.WriteLine("Type the account number where you want to transfer to.");
            input = Console.ReadLine();
            IntegerNumberVerifier(input, out accountnumberto, del);
            AccountNumberExist(accountnumberto, del);
            CheckLoanType(accountnumberto, del);
            if (accountnumberfrom == accountnumberto)
            {
                Console.WriteLine("Sorry you cannot transfer to the same account.");
                Console.WriteLine("Press enter to try again.");
                Console.ReadLine();
                del();
            }
            Console.WriteLine("How much would you like to transfer");
            input = Console.ReadLine();
            VerifyAmount(input, out amount, del);
            var accfrom = (IAccountV1)AccountSelector(accountnumberfrom);
            var accto = (IAccountV1)AccountSelector(accountnumberto);
            accfrom.Withdraw(amount);
            if (accfrom is TermDeposit)
            {
                CloseAccount(accfrom.AccountNumber);
            }
            accto.Deposit(amount);
            var transfrom = new Transaction()
            {
                TransactionID = _TransactionID,
                TimeOfTransaction = DateTime.Now,
                Amount = amount,
                TypeOfTransaction = "Withdraw",
                AccountNumber = accfrom.AccountNumber
            };
            if (accfrom.Transactions == null)
            {
                accfrom.Transactions = new List<Transaction>();
            }
            accfrom.Transactions.Add(transfrom);
            _TransactionID++;
            var transto = new Transaction()
            {
                TransactionID = _TransactionID,
                TimeOfTransaction = DateTime.Now,
                Amount = amount,
                TypeOfTransaction = "Deposit",
                AccountNumber = accfrom.AccountNumber
            };
            if (accto.Transactions == null)
            {
                accto.Transactions = new List<Transaction>();
            }
            accto.Transactions.Add(transto);
            _TransactionID++;
            Console.WriteLine("Transfer Complete!");
            Console.WriteLine("Press enter to go back to home screen.");
            Console.ReadLine();
            CustomerHomeScreen();
        }
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
            Console.WriteLine("         +++Loan Accounts+++");
            foreach (var account in _CurrentCustomer.Accounts.OfType<Loan>())
            {
                Console.WriteLine($"Loan Number: { account.AccountNumber } What you owe: ${ account.FullBalance.ToString("F") } " +
                                  $"Interest Rate: { account.InterestRate * 100 }% Term: {account.Term}years Monthly Payments: ${account.InstallmentAmount.ToString("F")}");
            }
            Console.WriteLine("         +++Term Deposit Accounts+++");
            foreach (var account in _CurrentCustomer.Accounts.OfType<TermDeposit>())
            {
                Console.WriteLine($"Account Number: { account.AccountNumber } Balance: ${ account.FullBalance } " +
                                  $"Interest Rate: { account.InterestRate * 100 }% Term: {account.Term}years Maturity Date: {account.Maturity}");
            }
            Console.WriteLine("Press enter to go back to home screen.");
            Console.ReadLine();
            CustomerHomeScreen();
        }
        public static void DisplayTransactionAccountScreen()
        {
            Console.Clear();
            CheckAccountsExist();
            var del = new ScreenDelegate(DisplayTransactionAccountScreen);
            int accountnumber;
            Console.WriteLine("+++Transactions+++");
            DisplayAllCustomerAccounts();
            Console.WriteLine("Please select the account you want to see the transactions for.");
            var input = Console.ReadLine();
            IntegerNumberVerifier(input,out accountnumber, del);
            AccountNumberExist(accountnumber, del);
            var account = AccountSelector(accountnumber);
            foreach (var transaction in account.Transactions)
            {
                Console.WriteLine($"Transaction Id: {transaction.TransactionID}|Type of Transaction: {transaction.TypeOfTransaction}" +
                                  $"|Time: {transaction.TimeOfTransaction}|Amount: ${transaction.Amount}");
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
        private static void OpenTermDeposit()
        {
            Console.Clear();
            decimal amount;
            int years;
            var del = new ScreenDelegate(OpenTermDeposit);
            Console.WriteLine("How much would you like to deposit?");
            var input = Console.ReadLine();
            VerifyAmount(input, out amount, del);
            Console.WriteLine("How long do want your term to be(years)?");
            input = Console.ReadLine();
            IntegerNumberVerifier(input, out years, del);
            VerifyYear(years, del);
            var account = new TermDeposit()
            {
                AccountNumber = _TermDepositID,
                Balance = amount,
                Term = years,
                Maturity = DateTime.Now.AddYears(years),
                //Maturity = DateTime.Now,
                isActive = true,
                InterestRate = 0.10,
                Transactions = new List<Transaction>(),
            };
            account.Deposit(amount);
            _CurrentCustomer.Accounts.Add(account);
            var tran = new Transaction()
            {
                TransactionID = _TransactionID,
                TypeOfTransaction = "Deposit",
                Amount = amount,
                TimeOfTransaction = DateTime.Now,
                AccountNumber = _TermDepositID
            };
            AccountSelector(_TermDepositID).Transactions.Add(tran);
            _TermDepositID++;
            Console.WriteLine($"You have created a new CD which will mature on {account.Maturity}");
            Console.WriteLine("Press enter to go back home");
            Console.ReadLine();
            CustomerHomeScreen();
        }
        private static void OpenLoan()
        {
            Console.Clear();
            decimal amount;
            int years;
            var del = new ScreenDelegate(OpenLoan);
            Console.WriteLine("How much would you like to loan?");
            var input = Console.ReadLine();
            VerifyAmount(input, out amount, del);
            Console.WriteLine("How long do want your term to be(years)?");
            input = Console.ReadLine();
            IntegerNumberVerifier(input, out years, del);
            VerifyYear(years, del);
            var account = new Loan()
            {
                AccountNumber = _LoanID,
                Balance = amount,
                Term = years,
                isActive = true,
                InterestRate = 0.10,
                Transactions = new List<Transaction>() 
            };
            account.FullBalance = amount + (decimal)((double)amount * account.InterestRate);
            account.InstallmentAmount = (decimal)(account.FullBalance / (account.Term*12));
            _CurrentCustomer.Accounts.Add(account);
            _LoanID++;
            Console.WriteLine($"You have taken out a new loan for ${account.Balance} with a monthly payment " +
                $"of ${account.InstallmentAmount.ToString("F")} for {account.Term} years.");
            Console.WriteLine("Press enter to go back home");
            Console.ReadLine();
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
            Console.WriteLine("         +++Loan Accounts+++");
            foreach (var account in _CurrentCustomer.Accounts.OfType<Loan>())
            {
                Console.WriteLine($"Loan Number: { account.AccountNumber } What you owe: ${ account.FullBalance.ToString("F") } " +
                                  $"Interest Rate: { account.InterestRate * 100 }% Term: {account.Term} years Monthly Payments: ${account.InstallmentAmount.ToString("F")}");
            }
            Console.WriteLine("         +++Term Deposit Accounts+++");
                foreach (var account in _CurrentCustomer.Accounts.OfType<TermDeposit>())
                {
                    Console.WriteLine($"Account Number: { account.AccountNumber } Balance: ${ account.FullBalance } " +
                                      $"Interest Rate: { account.InterestRate * 100 }% Term: {account.Term} years Maturity Date: {account.Maturity}");
                }

        }
        public static IAccount AccountSelector(int accountnumber)
        {
            var account = _CurrentCustomer.Accounts.First(account => account.AccountNumber == accountnumber);
            return account;
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
        private static void CheckLoanType(int accountnumber, ScreenDelegate del)
        {
            if (AccountSelector(accountnumber) is Loan)
            {
                Console.WriteLine("You cannot transfer from or to a loan. Try again.");
                Thread.Sleep(2000);
                del();
            }
        }
        private static void CheckNumberOfAccounts()
        {
            var numofbusaccounts = _CurrentCustomer.Accounts.OfType<BusinessAccount>().Count();
            var numofcheaccounts = _CurrentCustomer.Accounts.OfType<CheckingAccount>().Count();
            var numofcdaccounts = _CurrentCustomer.Accounts.OfType<TermDeposit>().Count();
            if ((numofcheaccounts + numofbusaccounts + numofcdaccounts) < 2)
            {
                Console.WriteLine("You dont have enough accounts to perform a transfer. Go open some.");
                Thread.Sleep(2000);
                CustomerHomeScreen();
            }
        }
        private static void CheckNumberOfLoanAccounts()
        {
            var numofloanaccounts = _CurrentCustomer.Accounts.OfType<Loan>().Count();
            if( numofloanaccounts < 1)
            {
                Console.WriteLine("You dont have any loans at this time.");
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
        private static void IntegerNumberVerifier(string input, out int accountnumber, ScreenDelegate del)
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
        private static void VerifyYear(int year, ScreenDelegate del)
        {
            if (year <= 0)
            {
                Console.WriteLine("Invalid Value.");
                Thread.Sleep(2000);
                del();
            }
        }
    }      
}
