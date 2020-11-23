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
        private readonly IProjectsService projects;

        public ProjectsController(ICategoriesService categories, IProjectsService projects)
        {
            this.categories = categories;
            this.projects = projects;
        }

        [OutputCache(Profile = "default")]
        public IActionResult Index()
        {
            ViewBag.Categories = categories.LoadCategories();
            ViewBag.Projects = projects.LoadProjects();
            return View();
        }
    }
}
