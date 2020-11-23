#region " Espacios de nombres "

using System;
using System.Collections.Generic;

#endregion

namespace Model.DTO.Projects
{
    /// Generated at 17/12/2013
    public interface IDtoProject : IDto
	{
		#region " Propiedades "
	
		/// <summary>
		/// Obtiene o establece "Nombre del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Nombre del proyecto"</value>
		String Name { get; set; }
		
		/// <summary>
		/// Obtiene o establece "Descripción del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Descripción del proyecto"</value>
		String Description { get; set; }
		
		/// <summary>
		/// Obtiene o establece "URL del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "URL del proyecto"</value>
		String Url { get; set; }
		
		/// <summary>
		/// Obtiene o establece "Fechas del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Fechas del proyecto"</value>
		String Date { get; set; }
		
		/// <summary>
		/// Obtiene o establece "Slug del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Slug del proyecto"</value>
		String Slug { get; set; }
		
		/// <summary>
		/// Obtiene o establece "Listado de tecnologías del proyecto"
		/// </summary>
		/// <value><see cref="List<String>"/>Obtiene o establece "Listado de tecnologías del proyecto"</value>
		List<String> Technologies { get; set; }
		
		/// <summary>
		/// Obtiene o establece "Tareas desarrolladas en el proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Tareas desarrolladas en el proyecto"</value>
		String Tasks { get; set; }
		
		/// <summary>
		/// Obtiene o establece "Descripción extendida del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Descripción extendida del proyecto"</value>
		String FullDescription { get; set; }
		
		#endregion
		
	}
}
