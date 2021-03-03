using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System;

namespace RealEstates.ConsoleApplication
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new RealEstateContext();
            db.Database.Migrate();

            IPropertiesServices propertiesServices = new PropertiesServices(db);

            propertiesServices.UpdateTags(2);
        }
    }
}
