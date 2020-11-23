#region " Espacios de nombres "

using System;

#endregion

namespace Model.DTO.Projects
{
    /// Generated at 16/12/2013
    [Serializable]
	public partial class DtoCategory : DtoBase, IDtoCategory
	{
		#region " Atributos "

		/// <summary>
		/// Nombre de la categoría
		/// </summary>
		private String name;

		/// <summary>
		/// Para prevenir disposiciones sucesivas
		/// </summary>
		private bool disposed; 
		
		#endregion
	
		#region " Propiedades "
		
		/// <summary>
		/// Obtiene o establece "Nombre de la categoría"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Nombre de la categoría"</value>
		public String Name
		{
			get { return name; }
			set { name = value; }
		}
		
		#endregion
	
		#region " Constructores "
		
		/// <summary>
	    /// Inicializa una nueva instancia de la class <see cref="DtoCategory"/>.
	    /// </summary>
		public DtoCategory() { }
	
	    /// <summary>
	    /// Inicializa una nueva instancia de la class <see cref="DtoCategory"/>.
	    /// </summary>
	    /// <param name="instantiatedObject">Si es <c>true</c> instancia los objetos que formen parte de esta clase.</param>
		public DtoCategory(Boolean instantiatedObject=false)
		{
			if (!instantiatedObject) return;
		}
	
		/// <summary>
	    /// Finaliza una instancia de la class <see cref="DtoCategory"/> y libera sus recursos.
	    /// </summary>
		~DtoCategory() 
	    {
	        Dispose(false);
	    }
	
	    /// <summary>
	    /// Libera el espacio reservado por sus objetos.
	    /// </summary>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				disposed = true;
			}
		}
	
		#endregion
	
		#region " Sobrecargas "

		/// <summary>
		/// Sobrecarga del método ToString() para la this.Element DtoCategory
		/// </summary>
		/// <returns>Un <see cref="System.String"/> que representa este DtoCategory</returns>
		public override String ToString()
		{
			return (String.IsNullOrEmpty(name) ? "" : name);
		}
		
		/// <summary>
		/// Delegado que se utiliza para ordenar las listas tipadas.
		/// </summary>
		/// <param name="x">Primer elemento a comparar</param>
		/// <param name="y">Segundo elemento a comparar</param>
		/// <returns>-1: y es mayor que x; 0: son iguales; 1: x es mayor que y</returns>
		public static int Compare(object x, object y)
		{
			if (x == null) return (y == null) ? 0 : -1;
			if (y == null) return 1;
			if (x.GetType() != typeof(DtoCategory)) return -1;
			if (y.GetType() != typeof(DtoCategory)) return 1;
		
			return ((DtoCategory)x).CompareTo(y);
		}
		
		/// <summary>
		/// Compara la instancia con el objeto recibe por parámetro.
		/// </summary>
		/// <param name="obj">Objeto con el que se compara la instancia.</param>
		/// <returns>-1: el parámetro es mayor que la instancia; 0: son iguales; 1: la instancia es mayor que el parámetro</returns>
		public override int CompareTo(object obj)
		{
			if ((obj == null) || ((obj.GetType() != typeof(DtoCategory)))) return 1;
			
			DtoCategory dtoCategory = (DtoCategory)obj;
			if (Id != dtoCategory.Id)
				return (Id < dtoCategory.Id) ? -1 : 1;
			if (!String.Equals(Name, dtoCategory.Name, StringComparison.CurrentCultureIgnoreCase))
				return (String.Compare(Name, dtoCategory.Name, StringComparison.CurrentCultureIgnoreCase));
			
			return 0;
		}
		
		/// <summary>
		/// Compara la instancia con el objeto recibe por parámetro.
		/// </summary>
		/// <param name="obj">Objeto con el que se compara la instancia.</param>
		/// <returns>True son iguales los objeto, false tienen algún campo distinto</returns>
		public override Boolean Equals(object obj)
		{
			if ((obj == null) || ((obj.GetType() != typeof (DtoCategory)))) return false;
		
			DtoCategory category = (DtoCategory)obj;
			return Equals(this, category);
		}
		
		/// <summary>
		/// Compara si las dos instancias son iguales.
		/// </summary>
		/// <param name="x">Primera instancia a comparar.</param>
		/// <param name="y">Segunda instancia a comparar.</param>
		/// <returns>Determina si las dos instancias tienen todos los campos iguales.</returns>
		public static new Boolean Equals(object x, object y)
		{
			if ((x == null) && (y == null)) return true;
			if ((x == null) || (y == null)) return false;
		
			if (x.GetType() != typeof(DtoCategory)) return false;
			if (y.GetType() != typeof(DtoCategory)) return false;
		
			DtoCategory category1 = (DtoCategory)x;
			DtoCategory category2 = (DtoCategory)y;
			if (category1.Id != category2.Id) return false;		
			if (!String.Equals(category1.Name, category2.Name, StringComparison.CurrentCultureIgnoreCase)) return false;
			return true;
		}
		
		#endregion
	}
}
