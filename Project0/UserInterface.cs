using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public static class UserInterface
    {
        private static int _CustomerID = 1000;
        private static List<Customer> _Customers = new List<Customer>();
        public static void Register()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Revature Bank");
            Console.WriteLine("Please Register Now: ");
            Console.WriteLine("First Name: ");
            var firstname = Console.ReadLine();
            Console.WriteLine("Last Name: ");
            var lastname = Console.ReadLine();
            var customer = new Customer()
            {
                ID = _CustomerID,
                FirstName = firstname,
                LastName = lastname
            };
            _Customers.Add(customer);
            _CustomerID++;
        }
        public static void HomeScreen()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Revature Bank");
            Console.WriteLine("1. Register");

        }
        public static void DisplayAllCustomers()
        {
            foreach (var customer in _Customers)
            {
                Console.WriteLine($"First Name: { customer.FirstName } Last Name: { customer.LastName } CustomerID: { customer.ID}");

            }
        }
    }
}
