#region " Espacios de nombres "

using Shared.Strings;

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Model.DTO
{
    /// <summary>
    /// Clase base que define las propiedades comunes de un DTO
    /// </summary>
    /// 30/01/2013 by Sergi
    public abstract class DtoBase : IDto
    {
        #region " Atributos "

        /// <summary>
        /// Identificador de base de datos
        /// </summary>
        /// 30/01/2013 by Sergi
        protected Int32 id = -1;

        /// <summary>
        /// Identificador de base de datos alternativo
        /// </summary>
        /// 30/01/2013 by Sergi
        protected String alternateId = "";

        #endregion

        #region " Propiedades "

        /// <summary>
        /// Obtiene o establece "Identificador de base de datos"
        /// </summary>
        /// <value><see cref="Int32"/>Obtiene o establece "Identificador de base de datos"</value>
        /// 30/01/2013 by Sergi
        public virtual Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Obtiene o establece "Identificador alternativo"
        /// </summary>
        /// <value><see cref="String"/>Obtiene o establece "Identificador alternativo"</value>
        /// 30/01/2013 by Sergi
        public virtual String AlternateId
        {
            get { return alternateId; }
            set { alternateId = value; }
        }

        /// <summary>
        /// Obtiene la fecha de creación del objeto
        /// </summary>
        /// 07/02/2013 by Sergi
        public virtual DateTime DateCreated
        {
            get; set;
        }

        /// <summary>
        /// Obtiene la cadena que representa a este objeto.
        /// </summary>
        /// <value>
        /// Realiza una llamada al método ToString() de esta clase.
        /// </value>
        /// 30/01/2013 by Sergi
        public virtual String Value
        {
            get { return ToString(); }
// ReSharper disable ValueParameterNotUsed
            set { }
// ReSharper restore ValueParameterNotUsed
        }

        #endregion

        #region " Dispose "

        /// <summary>
        /// Libera el espacio reservado por sus objetos.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera el espacio reservado por sus objetos.
        /// </summary>
        protected abstract void Dispose(bool disposing);
	
		#endregion

        #region " Sobrecargas "

        /// <summary>
        /// Sobrecarga del método ToString() para la this.Element DtoAplicacionApi
        /// </summary>
        /// <returns>Un <see cref="System.String"/> que representa este DtoAplicacionApi</returns>
        /// Generated at 16/01/2013
        public new abstract String ToString();

        /// <summary>
        /// Devuelve un <see cref="System.String"/>que representa este DtoBase
        /// </summary>
        /// <param name="maxSize">Tamaño máximo de la cadena que representa un DtoBase</param>
        /// <returns>Un <see cref="System.String"/> que representa este DtoBase</returns>
        /// 30/01/2013 by Sergi
        public virtual String ToString(Int32 maxSize)
        {
            String cadena = ToString();
            return (maxSize > 0) ? cadena.RecortaTexto(maxSize) : "";
        }

        /// <summary>
        /// Devuelve una cadena con los indentificadores de los objetos separados por comas
        /// </summary>
        /// <param name="lista">Lista de objetos de la que queremos sus identificadores</param>
        /// <returns>Cadena con los indentificadores separados por comas</returns>
        /// 30/01/2013 by Sergi
        public static String ToString(IEnumerable<DtoBase> lista)
        {
            // Formateo la lista de objetos a cadena de identificadores
            String id = "";
            if (lista == null) return id;

            id = lista.Aggregate(id, (current, obj) => current + (obj.id.ToString(System.Globalization.CultureInfo.InvariantCulture) + ","));
            if (id.Length > 0) id = id.Substring(0, id.Length - 1);

            return id;
        }

        /// <summary>
        /// Compara la instancia con el objeto recibe por parámetro.
        /// </summary>
        /// <param name="obj">Objeto con el que se compara la instancia.</param>
        /// <returns>-1: el parámetro es mayor que la instancia; 0: son iguales; 1: la instancia es mayor que el parámetro</returns>
        public abstract int CompareTo(object obj);

        /// <summary>
        /// Compara la instancia con el objeto recibe por parámetro.
        /// </summary>
        /// <param name="obj">Objeto con el que se compara la instancia.</param>
        /// <returns>True son iguales los objeto, false tienen algún campo distinto</returns>
        public new abstract Boolean Equals(object obj);

        /// <summary>
        /// Devuelve el hashCode de la instancia
        /// </summary>
        /// <returns>HashCode</returns>
        /// 30/01/2013 by Sergi
        public virtual new int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
