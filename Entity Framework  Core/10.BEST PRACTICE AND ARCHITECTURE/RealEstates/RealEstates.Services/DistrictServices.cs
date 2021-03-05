using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RealEstates.Services
{
    public class DistrictServices : IDistrictServices
    {
        private RealEstateContext db;

        public DistrictServices(RealEstateContext context)
        {
            this.db = context;
        }
        public IEnumerable<DistrictViewModel> GetDistrictsByAveragePrice(int count = 10)
        {
            var districts = db.Districts
                .Select(MapToDistrictViewModel())
                .OrderByDescending(d => d.AveragePrice)
                .Take(count)
                .ToArray();

            return districts;
        }
        public IEnumerable<DistrictViewModel> GetDistrictsByNumberOfProperties(int count = 10)
        {
            var districts = db.Districts
                .OrderByDescending(d=>d.Properties.Count)
                .Take(count)
                .Select(MapToDistrictViewModel())
                .ToArray();

            return districts;
        }
        private static Expression<Func<District, DistrictViewModel>> MapToDistrictViewModel()
        {
            return d => new DistrictViewModel()
            {
                Name = d.Name,
                MaxPrice = d.Properties.Max(p => p.Price),
                MinPrice = d.Properties.Min(p => p.Price),
                AveragePrice = d.Properties.Average(p => p.Price),
                PropertiesCounts = d.Properties.Count
            };
        }
    }
}
