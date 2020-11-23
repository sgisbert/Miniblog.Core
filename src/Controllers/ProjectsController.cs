
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
            ViewBag.Projects = projects.LoadProjectsFull();
            return View();
        }

        [OutputCache(Profile = "default")]
        [Route("/projects/{id}")]
        public IActionResult Project(string id)
        {
            // Ficha de proyecto
            var project = projects.LoadProject(id);
            return View(project);
        }
    }
}
