using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace RealEstates.Services
{
    public class TagServices : ITagServices
    {
        private readonly RealEstateContext db;

        public TagServices(RealEstateContext context)
        {
            this.db = context;
        }
        public IEnumerable<TagPropertyViewModel> GetPropertyByTags(string tags)
        {
            var tagsProp = db.RealEstatePropertyTags
                .Where(rept => rept.PropertyTag.Name == tags)
                .Select(rept => new TagPropertyViewModel()
                {
                    District = rept.Property.District.Name,
                    BuildingType = rept.Property.BuildingType.Name,
                    PropertyType = rept.Property.PropertyType.Name,
                    Price = rept.Property.Price,
                    Year = rept.Property.Year,
                    Size = rept.Property.Size,
                    Floor = (rept.Property.Floor ?? 0) + "/" + (rept.Property.TotalFloors ?? 0),
                    Tag = rept.PropertyTag.Name
                })
                .OrderBy(rept => rept.Price)
                .ToArray();

            return tagsProp;
        }
    }
}
