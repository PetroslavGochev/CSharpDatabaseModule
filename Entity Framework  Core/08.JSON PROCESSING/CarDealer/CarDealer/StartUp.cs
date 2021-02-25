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
    using Data;
    public class StartUp
    {
        private static string RESULT = "Successfully imported {0}.";
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            string inputJson = File.ReadAllText("../../../Datasets/parts.json");
            Console.WriteLine(ImportParts(db, inputJson));

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
    }
}