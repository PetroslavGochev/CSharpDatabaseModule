using System.Collections.Generic;

namespace RealEstates.Models
{
    public class RealEstateProperty
    {
        public RealEstateProperty()
        {
            this.PropertyTags = new HashSet<RealEstatePropertyTag>();
        }
        public int Id { get; set; }
        public int Size { get; set; }

        public byte? Floor { get; set; }

        public byte? TotalFloors { get; set; }

        public int DistrictId { get; set; }
        public virtual District District { get; set; }

        public int?  Year { get; set; }

        public int PropertyTypeId { get; set; }
        public virtual PropertyType PropertyType { get; set; }

        public int BuildingTypeId { get; set; }
        public virtual BuildingType BuildingType { get; set; }

        public decimal Price { get; set; }

    
        public virtual ICollection<RealEstatePropertyTag> PropertyTags { get; set; }
    }
}
