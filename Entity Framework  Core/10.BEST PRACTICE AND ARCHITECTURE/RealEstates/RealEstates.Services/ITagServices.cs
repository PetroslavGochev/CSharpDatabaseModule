using RealEstates.Models;
using RealEstates.Services.Models;
using System.Collections.Generic;

namespace RealEstates.Services
{
    public interface ITagServices
    {
        IEnumerable<TagPropertyViewModel> GetPropertyByTags(string tags);
    }
}
