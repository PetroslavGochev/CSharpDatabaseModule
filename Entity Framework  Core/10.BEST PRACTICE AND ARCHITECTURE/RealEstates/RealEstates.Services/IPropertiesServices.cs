using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services
{
    public interface IPropertiesServices
    {
        void Create(string district, int size, string buildingType, string propertyType, decimal price, int? year, byte? floor, byte? totalFloor);

        void UpdateTags(int propertyId);

        IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int maxSize, int minSize);

        IEnumerable<PropertyViewModel> SearchByPrice(decimal minPrice, decimal maxPrice);
    }
}
