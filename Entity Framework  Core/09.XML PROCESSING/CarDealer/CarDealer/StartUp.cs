using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.DTO.Export;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            string result = GetCarsWithDistance(db);
            IsDirectoryExist();
            File.WriteAllText(RESULT_DIRECTORY_PATH + "cars.xml", result);
            Console.WriteLine(result);
        }
        //public static string ImportSales(CarDealerContext context, string inputXml)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSales[]), new XmlRootAttribute("Sales"));

        //    ImportSales[] salesDto = (ImportSales[])xmlSerializer.Deserialize(new StringReader(inputXml));

        //    List<Sale> sales = new List<Sale>();

        //    salesDto = salesDto
        //        .Where(s => context.Cars.Any(c => c.Id == s.CarId))
        //        .ToArray();

        //    foreach (var sd in salesDto)
        //    {
        //        Sale sale = new Sale()
        //        {
        //            CarId = sd.CarId,
        //            CustomerId = sd.CustomerId,
        //            Discount = sd.Discount
        //        };
        //        sales.Add(sale);
        //    }

        //    context.Sales.AddRange(sales);

        //    context.SaveChanges();

        //    return string.Format(RESULT, sales.Count);

        //}
        //public static string ImportCustomers(CarDealerContext context, string inputXml)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomers[]), new XmlRootAttribute("Customers"));

        //    ImportCustomers[] customersDto = (ImportCustomers[])xmlSerializer.Deserialize(new StringReader(inputXml));

        //    List<Customer> customers = new List<Customer>();
        //    foreach (var c in customersDto)
        //    {
        //        Customer customer = new Customer()
        //        {
        //            Name = c.Name,
        //            IsYoungDriver = c.IsYoungDriver,
        //            BirthDate = c.BirthDate
        //        };
        //        customers.Add(customer);
        //    }

        //    context.Customers.AddRange(customers);

        //    context.SaveChanges();

        //    return string.Format(RESULT, customers.Count);
        //}
        //public static string ImportCars(CarDealerContext context, string inputXml)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCars[]), new XmlRootAttribute("Cars"));

        //    ImportCars[] carsDtos = (ImportCars[])xmlSerializer.Deserialize(new StringReader(inputXml));

        //    var cars = new List<Car>();
        //    var partCars = new List<PartCar>();

        //    foreach (var carDto in carsDtos)
        //    {
        //        var car = new Car()
        //        {
        //            Make = carDto.Make,
        //            Model = carDto.Model,
        //            TravelledDistance = carDto.TraveledDistance
        //        };

        //        var parts = carDto
        //            .Parts
        //            .Where(pc => context.Parts.Any(p => p.Id == pc.PartId))
        //            .Select(p => p.PartId)
        //            .Distinct();

        //        foreach (var part in parts)
        //        {
        //            PartCar partCar = new PartCar()
        //            {
        //                PartId = part,
        //                Car = car
        //            };

        //            partCars.Add(partCar);
        //        }

        //        cars.Add(car);

        //    }

        //    context.PartCars.AddRange(partCars);

        //    context.Cars.AddRange(cars);

        //    context.SaveChanges();
        //    return string.Format(RESULT,cars.Count);
        //}
        //public static string ImportParts(CarDealerContext context, string inputXml)
        //{
        //    XmlSerializer xml = new XmlSerializer(typeof(ImportParts[]), new XmlRootAttribute("Parts"));

        //    var partsDto = (ImportParts[])xml.Deserialize(new StringReader(inputXml));


        //    List<Part> partsCollection = new List<Part>();
        //    foreach (var p in partsDto)
        //    {
        //        if(context.Suppliers.Any(s=>s.Id == p.SupplierId))
        //        {
        //            Part part = new Part()
        //            {
        //                Name = p.Name,
        //                SupplierId = p.SupplierId,
        //                Price = p.Price,
        //                Quantity = p.Quantity
        //            };
        //            partsCollection.Add(part);
        //            context.Parts.Add(part);
        //        }
        //    }
        //    context.SaveChanges();

        //    return string.Format(RESULT,partsCollection.Count);
        //}
        //public static string ImportSuppliers(CarDealerContext context, string inputXml)
        //{
        //    XmlSerializer xml = new XmlSerializer(typeof(ImportSuppliers[]), new XmlRootAttribute("Suppliers"));

        //    var suppliersDto = (ImportSuppliers[])xml.Deserialize(new StringReader(inputXml));

        //    var suppliers = new List<Supplier>();
        //    foreach (var supplier in suppliersDto)
        //    {
        //        Supplier supp = new Supplier()
        //        {
        //            Name = supplier.Name,
        //            IsImporter = supplier.IsImporter
        //        };
        //        suppliers.Add(supp);
        //    }
        //    context.Suppliers.AddRange(suppliers);

        //    context.SaveChanges();
        //    return string.Format(RESULT,suppliers.Count);
        //}

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var cars = context.Cars
                .Where(c => c.TravelledDistance >= 20000000)
                .Select(c => new ExportCarWithDistanceDto()
                {
                    Make= c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .ToArray();

            XmlSerializer xml = new XmlSerializer(typeof(ExportCarWithDistanceDto[]), new XmlRootAttribute("Cars"));

            xml.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }



    }
}