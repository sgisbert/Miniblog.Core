namespace Shared.Services
{
    using Model.DTO.Projects;

    using Shared;
    using Shared.Constants;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class CategoriesService : ICategoriesService
    {
        #region " Constants "

        private static string _XML_FILE = "xml-data\\Categories.xml";

        /// <summary>
        /// Clave de cache en la que almacenamos el listado completo.
        /// </summary>
        private const String cacheKeyListItems = "Categories_GetList";

        #endregion

        public List<DtoCategory> LoadCategories()
        {
            List<DtoCategory> categories;

            const string cacheKey = cacheKeyListItems;
            categories = Cache.Get<List<DtoCategory>>(cacheKey, makeCopy: false);

            if (categories == null || Debugger.IsAttached)
            {
                var rootPath = (string)AppDomain.CurrentDomain.GetData(AppDomainConstants.ContentRootPath);

                categories = Shared.Serialization.FromXML<List<DtoCategory>>(Path.Combine(rootPath, _XML_FILE));
                Shared.Cache.Insert(cacheKey, categories, CacheConstants.DefaultExpirationTime, makeCopy: false);
            }

            return categories ?? new List<DtoCategory>();
        }
    }
}
