namespace Shared.Services
{
    using Model.DTO.Projects;

    using Shared;
    using Shared.Constants;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class ImagesService : IImagesService
    {
        #region " Constants "

        private static string _XML_FILE = "xml-data\\Images.xml";
        private static string _URL_BASE = "/assets/img/projects/";

        /// <summary>
        /// Clave de cache en la que almacenamos el listado completo.
        /// </summary>
        private const String cacheKeyListItems = "Images_GetList";

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

        public List<DtoImage> LoadImages()
        {
            List<DtoImage> images;

            const string cacheKey = cacheKeyListItems;
            images = Cache.Get<List<DtoImage>>(cacheKey, makeCopy: false);

            if (images == null || Debugger.IsAttached)
            {
                var rootPath = (string)AppDomain.CurrentDomain.GetData(AppDomainConstants.ContentRootPath);
                images = Shared.Serialization.FromXML<List<DtoImage>>(Path.Combine(rootPath, _XML_FILE));
                Cache.Insert(cacheKey, images, CacheConstants.DefaultExpirationTime, makeCopy: false);
            }
            return images ?? new List<DtoImage>();
        }

        public string GetUrl(DtoImage image)
        {
            return _URL_BASE + image.Url;
        }

        public string GetUrlThumbnail(DtoImage image)
        {
            return _URL_BASE + image.Url.Replace(".png", "_thumb.png");
        }
    }
}
