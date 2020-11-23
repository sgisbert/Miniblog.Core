#region " Espacios de nombres "

using System;

#endregion

namespace Model.DTO.Projects
{
    /// Generated at 16/12/2013
    public interface IDtoBusiness : IDto
	{
		#region " Propiedades "
	
		/// <summary>
		/// Obtiene o establece "Nombre de la empresa"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Nombre de la empresa"</value>
		String Name { get; set; }
		
		/// <summary>
		/// Obtiene o establece "URL de la empresa"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "URL de la empresa"</value>
		String Url { get; set; }
		
		#endregion
		
	}
}
