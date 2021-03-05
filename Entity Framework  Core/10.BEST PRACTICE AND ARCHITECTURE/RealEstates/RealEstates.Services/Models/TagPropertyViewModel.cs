using RealEstates.Models;
using System.Collections.Generic;

namespace RealEstates.Services.Models
{
    public class TagPropertyViewModel
    {
        public string District { get; set; }

        public string BuildingType { get; set; }

        public string PropertyType { get; set; }

        public decimal Price { get; set; }

        public int? Year { get; set; }

        public int Size { get; set; }

        public string Floor { get; set; }

        public string Tag { get; set; }
    }
}

