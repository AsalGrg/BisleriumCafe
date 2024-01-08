using Coursework1Development.Models;
using Coursework1Development.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;
using HealthKit;
using Intents;
using Foundation;



namespace Coursework1Development.Logics
{
    internal class StaffTasks
    {


        static Customer GetCustomerDetailByUsernameOrPhoneNumber(string phoneNumberOrUsername)
        {
            List<Customer> customers = Universal.GetAllMembers();

            Customer customer = customers.FirstOrDefault(customer => customer.Name == phoneNumberOrUsername || customer.PhoneNumber == phoneNumberOrUsername);

            return customer;
        }

        public static string CheckMemberValidity(string usernameOrPhoneNumber)
        {


            Customer customer = GetCustomerDetailByUsernameOrPhoneNumber(usernameOrPhoneNumber);

            //if customer is valid
            if (customer != null)
            {
                return null;
            }
            return "Invalid username or phone number";
        }

        public static bool AddNewCustomer(string username, string email, string phoneNumber)
        {
            try
            {

                List<Customer> customers = Universal.GetAllMembers();

                Customer customer = new Customer()
                {
                    ID = Guid.NewGuid(),
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Name = username,
                    AddedBy = "Staff",//static for now
                    AddedDate = DateTime.Now
                };

                customers.Add(customer);

                string savedJson = JsonSerializer.Serialize(customers);

                File.WriteAllText(FilePaths.AllCustomers(), savedJson);

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error adding customer: {ex.Message}");
                return false;
            }

        }

        public static void Checked(List<ProductOrderRequest> products)
        {
            File.WriteAllText("C:\\Users\\Lenovo\\Documents\\requirements.txt", JsonSerializer.Serialize(products));
        }

        //method to proceed order of member or guest.
        static bool ProceedOrderUniversal(List<ProductOrderRequest> products, List<ProductOrderRequest> addIns, string customerName)
        {
            if (addIns == null)
            {
                return SaveTransaction(
                new()
                {
                    Id = Guid.NewGuid(),
                    CustomerName = customerName,
                    TransactionAmount = GetTotalTransactionAmount(products, null),
                    OrderedAddOnsDetails = null,
                    OrderProductDetails = ChangeToEachProductDetailListForProduct(products),
                    TransactionDate = DateTime.Now
                });
            }

            return SaveTransaction(
                new()
                {
                    Id = Guid.NewGuid(),
                    CustomerName = customerName,
                    TransactionAmount = GetTotalTransactionAmount(products, addIns),
                    OrderedAddOnsDetails = ChangeToEachProductDetailListForAddIns(addIns),
                    OrderProductDetails = ChangeToEachProductDetailListForProduct(products),
                    TransactionDate = DateTime.Now
                });


        }

        public static bool ProceedGuestOrder(List<ProductOrderRequest> products, List<ProductOrderRequest> addIns)
        {
            return ProceedOrderUniversal(products, addIns, "Guest");

        }


        static bool SaveTransaction(Transactions transactionsToBeAdded)
        {
            try
            {
                string transactionsDataFilePath = FilePaths.AllTransactions();
                string allTransactionDataText = File.ReadAllText(transactionsDataFilePath);

                List<Transactions> allTransactions = JsonSerializer.Deserialize<List<Transactions>>(allTransactionDataText);

                allTransactions.Add(transactionsToBeAdded);

                File.WriteAllText(transactionsDataFilePath, JsonSerializer.Serialize(allTransactions));

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error adding transaction: {ex.Message}");
                return false;
            }
        }


        public static int CheckNoOfFreeDrinks(string phonenumberOrUsername)
        {
            List<NoOfTransactionHistory> noOfTransistionHistories = Universal.GetAllNoOfTransactionHistory();

            string memberName = GetCustomerDetailByUsernameOrPhoneNumber(phonenumberOrUsername).Name;

            NoOfTransactionHistory noOfTransactionHistory=  noOfTransistionHistories.FirstOrDefault(eachHistory => eachHistory.username == memberName);

            if(noOfTransactionHistory == null)
            {
                return 0;
            }

            int noOfComplimentaryDrinks = (int)(noOfTransactionHistory.noOfTransactions / 10) - noOfTransactionHistory.noOfFreeDrinksRedeemed;

            return (noOfTransactionHistory.noOfTransactions);

        }

        public static void ProceedMemberOrder(List<ProductOrderRequest> products, List<ProductOrderRequest> addIns, string memberNameOrPhoneNumber, int noOfComplementaryDrinks)
        {

            //by default is fault, might change when checking for regular customer
            bool isDiscountApplicable= false;

            int totalProductsOrdered= GetTotalProductQuantity(products);

            if (noOfComplementaryDrinks > totalProductsOrdered)
            {
                return;
            }
            Customer customer = GetCustomerDetailByUsernameOrPhoneNumber(memberNameOrPhoneNumber);

            if (customer == null)
            {
                return;
            }

            List<ProductOrderRequest> complementaryDrinks = new List<ProductOrderRequest>();

            //getting random drinks as complementary drinks
            HashSet<int> randomIndexes = GetUniqueRandomNumber(noOfComplementaryDrinks, products.Count);

            foreach(var eachRandomIndex in randomIndexes)
            {
                //getting hold of a random product ordered
                ProductOrderRequest randomOrderedProductDetails = products[eachRandomIndex];

                //saving random product with quantity 1 as complementary product
                ProductOrderRequest randomComplementaryOrderDetail = new()
                {
                    Name = randomOrderedProductDetails.Name,
                    Quantity = 1
                };

                complementaryDrinks.Add(randomComplementaryOrderDetail);


                //updating the ordered product by subtracting a qunatity
                if (randomOrderedProductDetails.Quantity > 1)
                {
                    products[eachRandomIndex] = new()
                    {
                        Name = randomOrderedProductDetails.Name,
                        Quantity = randomOrderedProductDetails.Quantity - 1
                    };
                }
                else if (randomOrderedProductDetails.Quantity < 2)
                {
                    products.RemoveAt(eachRandomIndex);
                }
            }

            //checking if user is a regular customer or not i.e. user data exists in the regular customer
            RegularCustomer regularCustomer = GetRegularCustomerDetail(customer.Name);

            //if not null means the customer is regular
            if (regularCustomer != null)
            {
                //checking if discount Start Date and today's date is prior a month
                isDiscountApplicable = CheckIfDiscountDateIsValid(customer.Name);
            }

            //saving the transaction
            ProceedOrderUniversal(products, addIns, customer.Name);

            //checking if the member is applicable for becoming regular
            bool isEligibleForRegular = CheckApplicabilityForRegular(customer.Name);

            if (isEligibleForRegular)
            {
                MakeCustomerRegular(customer.Name);
            }
            
        }

        static RegularCustomer GetRegularCustomerDetail(string username)
        {

            List<RegularCustomer> regularCustomers = Universal.GetRegularCustomersDetails();

            RegularCustomer regularCustomer= regularCustomers.FirstOrDefault(customer=> customer.Name == username);

            return regularCustomer;
        }

        static bool CheckIfDiscountDateIsValid(string customerName)
        {
            RegularCustomer regularCustomer = GetRegularCustomerDetail(customerName);

            DateTime discountStartDate = regularCustomer.DiscountStartDate;


            //if discount date exceeds 30 days or a month
            if ((discountStartDate - DateTime.Now).TotalDays > 30)
            {

                //deleting the user details from the regular customer details if the date is expired
                DeleteCustomerFromRegularCustomers(customerName);
                return false;
            }

            return true;
        }

        static void DeleteCustomerFromRegularCustomers(string username)
        {
            List<RegularCustomer> regularCustomers = Universal.GetRegularCustomersDetails();

            if (regularCustomers.Remove(regularCustomers.FirstOrDefault(customer => customer.Name == username)))
            {
                File.WriteAllText(FilePaths.RegularCustomersDetails(), JsonSerializer.Serialize(regularCustomers));
            }
        }

        //method to check if the member is applicable to become regular
        static bool CheckApplicabilityForRegular(string customerName)
        {
            List<Transactions> allTransactions = Universal.GetTransactionHistory();

            bool hasEveryDayPurchase = false;

            //getting transaction history of the customer;
            List<Transactions> userTransactions = allTransactions.Where(transaction => transaction.CustomerName == customerName).ToList();

            //checking if user has everyday purchase for 30 days before today

            HashSet<DateTime> datesOfMonthsBeforeTodayExcludingWeekends = new HashSet<DateTime>();
            DateTime todaysDate= DateTime.Now;

            

            for (int i=1; i<31; i++)
            { 
               DateTime eachDate=  todaysDate.AddDays(-i);

                if(eachDate.DayOfWeek!=DayOfWeek.Saturday && eachDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    datesOfMonthsBeforeTodayExcludingWeekends.Add(eachDate);
                }
            }


            //getting user transaction dates
            HashSet<DateTime> transactionDatesOfCustomer= new HashSet<DateTime>();
            foreach (Transactions transaction in userTransactions) {

                transactionDatesOfCustomer.Add(transaction.TransactionDate);
            }

            //looping through the datesOfMonthsBeforeTodayExcludingWeekends
            foreach (var eachDateInMonth in datesOfMonthsBeforeTodayExcludingWeekends)
            {
                //if one date is missing in the user transaction date, he becomes ineligible to be the regular customer
                if (!transactionDatesOfCustomer.Contains(eachDateInMonth))
                {
                    return false;
                }
            }

            //if the user is applicable to be the regular customer
            return true;
        }

        static void MakeCustomerRegular(string username)
        {
            List<RegularCustomer> regularCustomers = Universal.GetRegularCustomersDetails();
            regularCustomers.Add(
                   new RegularCustomer
                   { 
                       Name = username,
                       DiscountStartDate= DateTime.Now
                   }
                   );

            //overiding the file with newly updated list
            File.WriteAllText(FilePaths.RegularCustomersDetails(), JsonSerializer.Serialize(regularCustomers));
        }


        //utility methods
        static List<EachOrderProductDetail> ChangeToEachProductDetailListForProduct(List<ProductOrderRequest> productDetails)
        { 
            List<EachOrderProductDetail> responseList= new List<EachOrderProductDetail>();

            foreach(var eachProduct in productDetails)
            {
                responseList.Add(new()
                {
                    ProductName= eachProduct.Name,
                    ProductQuantity= eachProduct.Quantity,
                    NetTotal= GetProductPrice(eachProduct.Name)*eachProduct.Quantity
                });
            }

            return responseList;
        }

        static List<EachOrderProductDetail> ChangeToEachProductDetailListForAddIns(List<ProductOrderRequest> addInDetails)
        {
            List<EachOrderProductDetail> responseList= new();

            foreach(var eachAddIn in addInDetails)
            {
                responseList.Add(new()
                {
                    ProductName= eachAddIn.Name,
                    ProductQuantity= eachAddIn.Quantity,
                    NetTotal= GetAddInPrice(eachAddIn.Name)* eachAddIn.Quantity
                });
            }

            return responseList;
        }

        static float GetProductPrice(string name)
        {
            System.Diagnostics.Debug.WriteLine(name);
            List<Product> products = Universal.GetAllProducts();
            System.Diagnostics.Debug.WriteLine(products.Count);
            Product filteredAddInByName = products.FirstOrDefault(product => product.Name == name);
            return filteredAddInByName.Price;
        }


        static float GetAddInPrice(string name)
        {
            List<AddIns> addIns = Universal.GetallAddins();
            AddIns filteredAddInByName = addIns.FirstOrDefault(product => product.Name == name);
            return filteredAddInByName.Price;
        }



        static float GetTotalTransactionAmount(List<ProductOrderRequest> products, List<ProductOrderRequest> addIns)
        {
            float totalTransaction=0;

            foreach (var product in products)
            {
                totalTransaction += (GetProductPrice(product.Name) * product.Quantity);
            }

            if (addIns != null)
            {
                foreach (var addIn in addIns)
                {
                    totalTransaction += (GetAddInPrice(addIn.Name) * addIn.Quantity);
                }
            }

            return totalTransaction;
        }


        static HashSet<int> GetUniqueRandomNumber (int noOfRandomNumbers, int numberRange)
        {

            Random random = new Random();
            HashSet<int> generatedNumbers = new HashSet<int>();

            //getting random inde
            for (int i = 0; i < noOfRandomNumbers; i++)
            {
                int randomNumber;
                do
                {
                    randomNumber = random.Next(numberRange); // Generate a random number between 0 and 99
                } while (!generatedNumbers.Add(randomNumber)); // Keep generating until a new number is generated
            }
            return generatedNumbers;
        }

        static int GetTotalProductQuantity(List<ProductOrderRequest> products)
        {
            int totalProductQuantity = 0;

            foreach(var product in products)
            {
                totalProductQuantity += product.Quantity;
            };

            return totalProductQuantity;
        } 
    }


}
