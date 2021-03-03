using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RealEstates.Importer
{
    class Program
    {
        private static RealEstateContext db;
        static void Main(string[] args)
        {
            var json = File.ReadAllText("../../../imot.bg-raw-data-2020-07-23.json");
            var properties = JsonSerializer.Deserialize<IEnumerable<JsonPropertyModel>>(json);

            db = new RealEstateContext();
            
            IPropertiesServices propertiesServices = new PropertiesServices(db);
            foreach (var p in properties.Where(p=>p.Price > 40000 && p.Price < 95000))
            {
                try
                {
                    propertiesServices.Create
                       (
                       p.District,
                       p.Size,
                       p.BuildingType,
                       p.Type,
                       p.Price,
                       p.Year,
                       p.Floor,
                       p.TotalFloors
                       );

                }
                catch (ArgumentNullException)
                {

                }
                
            }

        }
    }
}
