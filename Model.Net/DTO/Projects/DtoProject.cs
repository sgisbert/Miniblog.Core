#region " Espacios de nombres "

using System;
using System.Collections.Generic;

#endregion

namespace Model.DTO.Projects
{
    /// Generated at 17/12/2013
    [Serializable]
	public partial class DtoProject : DtoBase, IDtoProject
	{
		#region " Atributos "

		/// <summary>
		/// Fechas del proyecto
		/// </summary>
		private String date;

		/// <summary>
		/// Descripción del proyecto
		/// </summary>
		private String description;

		/// <summary>
		/// Descripción extendida del proyecto
		/// </summary>
		private String fullDescription;

		/// <summary>
		/// Nombre del proyecto
		/// </summary>
		private String name;

		/// <summary>
		/// Slug del proyecto
		/// </summary>
		private String slug;

		/// <summary>
		/// Tareas desarrolladas en el proyecto
		/// </summary>
		private String tasks;

		/// <summary>
		/// Listado de tecnologías del proyecto
		/// </summary>
		private List<String> technologies;

		/// <summary>
		/// URL del proyecto
		/// </summary>
		private String url;

		/// <summary>
		/// TODO: documentación pendiente
		/// </summary>
		private List<DtoCategory> categories;

		/// <summary>
		/// TODO: documentación pendiente
		/// </summary>
		//private DtoClient client;

		/// <summary>
		/// TODO: documentación pendiente
		/// </summary>
		//private List<DtoImage> images;

		/// <summary>
		/// TODO: documentación pendiente
		/// </summary>
		//private DtoBusiness business;

		/// <summary>
		/// Para prevenir disposiciones sucesivas
		/// </summary>
		private bool disposed; 
		
		#endregion
	
		#region " Propiedades "
		
		/// <summary>
		/// Obtiene o establece "Fechas del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Fechas del proyecto"</value>
		public String Date
		{
			get { return date; }
			set { date = value; }
		}
		
		/// <summary>
		/// Obtiene o establece "Descripción del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Descripción del proyecto"</value>
		public String Description
		{
			get { return description; }
			set { description = value; }
		}
		
		/// <summary>
		/// Obtiene o establece "Descripción extendida del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Descripción extendida del proyecto"</value>
		public String FullDescription
		{
			get { return fullDescription; }
			set { fullDescription = value; }
		}
		
		/// <summary>
		/// Obtiene o establece "Nombre del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Nombre del proyecto"</value>
		public String Name
		{
			get { return name; }
			set { name = value; }
		}
		
		/// <summary>
		/// Obtiene o establece "Slug del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Slug del proyecto"</value>
		public String Slug
		{
			get { return slug; }
			set { slug = value; }
		}
		
		/// <summary>
		/// Obtiene o establece "Tareas desarrolladas en el proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "Tareas desarrolladas en el proyecto"</value>
		public String Tasks
		{
			get { return tasks; }
			set { tasks = value; }
		}
		
		/// <summary>
		/// Obtiene o establece "Listado de tecnologías del proyecto"
		/// </summary>
		/// <value><see cref="List<String>"/>Obtiene o establece "Listado de tecnologías del proyecto"</value>
		public List<String> Technologies
		{
			get { return technologies; }
			set { technologies = value; }
		}
		
		/// <summary>
		/// Obtiene o establece "URL del proyecto"
		/// </summary>
		/// <value><see cref="String"/>Obtiene o establece "URL del proyecto"</value>
		public String Url
		{
			get { return url; }
			set { url = value; }
		}
		
		/// <summary>
		/// TODO: documentación pendiente
		/// </summary>
		public List<DtoCategory> Categories
		{
			get { return categories; }
			set { categories = value; }
		}

		/// <summary>
		/// TODO: documentación pendiente
		/// </summary>
		//public DtoClient Client
		//{
		//	get { return client; }
		//	set { client = value; }
		//}

		/// <summary>
		/// TODO: documentación pendiente
		/// </summary>
		//public List<DtoImage> Images
		//{
		//	get { return images; }
		//	set { images = value; }
		//}

		/// <summary>
		/// TODO: documentación pendiente
		/// </summary>
		//public DtoBusiness Business
		//{
		//	get { return business; }
		//	set { business = value; }
		//}

		#endregion
	
		#region " Constructores "
		
		/// <summary>
	    /// Inicializa una nueva instancia de la class <see cref="DtoProject"/>.
	    /// </summary>
		public DtoProject() { }
	
	    /// <summary>
	    /// Inicializa una nueva instancia de la class <see cref="DtoProject"/>.
	    /// </summary>
	    /// <param name="instantiatedObject">Si es <c>true</c> instancia los objetos que formen parte de esta clase.</param>
		public DtoProject(Boolean instantiatedObject=false)
		{
			if (!instantiatedObject) return;
			//Client = new DtoClient();
			//Business = new DtoBusiness();
			categories = new List<DtoCategory>();
			//images = new List<DtoImage>();
		}
	
		/// <summary>
	    /// Finaliza una instancia de la class <see cref="DtoProject"/> y libera sus recursos.
	    /// </summary>
		~DtoProject() 
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
				if (disposing)
				{
					//if(Client != null)
					//{
					//	Client.Dispose();
					//	Client = null;
					//}	
					//if(Business != null)
					//{
					//	Business.Dispose();
					//	Business = null;
					//}	
					categories = null;
	
					//images = null;
	
				}
				disposed = true;
			}
		}
	
		#endregion
	
		#region " Sobrecargas "

		/// <summary>
		/// Sobrecarga del método ToString() para la this.Element DtoProject
		/// </summary>
		/// <returns>Un <see cref="System.String"/> que representa este DtoProject</returns>
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
			if (x.GetType() != typeof(DtoProject)) return -1;
			if (y.GetType() != typeof(DtoProject)) return 1;
		
			return ((DtoProject)x).CompareTo(y);
		}
		
		/// <summary>
		/// Compara la instancia con el objeto recibe por parámetro.
		/// </summary>
		/// <param name="obj">Objeto con el que se compara la instancia.</param>
		/// <returns>-1: el parámetro es mayor que la instancia; 0: son iguales; 1: la instancia es mayor que el parámetro</returns>
		public override int CompareTo(object obj)
		{
			if ((obj == null) || ((obj.GetType() != typeof(DtoProject)))) return 1;
			
			DtoProject dtoProject = (DtoProject)obj;
			if (Id != dtoProject.Id)
				return (Id < dtoProject.Id) ? -1 : 1;
			if (!String.Equals(Name, dtoProject.Name, StringComparison.CurrentCultureIgnoreCase))
				return (String.Compare(Name, dtoProject.Name, StringComparison.CurrentCultureIgnoreCase));
			
			return 0;
		}
		
		/// <summary>
		/// Compara la instancia con el objeto recibe por parámetro.
		/// </summary>
		/// <param name="obj">Objeto con el que se compara la instancia.</param>
		/// <returns>True son iguales los objeto, false tienen algún campo distinto</returns>
		public override Boolean Equals(object obj)
		{
			if ((obj == null) || ((obj.GetType() != typeof (DtoProject)))) return false;
		
			DtoProject project = (DtoProject)obj;
			return Equals(this, project);
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
		
			if (x.GetType() != typeof(DtoProject)) return false;
			if (y.GetType() != typeof(DtoProject)) return false;
		
			DtoProject project1 = (DtoProject)x;
			DtoProject project2 = (DtoProject)y;
			if (project1.Id != project2.Id) return false;		
			if (!String.Equals(project1.Name, project2.Name, StringComparison.CurrentCultureIgnoreCase)) return false;
			return true;
		}
		
		#endregion
	}
}
