using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static string RESULT_DIRECTORY_PATH = "../../../Datasets/Results";
        public static void Main(string[] args)
        {
            
            ProductShopContext db = new ProductShopContext();
            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            string result = GetUsersWithProducts(db);
            if (!Directory.Exists(RESULT_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(RESULT_DIRECTORY_PATH);
            }
            File.WriteAllText($"{RESULT_DIRECTORY_PATH}/users-and-products.json",result);
        }
        private static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfullt deleted!");
            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfullt created!");
        }

        ////Problem 01 ImportUsers
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        ////Problem 02  Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        ////Problem 3 Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert
                .DeserializeObject<Category[]>(inputJson)
                .Where(x => x.Name != null)
                .ToArray();


            context.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        //problem 04 Import CategoriesProduct
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.AddRange(categoriesProducts);
            context.SaveChanges();


            return $"Successfully imported {categoriesProducts.Length}";
        }

        //problem 05 ExportProduct in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .Where(x => x.price >= 500 && x.price <= 1000)
                .OrderBy(x => x.price)
                .ToList();

            string product = JsonConvert.SerializeObject(products, Formatting.Indented);

            return product;

        }

        problem 06.export successfully sold
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                    .Select(ps => new
                    {
                        name = ps.Name,
                        price = ps.Price,
                        buyerFirstName = ps.Buyer.FirstName,
                        buyerLastName = ps.Buyer.LastName
                    })
                    .ToArray()
                })
                .OrderBy(u => u.lastName)
                .ThenBy(u => u.firstName)
                .ToArray();


            string usersJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            return usersJson;

        }

        problem 07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var category = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count(),
                    averagePrice = $"{c.CategoryProducts.Average(x => x.Product.Price):f2}",
                    totalRevenue = $"{c.CategoryProducts.Sum(x => x.Product.Price):f2}"
                })
                .OrderByDescending(c => c.productsCount)
                .ToArray();
            var categoryJson = JsonConvert.SerializeObject(category, Formatting.Indented);

            return categoryJson;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .Select(u => new
                {
                    lastName = u.LastName,
                    age = u.Age,
                    soldProdcuts = new
                    {
                        count = u.ProductsSold.Count,
                        products = u.ProductsSold
                        .Select(ps => new
                        {
                            name = ps.Name,
                            price = ps.Price
                        })
                        .ToArray()
                    }

                })
                .OrderByDescending(u => u.soldProdcuts.count)
                .ToArray();


            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var userResult = new
            {
                usersCount = users.Length,
                users = users
            };

            var usersJson = JsonConvert.SerializeObject(userResult, settings);
            return usersJson;
        }

    }
}