#region " Espacios de nombres "

using System;

#endregion

namespace Model.DTO.Projects
{
    /// Generated at 16/12/2013
    public interface IDtoImage : IDto
	{
		#region " Propiedades "
	
		/// <summary>
		/// Obtiene o establece "Nombre de la imagen"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Nombre de la imagen"</value>
		String Name { get; set; }
		
		/// <summary>
		/// Obtiene o establece "ULR de la imagen"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "ULR de la imagen"</value>
		String Url { get; set; }
		
		#endregion
		
	}
}
