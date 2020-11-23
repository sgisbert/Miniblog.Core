namespace Shared.Services
{
    using Model.DTO.Projects;

    using Shared;
    using Shared.Constants;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public class ProjectsService : IProjectsService
    {
        private readonly ICategoriesService categories;
        private readonly IImagesService images;
        private readonly IClientsService clients;
        private readonly IBusinessService business;

        #region " Constants "

        private static string _XML_FILE = "xml-data\\Projects.xml";

        /// <summary>
        /// Clave de cache en la que almacenamos el listado completo.
        /// </summary>
        private const String cacheKeyListItems = "Projects_GetList";

        #endregion

        public ProjectsService(ICategoriesService categories, IImagesService images, IClientsService clients, IBusinessService business)
        {
            this.categories = categories;
            this.images = images;
            this.clients = clients;
            this.business = business;
        }

        public List<DtoProject> LoadProjectsFull()
        {
            List<DtoProject> projects;
            List<DtoProject> projectsLoaded = new List<DtoProject>();

            projects = LoadProjects();
            var categories = this.categories.LoadCategories();
            var clients = this.clients.LoadClients();
            var images = this.images.LoadImages();
            var business = this.business.LoadBusiness();

            foreach (var project in projects)
            {
                var tmpProject = LoadProjectData(project, categories, clients, images, business);
                projectsLoaded.Add(tmpProject);
            }
            return projectsLoaded ?? new List<DtoProject>();
        }

        public DtoProject LoadProject(string slug)
        {
            List<DtoProject> projects = LoadProjects();
            var categories = this.categories.LoadCategories();
            var clients = this.clients.LoadClients();
            var images = this.images.LoadImages();
            var business = this.business.LoadBusiness();
            DtoProject project = projects.First(p => p.Slug == slug);
            var tmpProject = LoadProjectData(project, categories, clients, images, business);

            return tmpProject;
        }

        public List<DtoProject> LoadProjects()
        {
            List<DtoProject> projects;
            const string cacheKey = cacheKeyListItems;
            projects = Cache.Get<List<DtoProject>>(cacheKey, makeCopy: false);

            if (projects == null || Debugger.IsAttached)
            {
                var rootPath = (string)AppDomain.CurrentDomain.GetData(AppDomainConstants.ContentRootPath);
                projects = Shared.Serialization.FromXML<List<DtoProject>>(Path.Combine(rootPath, _XML_FILE));
                Cache.Insert(cacheKey, projects, CacheConstants.DefaultExpirationTime, makeCopy: false);
            }
            return projects ?? new List<DtoProject>();
        }

        public DtoProject LoadProjectData(DtoProject project, List<DtoCategory> categories, List<DtoClient> clients, List<DtoImage> images, List<DtoBusiness> business)
        {
            var tmpProject = project.Copy();

            // Categories
            tmpProject.Categories = new List<DtoCategory>();
            foreach (var category in project.Categories)
            {
                tmpProject.Categories.Add(categories.FirstOrDefault(c => c.Id == category.Id));
            }

            // Images
            tmpProject.Images = new List<DtoImage>();
            foreach (var image in project.Images)
            {
                tmpProject.Images.Add(images.FirstOrDefault(c => c.Id == image.Id));
            }

            // Client
            if (project.Client != null)
                tmpProject.Client = clients.FirstOrDefault(c => c.Id == project.Client.Id);

            // Business
            if (project.Business != null)
                tmpProject.Business = business.FirstOrDefault(c => c.Id == project.Business.Id);

            return tmpProject;
        }
    }
}
