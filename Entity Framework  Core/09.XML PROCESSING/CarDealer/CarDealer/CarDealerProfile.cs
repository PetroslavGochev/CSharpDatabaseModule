using AutoMapper;
using CarDealer.DTO;
using CarDealer.DTO.Export;
using CarDealer.Models;
using System.Linq;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSuppliers, Supplier>();

            this.CreateMap<ImportParts, Part>();

            this.CreateMap<Supplier, ExportLocalSuppliers>()
                .ForMember(x => x.PartsCount, y => y.MapFrom(s => s.Parts.Count))
                .ReverseMap();

            this.CreateMap<PartCar, ExportPartCarsDTO>()
                .ForMember(pc => pc.Name, p => p.MapFrom(pc => pc.Part.Name))
                .ForMember(pc => pc.Price, c => c.MapFrom(pc => pc.Part.Price));

            this.CreateMap<Car, ExportCarWithParts>()
                .ForMember(x => x.Parts, y => y.MapFrom(s => s.PartCars.OrderByDescending(pc => pc.Part.Price)));
        }
    }
}
