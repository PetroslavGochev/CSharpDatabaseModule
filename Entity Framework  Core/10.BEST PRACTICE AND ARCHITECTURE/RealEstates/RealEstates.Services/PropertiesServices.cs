using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RealEstates.Services
{
    public class PropertiesServices : IPropertiesServices
    {
        private RealEstateContext db;

        public PropertiesServices(RealEstateContext context)
        {
            this.db = context;
        }
        public void Create(string district, int size, string buildingType, string propertyType, decimal price, int? year, byte? floor, byte? totalFloor)
        {
            if(district == null)
            {
                throw new ArgumentNullException(nameof(district));
            }
            RealEstateProperty property = new RealEstateProperty()
            {
                Size = size,
                Price = price,
                Year = year > 1800 ? year : null,
                Floor = floor > 0 ? floor : null,
                TotalFloors = totalFloor != 0 ? totalFloor : null,
            };
            property.District = ReturnDistrict(district);
            property.BuildingType = ReturnBuilding(buildingType);
            property.PropertyType = ReturnPropertyType(propertyType);

            db.RealEstateProperties.Add(property);
            db.SaveChanges();

            UpdateTags(property.Id);
        }

        private PropertyType ReturnPropertyType(string typeOfProperty)
        {
            var propertyType = db.PropertyTypes
               .Where(d => d.Name == typeOfProperty)
               .FirstOrDefault();
            if (propertyType == null)
            {
                propertyType = new PropertyType()
                {
                    Name = typeOfProperty,
                };
                db.PropertyTypes.Add(propertyType);
                db.SaveChanges();
            }
            return db.PropertyTypes.Where(d => d.Name == typeOfProperty).FirstOrDefault();
        }
        private District ReturnDistrict(string districtsName)
        {
            var district = db.Districts
                .Where(d => d.Name == districtsName)
                .FirstOrDefault();
            if(district == null)
            {
                district = new District()
                {
                    Name = districtsName,
                };
                db.Districts.Add(district);
                db.SaveChanges();
            }
            return db.Districts.Where(d => d.Name == districtsName).FirstOrDefault();
        }

        private BuildingType ReturnBuilding(string buildingTypeName)
        {
            var buildingType = db.BuildingTypes
               .Where(bt=> bt.Name == buildingTypeName)
               .FirstOrDefault();
            if (buildingType == null)
            {
                buildingType = new BuildingType()
                {
                    Name = buildingTypeName,
                };
                db.BuildingTypes.Add(buildingType);
                db.SaveChanges();
            }
            return db.BuildingTypes.Where(d => d.Name == buildingTypeName).FirstOrDefault();
        }
        

        public IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int maxSize, int minSize)
        {
            var properties = db.RealEstateProperties
               .Where(rep => rep.Year >= minYear && rep.Year <= maxYear && rep.Size >= minSize && rep.Size <= maxSize)
               .Select(MapToPropertyViewModel())
               .OrderBy(x => x.Price)
               .ToArray();

            return properties;
        }

        public IEnumerable<PropertyViewModel> SearchByPrice(decimal minPrice, decimal maxPrice)
        {
            var properties = db.RealEstateProperties
                .Where(rep => rep.Price >= minPrice && rep.Price <= maxPrice)
                .Select(MapToPropertyViewModel())
                .OrderBy(x => x.Price)
                .ToArray();

            return properties;
        }

        private static Expression<Func<RealEstateProperty, PropertyViewModel>> MapToPropertyViewModel()
        {
            return x => new PropertyViewModel
            {
                District = x.District.Name,
                Buildingtype = x.BuildingType.Name,
                PropertyType = x.PropertyType.Name,
                Size = x.Size,
                Floor = (x.Floor ?? 0) + "/" + (x.TotalFloors ?? 0),
                Price = x.Price,
                Year = x.Year ?? 0
            };
        }

        public void UpdateTags(int propertyId)
        {
            var property = db.RealEstateProperties
                .Where(rep => rep.Id == propertyId)
                .FirstOrDefault();
            if(property.Year.HasValue && property.Year < 1990)
            {
                var tags = new RealEstatePropertyTag()
                {
                    PropertyTag = CreateTag("OldBuildings")
                };
                property.PropertyTags.Add(tags);
            }

            if (property.Size >= 80)
            {
                var tags = new RealEstatePropertyTag()
                {
                    PropertyTag = CreateTag("HugeApartment")
                };
                property.PropertyTags.Add(tags);
            }
            else if (property.Size >= 50 && property.Size < 80)
            {
                var tags = new RealEstatePropertyTag()
                {
                    PropertyTag = CreateTag("MiddleApartment")
                };
                property.PropertyTags.Add(tags);
            }
            else
            {
                var tags = new RealEstatePropertyTag()
                {
                    PropertyTag = CreateTag("SmallApartmnet")
                };
                property.PropertyTags.Add(tags);
            }

            if (property.Floor.HasValue && property.Floor > 4 && property.Floor != property.TotalFloors)
            {
                var tags = new RealEstatePropertyTag()
                {
                    PropertyTag = CreateTag("HighFloor")
                };
                property.PropertyTags.Add(tags);
            }
            else if(property.Floor == property.TotalFloors)
            {
                var tags = new RealEstatePropertyTag()
                {
                    PropertyTag = CreateTag("LastFloor")
                };
                property.PropertyTags.Add(tags);
            }
            else
            {
                var tags = new RealEstatePropertyTag()
                {
                    PropertyTag = CreateTag("LowFloor")
                };
                property.PropertyTags.Add(tags);
            }

        }

        private Tag CreateTag(string tag)
        {
            var tags = db.Tags
                .Where(t => t.Name == tag)
                .FirstOrDefault();
            if(tags == null)
            {
                tags = new Tag() { Name = tag };
                db.Tags.Add(tags);
                db.SaveChanges();
            }
            return db.Tags.Where(t => t.Name == tag).FirstOrDefault();
        }
    }
}
