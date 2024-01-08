using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1Development
{
    internal class FilePaths
    {
        private static String commonDirectoryPath = "C:\\Users\\Lenovo\\Documents\\MODULES\\Application Development";

        public static String UserData()
        {
            return Path.Combine(commonDirectoryPath, "users.json");
        }

        public static String AllProductsData()
        {
            return Path.Combine(commonDirectoryPath, "allProducts.json");
        }

        public static String AllAddinsData()
        {
            return Path.Combine(commonDirectoryPath, "allAddins.json");
        }

        public static String AllCustomers() {
            return Path.Combine(commonDirectoryPath, "customers.json");
        }

        public static String AllTransactions()
        {
            return Path.Combine(commonDirectoryPath, "transactions.json");
        }

        public static String AllNoOfTransactionHistory()
        {
            return Path.Combine(commonDirectoryPath, "userTotalTransaction.json");
        }

        public static String RegularCustomersDetails()
        {
            return Path.Combine(commonDirectoryPath, "regularCustomers.json");
        }

    }
}
