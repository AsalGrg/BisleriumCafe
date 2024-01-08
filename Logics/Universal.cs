using Coursework1Development.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Coursework1Development.Logics
{
    internal class Universal
    {
        public static List<Product> GetAllProducts()
        {
            List<Product> products;

            string filePath = FilePaths.AllProductsData();

            String allProductsText = File.ReadAllText(filePath);

            products = JsonSerializer.Deserialize<List<Product>>(allProductsText);

            if (products == null || products.Count == 0)
            {
                return [];
            }

            return products;

        }

        public static List<AddIns> GetallAddins()
        {
            List<AddIns> addIns;

            string filePath = FilePaths.AllAddinsData();

            String allAddInsText = File.ReadAllText(filePath);

            addIns = JsonSerializer.Deserialize<List<AddIns>>(allAddInsText);

            if (addIns == null || addIns.Count == 0)
            {
                return [];
            }

            return addIns;

        }


        public static List<Customer> GetAllMembers()
        {

            string jsonInText = File.ReadAllText(FilePaths.AllCustomers());

            List<Customer> customers = JsonSerializer.Deserialize<List<Customer>>(jsonInText) ?? [];

            return customers;

        }

        public static List<Transactions> GetTransactionHistory()
        {
            string jsonInText = File.ReadAllText(FilePaths.AllTransactions());

            List<Transactions> allTransactions = JsonSerializer.Deserialize<List<Transactions>>(jsonInText) ?? [];

            return allTransactions;
        }


        public static List<NoOfTransactionHistory> GetAllNoOfTransactionHistory()
        {
            string jsonInText = File.ReadAllText(FilePaths.AllNoOfTransactionHistory());

            List<NoOfTransactionHistory> noOfTransactionHistories = JsonSerializer.Deserialize<List<NoOfTransactionHistory>>(jsonInText) ?? [];

            return noOfTransactionHistories;
        }

        public static List<RegularCustomer> GetRegularCustomersDetails()
        {
            string regularCustomerDetails = FilePaths.RegularCustomersDetails();
            string regularCustomerDataInText = File.ReadAllText(regularCustomerDetails);
            List<RegularCustomer> regularCustomers = JsonSerializer.Deserialize<List<RegularCustomer>>(regularCustomerDataInText);
            return regularCustomers;
        }
    }
}
