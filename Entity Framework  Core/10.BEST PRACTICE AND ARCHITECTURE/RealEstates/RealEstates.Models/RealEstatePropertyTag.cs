namespace RealEstates.Models
{
    public class RealEstatePropertyTag
    {

        public int PropertyId { get; set; }
        public virtual RealEstateProperty Property { get; set; }

        public int PropertyTagId { get; set; }

        public virtual Tag PropertyTag { get; set; }
    }
}
