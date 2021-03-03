using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Text;

namespace RealEstates.ConsoleApplication
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var db = new RealEstateContext();
            db.Database.Migrate();

            //IPropertiesServices propertiesServices = new PropertiesServices(db);
            //Console.Write("Min Price:");
            //int minPrice = int.Parse(Console.ReadLine());
            //Console.Write("Max Price:");
            //int maxPrice = int.Parse(Console.ReadLine());
            //var properties = propertiesServices.SearchByPrice(minPrice, maxPrice);

            //foreach (var p in properties)
            //{
            //    Console.WriteLine($" -- {p.District} => {p.Price} - {p.Size} => {p.Year} => {p.Buildingtype} => {p.Floor}");

            //}



            IDistrictServices districtServices = new DistrictServices(db);
            var properties = districtServices.GetDistrictsByAveragePrice();
            //Console.WriteLine("District By Average Price:");
            //foreach (var p in properties)
            //{
            //    Console.WriteLine($" -- {p.Name} => {p.MaxPrice} - {p.MinPrice} => {p.AveragePrice} => {p.PropertiesCounts}");
            //}

            Console.WriteLine("District By Nubmer Of properties:");
            properties = districtServices.GetDistrictsByNumberOfProperties();
            foreach (var p in properties)
            {
                Console.WriteLine($" -- {p.Name} => {p.MaxPrice} - {p.MinPrice} => {p.AveragePrice} => {p.PropertiesCounts}");
            }

        }
    }
}
