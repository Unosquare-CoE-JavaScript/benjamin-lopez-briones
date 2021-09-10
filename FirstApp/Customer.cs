using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp
{
    public class Customer
    {
        public string Name => "Benjamin";
        public int Age => 23;

        public int GetOrdersByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("No name");
            }

            return 100;
        }

        public string GetFullName(string firstName, string lastName)
        {
            return $"{firstName} {lastName}";
        }
    }

    public class LoyalCustomer : Customer
    {
        public int Discount { get; set; }

        public LoyalCustomer()
        {
            Discount = 10;
        }
    }

    public static class CustomerFactory
    {
        public static Customer CreateCustomerInstance(int orderCount)
        {
            if (orderCount <= 100)
                return new Customer();
            else
                return new LoyalCustomer();
        }
    }
}
