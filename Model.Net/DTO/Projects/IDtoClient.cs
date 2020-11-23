#region " Espacios de nombres "

using System;

#endregion

namespace Model.DTO.Projects
{
    /// Generated at 16/12/2013
    public interface IDtoClient : IDto
	{
		#region " Propiedades "
	
		/// <summary>
		/// Obtiene o establece "Nombre del cliente"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Nombre del cliente"</value>
		String Name { get; set; }
		
		/// <summary>
		/// Obtiene o establece "URL del cliente"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "URL del cliente"</value>
		String Url { get; set; }
		
		#endregion
		
	}
}
