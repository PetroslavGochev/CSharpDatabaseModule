using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services
{
    public interface IDistrictServices
    {
        IEnumerable<DistrictViewModel> GetDistrictsByAveragePrice(int count = 10);

        IEnumerable<DistrictViewModel> GetDistrictsByNumberOfProperties(int count = 10);

    }
}
