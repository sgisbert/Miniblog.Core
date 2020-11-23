#region " Espacios de nombres "

using System;

#endregion

namespace Model.DTO.Projects
{
    /// Generated at 16/12/2013
    [Serializable]
	public partial class DtoClient : DtoBase, IDtoClient
	{
		#region " Atributos "

		/// <summary>
		/// Nombre del cliente
		/// </summary>
		private String name;

		/// <summary>
		/// URL del cliente
		/// </summary>
		private String url;

		/// <summary>
		/// Para prevenir disposiciones sucesivas
		/// </summary>
		private bool disposed; 
		
		#endregion
	
		#region " Propiedades "
		
		/// <summary>
		/// Obtiene o establece "Nombre del cliente"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Nombre del cliente"</value>
		public String Name
		{
			get { return name; }
			set { name = value; }
		}
		
		/// <summary>
		/// Obtiene o establece "URL del cliente"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "URL del cliente"</value>
		public String Url
		{
			get { return url; }
			set { url = value; }
		}
		
		#endregion
	
		#region " Constructores "
		
		/// <summary>
	    /// Inicializa una nueva instancia de la class <see cref="DtoClient"/>.
	    /// </summary>
		public DtoClient() { }
	
	    /// <summary>
	    /// Inicializa una nueva instancia de la class <see cref="DtoClient"/>.
	    /// </summary>
	    /// <param name="instantiatedObject">Si es <c>true</c> instancia los objetos que formen parte de esta clase.</param>
		public DtoClient(Boolean instantiatedObject=false)
		{
			if (!instantiatedObject) return;
		}
	
		/// <summary>
	    /// Finaliza una instancia de la class <see cref="DtoClient"/> y libera sus recursos.
	    /// </summary>
		~DtoClient() 
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
		/// Sobrecarga del método ToString() para la this.Element DtoClient
		/// </summary>
		/// <returns>Un <see cref="System.String"/> que representa este DtoClient</returns>
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
			if (x.GetType() != typeof(DtoClient)) return -1;
			if (y.GetType() != typeof(DtoClient)) return 1;
		
			return ((DtoClient)x).CompareTo(y);
		}
		
		/// <summary>
		/// Compara la instancia con el objeto recibe por parámetro.
		/// </summary>
		/// <param name="obj">Objeto con el que se compara la instancia.</param>
		/// <returns>-1: el parámetro es mayor que la instancia; 0: son iguales; 1: la instancia es mayor que el parámetro</returns>
		public override int CompareTo(object obj)
		{
			if ((obj == null) || ((obj.GetType() != typeof(DtoClient)))) return 1;
			
			DtoClient dtoClient = (DtoClient)obj;
			if (Id != dtoClient.Id)
				return (Id < dtoClient.Id) ? -1 : 1;
			if (!String.Equals(Name, dtoClient.Name, StringComparison.CurrentCultureIgnoreCase))
				return (String.Compare(Name, dtoClient.Name, StringComparison.CurrentCultureIgnoreCase));
			
			return 0;
		}
		
		/// <summary>
		/// Compara la instancia con el objeto recibe por parámetro.
		/// </summary>
		/// <param name="obj">Objeto con el que se compara la instancia.</param>
		/// <returns>True son iguales los objeto, false tienen algún campo distinto</returns>
		public override Boolean Equals(object obj)
		{
			if ((obj == null) || ((obj.GetType() != typeof (DtoClient)))) return false;
		
			DtoClient client = (DtoClient)obj;
			return Equals(this, client);
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
		
			if (x.GetType() != typeof(DtoClient)) return false;
			if (y.GetType() != typeof(DtoClient)) return false;
		
			DtoClient client1 = (DtoClient)x;
			DtoClient client2 = (DtoClient)y;
			if (client1.Id != client2.Id) return false;		
			if (!String.Equals(client1.Name, client2.Name, StringComparison.CurrentCultureIgnoreCase)) return false;
			return true;
		}
		
		#endregion
	}
}
