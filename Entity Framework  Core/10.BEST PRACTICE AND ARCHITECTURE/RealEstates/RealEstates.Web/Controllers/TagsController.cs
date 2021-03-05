using Microsoft.AspNetCore.Mvc;
using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services;
using System.Linq;

namespace RealEstates.Web.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagServices tagServices;
        private readonly RealEstateContext db;


        public TagsController(ITagServices tagServices,RealEstateContext db)
        {
            this.tagServices = tagServices;
            this.db = db;
        }

        public IActionResult ApartmentByTag()
        {
            var tags = db.Tags.ToList();
            return this.View(tags);
        }
        public IActionResult DoSearchByTags(string tag)
        {
            var tagsProperties = this.tagServices.GetPropertyByTags(tag);
            return this.View(tagsProperties);
        }

    }
}
