#region  Espacios de nombres 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

#endregion

namespace Shared
{
    /// <summary>
    /// Clase que encapsula los métodos típicos de manejo de la caché.
    /// </summary>
    /// 21/06/2010 by corcoles
    public static class Cache
    {
        #region " Valores por defecto "

        /// <summary>
        /// Controla si el uso de la caché está habilitado o no 
        /// </summary>
        /// 13/12/2011 by sergi
        public static Boolean Enabled = true;

        /// <summary>
        /// Obtiene el valor por defecto de la duración de un dato en la cache.
        /// </summary>
        /// 21/06/2010 by corcoles
        public static DateTime DefaultExpirationTime
        {
            get { return DateTime.Now.AddMinutes(5); }
        }

        /// <summary>
        /// Obtiene el valor de la duración en la cache de aquellos datos que no vayan a
        /// tener un tiempo de vida muy elevado.
        /// </summary>
        /// 15/06/2011 by corcoles
        public static DateTime ShortExpirationTime
        {
            get { return DateTime.Now.AddMinutes(2); }
        }

        /// <summary>
        /// Obtiene el valor de tiempo asignado a la duración de un dato en la cache para indicar que nunca expira.
        /// </summary>
        /// 13/04/2011 by corcoles
        public static DateTimeOffset NoAbsoluteExpiration
        {
            get { return System.Runtime.Caching.MemoryCache.InfiniteAbsoluteExpiration; }
        }

        /// <summary>
        /// Obtiene el valor de tiempo para indicar que el dato no se mantendrá un tiempo adicional en la cache desde el último acceso al mismo.
        /// </summary>
        /// 13/04/2011 by corcoles
        public static TimeSpan NoSlidingExpiration
        {
            get { return System.Runtime.Caching.MemoryCache.NoSlidingExpiration; }
        }

        #endregion

        #region " Métodos "

        #region " Inserción de datos "

        /// <summary>
        /// Inserta un dato en la cache.
        /// </summary>
        /// <param name="key"><see cref="String"/> con la clave identificativa para almacenar el dato.</param>
        /// <param name="value"><see cref="Object"/> con el dato a insertar.</param>
        /// <param name="dependencies">Dependencia de una clave de la cache respecto al origen del dato que contiene.</param>
        /// <param name="absoluteExpiration"><see cref="DateTime"/> con el tiempo en el cual el tiempo de vida del dato en la cache expirará y será borrado.</param>
        /// <param name="slidingExpiration"><see cref="TimeSpan"/> con el valor de tiempo que se mantendrá el dato en la cache desde que fue accedido por última vez.</param>
        /// <param name="makeCopy">Indica si si guarda una copia del objeto en caché o se pasa por referencia</param>
        /// <returns>
        ///   <c>true</c> si el dato se ha insertado correctamente en la cache; en caso contrario, <c>false</c>.
        /// </returns>
        /// 21/06/2010 by corcoles
        public static Boolean Insert(String key, Object value, DateTime? absoluteExpiration = null, TimeSpan? slidingExpiration = null, bool makeCopy = true)
        {
            if (!Enabled || value == null)
                return true;

            try
            {
                MemoryCache.Default.Add(new CacheItem(key)
                {
                    Value = makeCopy ? value.Copy() : value
                }, new CacheItemPolicy()
                {
                    AbsoluteExpiration = absoluteExpiration ?? DefaultExpirationTime,
                    SlidingExpiration = slidingExpiration ?? TimeSpan.Zero
                });
            }
            catch (Exception e)
            {
                throw new Exception("Error en la inserción del dato en la cache.", innerException: e);
            }

            return true;
        }

        /// <summary>
        /// Elimina de la cache el dato asociado a la clave especificada.
        /// </summary>
        /// <param name="key">Clave de la cache en la que está el dato que queremos eliminar.</param>
        /// <returns><c>true</c> si el dato se ha eliminado correctamente de la cache; en caso contrario, <c>false</c>.</returns>
        /// 13/04/2011 by corcoles
        public static Boolean Remove(String key)
        {
            if (!Enabled)
                return true;

            if (MemoryCache.Default[key] == null)
                return false;

            MemoryCache.Default.Remove(key);
            return true;
        }

        /// <summary>
        /// Elimina de la cache los datos asociados a las claves especificadas.
        /// </summary>
        /// <param name="listKeys">Claves de la cache en las que están los datos que queremos eliminar.</param>
        /// <returns><c>true</c> si se ha eliminado algún dato de la cache; en caso contrario, <c>false</c>.</returns>
        /// 15/06/2011 by corcoles
        public static Boolean RemoveKeys(List<String> listKeys)
        {
            if (!Enabled)
                return true;

            Boolean dataDeleted = false;

            foreach (String key in listKeys.Where(key => MemoryCache.Default[key] != null))
            {
                MemoryCache.Default.Remove(key);
                dataDeleted = true;
            }

            return dataDeleted;
        }

        /// <summary>
        /// Elimina de la cache los datos asociados a claves que comiencen por la cadena especificada.
        /// </summary>
        /// <param name="prefix">Prefijo de las claves a eliminar de la cache.</param>
        /// <returns><c>true</c> si se ha eliminado algún dato de la cache; en caso contrario, <c>false</c>.</returns>
        /// 15/06/2011 by corcoles
        public static Boolean RemoveWithPrefix(String prefix)
        {
            if (!Enabled)
                return true;

            List<String> keysToDelete = new List<String>();

            // Recorremos los elementos almacenados en la cache
            foreach (var item in MemoryCache.Default)
            {
                String cacheKey = item.Key as String;
                if (String.IsNullOrEmpty(cacheKey)) continue;

                if (cacheKey.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                    keysToDelete.Add(cacheKey);
            }

            // No hemos encontrado ninguna clave de cache
            if (keysToDelete.Count == 0)
                return false;

            // Eliminamos las claves de cache que comienzan por el prefijo
            RemoveKeys(keysToDelete);

            return true;
        }

        /// <summary>
        /// Elimina de la cache los datos asociados a claves que contengan la cadena especificada.
        /// </summary>
        /// <param name="text">Texto que deben contener las claves a eliminar de la cache.</param>
        /// <returns><c>true</c> si se ha eliminado algún dato de la cache; en caso contrario, <c>false</c>.</returns>
        /// 15/06/2011 by corcoles
        public static Boolean RemoveWithText(String text)
        {
            if (!Enabled)
                return true;

            List<String> keysToDelete = new List<String>();

            // Recorremos los elementos almacenados en la cache
            foreach (var item in MemoryCache.Default)
            {
                String cacheKey = item.Key as String;
                if (String.IsNullOrEmpty(cacheKey)) continue;

                if (cacheKey.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) != -1)
                    keysToDelete.Add(cacheKey);
            }

            // No hemos encontrado ninguna clave de cache
            if (keysToDelete.Count == 0)
                return false;

            // Eliminamos las claves de cache que contienen el texto
            RemoveKeys(keysToDelete);

            return true;
        }

        #endregion

        #region " Extracción de datos "

        /// <summary>
        /// Obtiene de la cache el dato asociado a una clave especificada.
        /// </summary>
        /// <typeparam name="T">Tipo de datos de salida del dato que queremos recuperar</typeparam>
        /// <param name="cacheKey"><see cref="String"/> con la clave del dato que queremos recuperar.</param>
        /// <param name="makeCopy">Indica si si devuelve una copia del objeto en caché o se pasa por referencia</param>
        /// <returns>Dato asociado a la clave.</returns>
        /// 21/06/2010 by corcoles
        public static T Get<T>(String cacheKey, bool makeCopy = true)
        {
            try
            {
                if (!Enabled)
                    return default(T);

                Object cacheData = MemoryCache.Default[cacheKey];
                if (cacheData == null) return default(T);

                return makeCopy ? (T)cacheData.Copy() : (T)cacheData;
            }
            catch
            {
                return default(T);
            }
        }

        #endregion

        #endregion
    }
}
