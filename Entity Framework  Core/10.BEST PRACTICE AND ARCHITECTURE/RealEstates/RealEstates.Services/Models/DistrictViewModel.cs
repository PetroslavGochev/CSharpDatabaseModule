using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services.Models
{
    public class DistrictViewModel
    {
        public string Name { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal AveragePrice { get; set; }

        public int PropertiesCounts  { get; set; }
    }
}
