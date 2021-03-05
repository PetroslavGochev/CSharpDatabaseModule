using Microsoft.AspNetCore.Mvc;
using RealEstates.Data;
using RealEstates.Services;
using System.Linq;

namespace RealEstates.Web.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IPropertiesServices propertyServices;
        private readonly RealEstateContext db;

        public PropertiesController(IPropertiesServices propertyServices,RealEstateContext db)
        {
            this.propertyServices = propertyServices;
            this.db = db;
        }

        public IActionResult Search()
        {

            return this.View();
        }

        public IActionResult DoSearch(int minPrice, int maxPrice)
        {
            var properties = this.propertyServices.SearchByPrice(minPrice, maxPrice);
            return this.View(properties);
        }
        public IActionResult SearchBySize()
        {

            return this.View();
        }
        public IActionResult DoSearchBySize(int minYear, int maxYear, int minSize, int maxSize)
        {
            var properties = this.propertyServices.Search(minYear, maxYear, minSize, maxSize);
            return this.View(properties);
        }

        public IActionResult AddProperty()
        {
            return this.View();
        }
        public IActionResult CreateProperty(string district, int size, string buildingType, string propertyType, decimal price, int? year, byte? floor, byte? totalFloor)
        {
            this.propertyServices.Create(district, size, buildingType, propertyType, price, year, floor, totalFloor);
            var property = this.db.RealEstateProperties
                .OrderBy(x=>x.Id)
                .Last();
            return this.View(property);
        }

    }

}
