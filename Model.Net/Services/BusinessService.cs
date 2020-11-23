namespace Shared.Services
{
    using Model.DTO.Projects;

    using Shared;
    using Shared.Constants;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class BusinessService : IBusinessService
    {
        #region " Constants "

        private static string _XML_FILE = "xml-data\\Business.xml";

        /// <summary>
        /// Clave de cache en la que almacenamos el listado completo.
        /// </summary>
        private const String cacheKeyListItems = "Business_GetList";

        #endregion

        public List<DtoBusiness> LoadBusiness()
        {
            List<DtoBusiness> business;

            const string cacheKey = cacheKeyListItems;
            business = Cache.Get<List<DtoBusiness>>(cacheKey, makeCopy: false);

            if (business == null || Debugger.IsAttached)
            {
                var rootPath = (string)AppDomain.CurrentDomain.GetData(AppDomainConstants.ContentRootPath);

                business = Shared.Serialization.FromXML<List<DtoBusiness>>(Path.Combine(rootPath, _XML_FILE));
                Shared.Cache.Insert(cacheKey, business, CacheConstants.DefaultExpirationTime, makeCopy: false);
            }

            return business ?? new List<DtoBusiness>();
        }
    }
}
