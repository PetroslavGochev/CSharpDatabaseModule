using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private const string RESULT = "Successfully imported {0}";
        private static string XML_INPUT = "../../../Datasets/";
        private static string RESULT_DIRECTORY_PATH = "../../../Datasets/Result/";

        private static void IsDirectoryExist()
        {
            if (!Directory.Exists(RESULT_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(RESULT_DIRECTORY_PATH);
            }
        }
        private static void ResetDataBase(CarDealerContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted!");
            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
        }
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            //ResetDataBase(db);
            InitializeMapper();
            string inputXml = File.ReadAllText(XML_INPUT + "parts.xml");
            Console.WriteLine(ImportParts(db, inputXml));
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportCars[]), new XmlRootAttribute("Parts"));

        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportParts[]), new XmlRootAttribute("Parts"));

            var partsDto = (ImportParts[])xml.Deserialize(new StringReader(inputXml));

            var parts = Mapper.Map<Part[]>(partsDto);

            List<Part> partsCollection = new List<Part>();
            foreach (var part in parts)
            {
                if(context.Suppliers.Any(s=>s.Id == part.SupplierId))
                {
                    partsCollection.Add(part);
                    context.Parts.Add(part);
                }
            }
            context.SaveChanges();

            return string.Format(RESULT,partsCollection.Count);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ImportSuppliers[]), new XmlRootAttribute("Suppliers"));

            var suppliersDto = (ImportSuppliers[])xml.Deserialize(new StringReader(inputXml));

            var suppliers = Mapper.Map<Supplier[]>(suppliersDto);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();
            return string.Format(RESULT,suppliers.Length);
        }

        public static void InitializeMapper()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<CarDealerProfile>(); });
        }
    }
}