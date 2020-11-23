namespace Shared.Services
{
    using Model.DTO.Projects;

    using Shared;
    using Shared.Constants;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class ProjectsService : IProjectsService
    {
        #region " Constants "

        private static string _XML_FILE = "xml-data\\Projects.xml";

        /// <summary>
        /// Clave de cache en la que almacenamos el listado completo.
        /// </summary>
        private const String cacheKeyListItems = "Projects_GetList";

        #endregion

        public List<DtoProject> LoadProjectsFull()
        {
            throw new NotImplementedException();
        }

        public DtoProject LoadProject(string slug)
        {
            throw new NotImplementedException();
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
    }
}
