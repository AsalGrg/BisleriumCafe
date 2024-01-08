
using Coursework1Development.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Coursework1Development.Logics
{
    internal class AdminTasks
    {

        public static bool AddProduct(string name, string description, float price)
        {

            try
            {
                string allAddProductsFilePath = FilePaths.AllProductsData();
                string allAddProductsText = File.ReadAllText(allAddProductsFilePath);

                List<Product> allProducts = JsonSerializer.Deserialize<List<Product>>(allAddProductsText);

                allProducts.Add(
                    new Product
                    {
                        ID = Guid.NewGuid(),
                        Name = name,
                        Description = description,
                        Price = price
                    }
                    );
                //overiding the file with newly updated list
                File.WriteAllText(allAddProductsFilePath, JsonSerializer.Serialize(allProducts));

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error adding product: {ex.Message}");
                return false;
            }
        }

        public static bool ChangePassword(string username, string password, string newPassword)
        {
            string userDataFilePath = FilePaths.UserData();

            string allUserDataText = File.ReadAllText(userDataFilePath);

            UserList userList = JsonSerializer.Deserialize<UserList>(allUserDataText);

            User user = userList.users.FirstOrDefault(u => u.name == username && u.password == password);

            if (user == null)
            {
                return false;
            }

            user.password = newPassword;

            File.WriteAllText(userDataFilePath, JsonSerializer.Serialize(userList));
            return true;
        }


        public static bool DeleteProduct(Product selectedProduct)
        {
            List<Product> products = Universal.GetAllProducts();
            if (products.Remove(products.FirstOrDefault(product=> product.ID==selectedProduct.ID)))
            {
                File.WriteAllText(FilePaths.AllProductsData(), JsonSerializer.Serialize(products));
                return true;
            }


            return false;
        }

        public static bool EditProduct(Product updatedProduct)
        {
            List<Product> products = Universal.GetAllProducts();

            // Find the index of the product to be updated
            int index = products.FindIndex(product => product.ID == updatedProduct.ID);

            // If the product is found, update it
            if (index != -1)
            {
                products[index] = updatedProduct;
            }
            else
            {
                // Handle the case where the product is not found
                return false;
            }

            // Save the updated products list back to the file
            File.WriteAllText(FilePaths.AllProductsData(), JsonSerializer.Serialize(products));

            return true;
        }



        public static bool AddAddIns(string name, string description, float price)
        {

            try
            {
                string allAddInsFilePath = FilePaths.AllAddinsData();
                string allAddInsText = File.ReadAllText(allAddInsFilePath);

                List<AddIns> allAddIns = JsonSerializer.Deserialize<List<AddIns>>(allAddInsText);

                allAddIns.Add(
                    new AddIns
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                        Description = description,
                        Price = price
                    }
                    );
                //overiding the file with newly updated list
                File.WriteAllText(allAddInsFilePath, JsonSerializer.Serialize(allAddIns));

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error adding product: {ex.Message}");
                return false;
            }
        }

        public static bool DeleteAddIns(AddIns selectedProduct)
        {
            List<AddIns> addIns = Universal.GetallAddins();
            if (addIns.Remove(addIns.FirstOrDefault(product => product.Id == selectedProduct.Id)))
            {
                File.WriteAllText(FilePaths.AllAddinsData(), JsonSerializer.Serialize(addIns));
                return true;
            }


            return false;
        }

        public static bool EditAddIns(AddIns updatedAddIns)
        {
            List<AddIns> products = Universal.GetallAddins();

            // Find the index of the product to be updated
            int index = products.FindIndex(product => product.Id == updatedAddIns.Id);

            // If the product is found, update it
            if (index != -1)
            {
                products[index] = updatedAddIns;
            }
            else
            {
                // Handle the case where the product is not found
                return false;
            }

            // Save the updated products list back to the file
            File.WriteAllText(FilePaths.AllAddinsData(), JsonSerializer.Serialize(products));

            return true;
        }


    }
}
