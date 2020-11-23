#region  Espacios de nombres 

using System;

#endregion

namespace Shared.Constants
{
    /// <summary>
    /// Definición de constantes relativas al manejo de la cache de datos almacenada en el servidor.
    /// </summary>
    /// 24/11/2011 by corcoles
    public static class Cache
    {
        #region " Valores por defecto "

        /// <summary>
        /// Obtiene el valor por defecto de la duración de un dato en la cache.
        /// </summary>
        /// 24/11/2011 by corcoles
        public static DateTime DefaultExpirationTime
        {
            get { return DateTime.Now.AddMinutes(5); }
        }

        /// <summary>
        /// Obtiene el valor de la duración en la cache de aquellos datos que se refrescarán muy pronto
        /// </summary>
        /// 31/01/2012 by sergi
        public static DateTime MinimumExpirationTime
        {
            get { return DateTime.Now.AddSeconds(30); }
        }

        /// <summary>
        /// Obtiene el valor de la duración en la cache de aquellos datos que no vayan a
        /// tener un tiempo de vida muy elevado.
        /// </summary>
        /// 24/11/2011 by corcoles
        public static DateTime ShortExpirationTime
        {
            get { return DateTime.Now.AddMinutes(2); }
        }

        /// <summary>
        /// Obtiene el valor de la duración en la cache de aquellos datos que vayan a
        /// tener un tiempo de vida moderado.
        /// </summary>
        /// 24/11/2011 by corcoles
        public static DateTime MediumExpirationTime
        {
            get { return DateTime.Now.AddMinutes(15); }
        }

        /// <summary>
        /// Obtiene el valor de la duración en la cache de aquellos datos raramente modificables que tendrán
        /// un tiempo de vida muy elevado.
        /// </summary>
        /// 24/11/2011 by corcoles
        public static DateTime LongExpirationTime
        {
            get { return DateTime.Now.AddMinutes(60); }
        }

        /// <summary>
        /// Obtiene el valor de tiempo asignado a la duración de un dato en la cache para indicar que nunca expira.
        /// </summary>
        /// 24/11/2011 by corcoles
        public static DateTimeOffset NoAbsoluteExpiration
        {
            get { return System.Runtime.Caching.ObjectCache.InfiniteAbsoluteExpiration; }
        }

        #endregion
    }
}
