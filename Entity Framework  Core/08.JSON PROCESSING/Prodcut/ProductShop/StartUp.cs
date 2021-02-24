using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            string inputJson = File.ReadAllText("../../../Datasets/categories.json");
            Console.WriteLine(ImportCategories(db, inputJson));
        }
        private static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfullt deleted!");
            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfullt created!");
        }

        ////Problem 01 ImportUsers
        //public static string ImportUsers(ProductShopContext context, string inputJson)
        //{
        //    var users = JsonConvert.DeserializeObject<User[]>(inputJson);

        //    context.Users.AddRange(users);
        //    context.SaveChanges();

        //    return $"Successfully imported {users.Length}";
        //}

        ////Problem 02  Import Products
        //public static string ImportProducts(ProductShopContext context, string inputJson)
        //{
        //    var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

        //    context.Products.AddRange(products);
        //    context.SaveChanges();

        //    return $"Successfully imported {products.Length}";
        //}

        ////Problem 3 Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert
                .DeserializeObject<Category[]>(inputJson)
                .Where(x => x.Name != null)
                .ToArray() ;


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
    }
}