using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private const string RESULT = "Successfully imported {0}";
        //private static string XML_INPUT = "../../../Datasets/";
        private static string RESULT_DIRECTORY_PATH = "../../../Datasets/Result/";

        private static void IsDirectoryExist()
        {
            if (!Directory.Exists(RESULT_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(RESULT_DIRECTORY_PATH);
            }
        }
        private static void ResetDataBase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted!");
            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
        }
        public static void Main(string[] args)
        {
            
            ProductShopContext db = new ProductShopContext();
            IsDirectoryExist();
            InitializeMapper();
            string result = GetUsersWithProducts(db);
            File.WriteAllText( RESULT_DIRECTORY_PATH + "users-and-products.xml", result);
            Console.WriteLine(result);
        }
        //public static string GetUsersWithProducts(ProductShopContext context)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    var namespaces = new XmlSerializerNamespaces();
        //    namespaces.Add(string.Empty, string.Empty);

        //    var usersWithProducts = context.Users
        //        .Where(u => u.ProductsSold.Count(ps => ps.Buyer != null) >= 1)
        //        .Select(u => new ExportUsers()
        //        {
        //            FirstName = u.FirstName,
        //            LastName = u.LastName,
        //            Age = u.Age,
        //            SoldProducts = new ExportListOfProducts()
        //            {
        //               Count = u.ProductsSold.Count,
        //               Products = u.ProductsSold
        //               .Select(x=> new 
        //               {
        //                   name
        //               })

        //            }

        //        })
        //        .ToArray();



        //    XmlSerializer xml = new XmlSerializer(typeof(ExportCategoriesProductsCount[]), new XmlRootAttribute("Users"));

        //    xml.Serialize(new StringWriter(sb), categories, namespaces);

        //    return sb.ToString().TrimEnd();
        //}

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var users = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderByDescending(u => u.ProductsSold.Count)
                .Take(1)
                .Select(u => new ExportUsers()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportListOfProducts()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                        .Where(ps => ps.Buyer != null)
                        .Select(ps => new ExportProductNameAndPriceDto()
                        {
                            Name = ps.Name,
                            Price = ps.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }
                })
                .ToArray();

            var usersAndProducts = new UsersAndProductsDto()
            {
                Count = users.Length,
                Users = users
            };

            XmlSerializer xml = new XmlSerializer(typeof(UsersAndProductsDto), new XmlRootAttribute("Users"));

            xml.Serialize(new StringWriter(sb), usersAndProducts, namespaces);

            return sb.ToString().TrimEnd();

        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var categories = context.Categories
                .Select(c => new ExportCategoriesProductsCount()
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray(); 

            XmlSerializer xml = new XmlSerializer(typeof(ExportCategoriesProductsCount[]), new XmlRootAttribute("Users"));

            xml.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var users = context.Users
                .Where(u => u.ProductsSold.Count() >= 1)
                .Select(u => new ExportSoldProducts()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Take(5)
                .ToArray();
              

            XmlSerializer xml = new XmlSerializer(typeof(ExportSoldProducts[]), new XmlRootAttribute("Users"));

            xml.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductsInRange()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();

            XmlSerializer xml = new XmlSerializer(typeof(ProductsInRange[]), new XmlRootAttribute("Products"));

            xml.Serialize(new StringWriter(sb),products,namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportUsersDto[]), new XmlRootAttribute("Users"));

            var usersDTO = (ImportUsersDto[])xml.Deserialize(new StringReader(inputXml));

            var users = Mapper.Map<User[]>(usersDTO);

            context.Users.AddRange(users);

            context.SaveChanges();

            return string.Format(RESULT,usersDTO.Length);
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportProductsDto[]), new XmlRootAttribute("Products"));

            var productsDTO = (ImportProductsDto[])xml.Deserialize(new StringReader(inputXml));

            List<Product> listOfProducts = new List<Product>();

            foreach (var productDto in productsDTO)
            {
               
                Product product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId
                };
                if (productDto.BuyerId != 0)
                {
                    product.BuyerId = productDto.BuyerId;
                }
                listOfProducts.Add(product);
            }

            context.Products.AddRange(listOfProducts);

            context.SaveChanges();


            return string.Format(RESULT, listOfProducts.Count);
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportCategoriesDto[]), new XmlRootAttribute("Categories"));

            var categoriesDto = (ImportCategoriesDto[])xml.Deserialize(new StringReader(inputXml));

            var categories = Mapper.Map<Category[]>(categoriesDto);
            categories = categories.Where(x => x.Name != null).ToArray();

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return string.Format(RESULT, categories.Length);
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportCategoryProductsDto[]), new XmlRootAttribute("CategoryProducts"));

            var categoryProducts = (ImportCategoryProductsDto[])xml.Deserialize(new StringReader(inputXml));

            var categoryProductsDto = Mapper.Map<CategoryProduct[]>(categoryProducts);

            List<CategoryProduct> listOfCategoryProducts = new List<CategoryProduct>();

            foreach (var item in categoryProductsDto)
            {
                var category = context.Categories
                    .Where(c => c.Id == item.CategoryId)
                    .FirstOrDefault();
                var product = context.Products
                    .Where(p => p.Id == item.ProductId)
                    .FirstOrDefault();
                if(category != null && product != null)
                {
                    listOfCategoryProducts.Add(item);
                }
            }

            context.CategoryProducts.AddRange(listOfCategoryProducts);

            context.SaveChanges();

            return string.Format(RESULT, listOfCategoryProducts.Count);
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<ProductShopProfile>(); });
        }

    }
}
