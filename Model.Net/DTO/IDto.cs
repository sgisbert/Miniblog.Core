using System;

namespace Model.DTO
{
    /// <summary>
    /// Interfaz que define un DTO Base
    /// </summary>
    /// 26/02/2013 by Sergi
    public interface IDto : IDisposable
    {
        /// <summary>
        /// Obtiene o establece "Identificador de base de datos"
        /// </summary>
        /// <value><see cref="Int32"/>Obtiene o establece "Identificador de base de datos"</value>
        /// 30/01/2013 by Sergi
        Int32 Id { get; set; }

        /// <summary>
        /// Obtiene o establece "Identificador alternativo"
        /// </summary>
        /// <value><see cref="String"/>Obtiene o establece "Identificador alternativo"</value>
        /// 30/01/2013 by Sergi
        String AlternateId { get; set; }

        /// <summary>
        /// Obtiene la fecha de creación del objeto
        /// </summary>
        /// 07/02/2013 by Sergi
        DateTime DateCreated { get; set; }

        /// <summary>
        /// Obtiene la cadena que representa a este objeto.
        /// </summary>
        /// <value>
        /// Realiza una llamada al método ToString() de esta clase.
        /// </value>
        /// 30/01/2013 by Sergi
        String Value { get; set; }
    }
}
