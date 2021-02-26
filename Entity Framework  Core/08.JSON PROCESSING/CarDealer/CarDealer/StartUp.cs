using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    using CarDealer.DTO;
    using Data;
    public class StartUp
    {
        private const string RESULT = "Successfully imported {0}.";
        private const string DIRECTORY_RESULT = "../../../Datasets/Result/";

        private static void IfResultExist()
        {
            if (!Directory.Exists(DIRECTORY_RESULT))
            {
                Directory.CreateDirectory(DIRECTORY_RESULT);
            }
        }
        public static void Main(string[] args)
        {
            //string inputJson = File.ReadAllText("../../../Datasets/sales.json");
            CarDealerContext db = new CarDealerContext();
            
            string result = (GetSalesWithAppliedDiscount(db));
            IfResultExist();
            File.WriteAllText($"{DIRECTORY_RESULT}/sales-discounts.json", result);

        }
        private static void ResetDatabase(CarDealerContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfullt deleted!");
            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfullt created!");
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.AddRange(suppliers);

            context.SaveChanges();

            return string.Format(RESULT, suppliers.Length);
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            List<Part> parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);
            var suppliers = context.Suppliers.Select(s => s.Id);
            parts = parts.Where(p => suppliers.Any(s => s == p.SupplierId)).ToList();
            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsInput = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson);

            List<Car> listOfcars = new List<Car>();
            foreach (var carJson in carsInput)
            {
                Car car = new Car()
                {
                    Make = carJson.Make,
                    Model = carJson.Model,
                    TravelledDistance = carJson.TravelledDistance
                };
                foreach (var partId in carJson.PartsId.Distinct())
                {
                    car.PartCars.Add(new PartCar()
                    {
                        Car = car,
                        PartId = partId                      
                    });
                }
                listOfcars.Add(car);
            }

            context.Cars.AddRange(listOfcars);
            context.SaveChanges();
            return string.Format(RESULT,listOfcars.Count);

        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return String.Format(RESULT, customers.Length);
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return string.Format(RESULT, sales.Length);
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                    IsYoungDriver = x.IsYoungDriver
                })
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .ToList();
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "dd/MM/yyyy";
            jsonSettings.Formatting = Formatting.Indented;
            string customersJson = JsonConvert.SerializeObject(customers,jsonSettings);

            return customersJson;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x=>x.Make == "Toyota")
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            string carsJson = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return carsJson;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            string result = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsParts = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    parts = c.PartCars.Select(ps => new
                    {
                        Name = ps.Part.Name,
                        Price = $"{ps.Part.Price:F2}"
                    }).ToArray()


                })
                .ToArray();

            var result = JsonConvert.SerializeObject(carsParts, Formatting.Indented);

            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            string result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = s.Discount.ToString("f2"),
                    price = s.Car.PartCars.Sum(x => x.Part.Price).ToString("f2"),
                    priceWithDiscount = (((100 - s.Discount) / 100) * s.Car.PartCars.Sum(x => x.Part.Price)).ToString("f2")
                })
                .Take(10)
                .ToArray();

            string result = JsonConvert.SerializeObject(sales,Formatting.Indented);

            return result;

        }
    }
}