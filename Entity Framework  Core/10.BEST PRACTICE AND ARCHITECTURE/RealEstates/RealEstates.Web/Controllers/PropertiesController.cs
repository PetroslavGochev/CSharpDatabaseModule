﻿using Microsoft.AspNetCore.Mvc;
using RealEstates.Services;

namespace RealEstates.Web.Controllers
{
    public class PropertiesController : Controller    
    {
        private readonly IPropertiesServices propertyServices;

        public PropertiesController(IPropertiesServices propertyServices)
        {
            this.propertyServices = propertyServices;
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
    }

}
