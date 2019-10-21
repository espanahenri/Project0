using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public static class UserInterface
    {
        private static int _CustomerID = 1000;
        private static List<Customer> _Customers = new List<Customer>();
        private static int _CheckingAccountNumber = 2000;
        private static int _BusinessAccountNumber = 3000;
        private static Customer _CurrentCustomer = new Customer();

        #region Register Page
        public static void Register()
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
            OpenAccount();
            CustomerHomeScreen(_CurrentCustomer);
        }
        #endregion
        public static void CustomerHomeScreen(Customer customer)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to Revature Bank { customer.FirstName } {customer.LastName}!");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Open new account");
            Console.WriteLine("3. Close account");
            Console.WriteLine("4. Deposit");
            Console.WriteLine("5. Transfer");
            Console.WriteLine("6. List of all accounts");

        }
        public static void OpenAccount()
        {
            Console.Clear();
            Console.WriteLine("What type of account would you like to open?");
            Console.WriteLine("Checking Account - 1");
            Console.WriteLine("Business Account - 2");
            var selection = Convert.ToInt32(Console.ReadLine());
            if (selection == 1)
            {
                OpenCheckingAccount();
            }
        }
        private static void OpenCheckingAccount()
        {
            var account = new CheckingAccount()
            {
                AccountNumber = _CheckingAccountNumber,
                Funds = 0.0M,
                InterestRate = 0.0
            };
            _CurrentCustomer.Accounts.Add(account);
            _CheckingAccountNumber++;
        }
        public static void DisplayAllCustomers()
        {
            foreach (var customer in _Customers)
            {
                Console.WriteLine($"First Name: { customer.FirstName } Last Name: { customer.LastName } CustomerID: { customer.ID}");
                Console.WriteLine("+++Checking Accounts+++");
                foreach (var account in customer.Accounts)
                {
                    Console.WriteLine($"Account Number: { account.AccountNumber } Funds: { account.Funds } " +
                                      $"Current Interest Rate: { account.InterestRate }");
                }
                Console.WriteLine("+++Business Accounts+++");
            }
        }
    }
}
