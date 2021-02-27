using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private const string RESULT = "Successfully imported {0}";

        
        public static void Main(string[] args)
        {
            
            ProductShopContext db = new ProductShopContext();
            InitializeMapper();
            var xmlInput = File.ReadAllText("../../../Datasets/users.xml");
            string result = ImportUsers(db, xmlInput);
            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
           // XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportUserDTO[]), new XmlRootAttribute("Users"));
            XmlSerializer xml = new XmlSerializer(typeof(ImportUsersDto[]), new XmlRootAttribute("Users"));

            var usersDTO = (ImportUsersDto[])xml.Deserialize(new StringReader(inputXml));

            var users = Mapper.Map<User[]>(usersDTO);

            context.Users.AddRange(users);

            context.SaveChanges();

            return string.Format(RESULT,usersDTO.Length);
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<ProductShopProfile>(); });
        }

    }
}
