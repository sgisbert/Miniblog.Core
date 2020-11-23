namespace Shared.Services
{
    using Model.DTO.Projects;

    using Shared;
    using Shared.Constants;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class ClientsService : IClientsService
    {
        #region " Constants "

        private static string _XML_FILE = "xml-data\\Clients.xml";

        /// <summary>
        /// Clave de cache en la que almacenamos el listado completo.
        /// </summary>
        private const String cacheKeyListItems = "Clients_GetList";

        #endregion

        public List<DtoClient> LoadClients()
        {
            List<DtoClient> clients;

            const string cacheKey = cacheKeyListItems;
            clients = Cache.Get<List<DtoClient>>(cacheKey, makeCopy: false);

            if (clients == null || Debugger.IsAttached)
            {
                var rootPath = (string)AppDomain.CurrentDomain.GetData(AppDomainConstants.ContentRootPath);

                clients = Shared.Serialization.FromXML<List<DtoClient>>(Path.Combine(rootPath, _XML_FILE));
                Shared.Cache.Insert(cacheKey, clients, CacheConstants.DefaultExpirationTime, makeCopy: false);
            }

            return clients ?? new List<DtoClient>();
        }
    }
}
