using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Shared.Services;

namespace Miniblog.Core.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ICategoriesService categories;

        public ProjectsController(ICategoriesService categories)
        {
            this.categories = categories;
        }

        [OutputCache(Profile = "default")]
        public IActionResult Index()
        {
            ViewBag.Categories = categories.LoadCategories();
            return View();
        }
    }
}
