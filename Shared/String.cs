#region  Espacios de nombres 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Shared.Diff;

#endregion

namespace Shared.Strings 
{
    /// <summary>
    /// Extensiones de utilidad para la clase String
    /// </summary>
    /// 01/11/2010 by sergi 
    public static class Cadena  
    {
        /// <summary>
        /// Tamaño por defecto para la función recortaTexto
        /// </summary>
        public const Int16 DefaultRecortaTexto = 20;  

        /// <summary>
        /// Función para asegurar que el tamaño de la cadena no exede el número de caracteres indicado
        /// </summary>
        /// <param name="cadena">Cadena a recortar en el caso de que exceda el tamaño</param>
        /// <param name="maxSize">Tamaño máximo de carácteres que puede tener la cadena</param>
        /// <returns>Cadena con el número de carácteres limitado al indicado</returns>
        public static String RecortaTexto(this String cadena, Int32 maxSize = DefaultRecortaTexto)
        {
			if (maxSize == 0) return String.Empty;
            if (maxSize >= cadena.Length) return cadena;

            Int32 lastSpace = cadena.Substring(0, maxSize).LastIndexOf(" ", StringComparison.CurrentCultureIgnoreCase);
            if (lastSpace != -1) maxSize = lastSpace;
            return cadena.Substring(0, maxSize) + "...";
        }

        /// <summary>
        /// Pone la primera letra de la cadena en mayúsculas.
        /// </summary>
        /// <param name="s">Cadena a capitalizar.</param>
        /// <returns>Cadena con la primera letra en mayúsculas.</returns>
        /// 08/04/2011 by corcoles
        public static String Capitalize(this String s)
        {
            return s.Substring(0, 1).ToUpper() + (s.Length > 1 ? s.Substring(1) : "");
        }

        /// <summary>
        /// Pone la primera letra de la cadena en minúsculas.
        /// </summary>
        /// <param name="s">Cadena a capitalizar.</param>
        /// <returns>Cadena con la primera letra en mayúsculas.</returns>
        /// 08/04/2011 by corcoles
        public static String Uncapitalize(this String s)
        {
            return s.Substring(0, 1).ToLower() + (s.Length > 1 ? s.Substring(1) : "");
        }

        /// <summary>
        /// Normaliza una cadena, eliminando los acentos de las vocales que haya.
        /// </summary>
        /// <param name="s">Cadena a normalizar.</param>
        /// <returns>Cadena normalizada sin acentos.</returns>
        /// 25/05/2011 by corcoles
        public static String ToNormalized(this String s)
        {
            // Reemplazamos las vocales acentuadas en base a las expresiones regulares definidas
            s = Regex.Replace(s, "[á|à|ä|â]", "a", RegexOptions.None);
            s = Regex.Replace(s, "[é|è|ë|ê]", "e", RegexOptions.None);
            s = Regex.Replace(s, "[í|ì|ï|î]", "i", RegexOptions.None);
            s = Regex.Replace(s, "[ó|ò|ö|ô]", "o", RegexOptions.None);
            s = Regex.Replace(s, "[ú|ù|ü|û]", "u", RegexOptions.None);

            s = Regex.Replace(s, "[Á|À|Ä|Â]", "A", RegexOptions.None);
            s = Regex.Replace(s, "[É|È|Ë|Ê]", "E", RegexOptions.None);
            s = Regex.Replace(s, "[Í|Ì|Ï|Î]", "I", RegexOptions.None);
            s = Regex.Replace(s, "[Ó|Ò|Ö|Ô]", "O", RegexOptions.None);
            s = Regex.Replace(s, "[Ú|Ù|Ü|Û]", "U", RegexOptions.None);

            return s;
        }

        /// <summary>
        /// Normaliza una cadena para su uso como código HTML. Sustituye ciertos caracteres por sus equivalentes HTMLEntities.
        /// </summary>
        /// <param name="s">Cadena a normalizar.</param>
        /// <returns>Cadena normalizada para su uso como código HTML.</returns>
        /// 13/09/2011 by corcoles
        public static String NormalizeHtml(this String s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            s = s.Replace(" ", "&nbsp;");
            s = s.Replace("-", "&ndash;");
			s = s.Replace("á", "&aacute;");
			s = s.Replace("Á", "&Aacute;");
			s = s.Replace("é", "&eacute;");
			s = s.Replace("É", "&Eacute;");
			s = s.Replace("í", "&iacute;");
			s = s.Replace("Í", "&Iacute;");
			s = s.Replace("ó", "&oacute;");
			s = s.Replace("Ó", "&Oacute;");
			s = s.Replace("ú", "&uacute;");
			s = s.Replace("Ú", "&Uacute;");
			s = s.Replace("ñ", "&ntilde;");
			s = s.Replace("Ñ", "&Ntilde;");
            s = s.Replace("\r\n", "<br/>");
            s = s.Replace("\n", "<br/>");

            return s;
        }

        /// <summary>
        /// Elimina los tags HTML de la cadena de entrada.
        /// </summary>
        /// <param name="inputHtml"><see cref="String"/> de entrada con tags HTML.</param>
        /// <returns>Texto plano sin tags HTML.</returns>
        /// 06/04/2012 by corcoles
        public static String CleanHtmlTags(this String inputHtml)
        {
            if (String.IsNullOrEmpty(inputHtml))
                return inputHtml;

            String plainText = Regex.Replace(inputHtml, "<[^>]*>", String.Empty);
            plainText = HttpUtility.HtmlDecode(plainText.Replace("&nbsp;", " "));
            return plainText.Replace("\r", String.Empty).Replace("\n", "<br/>");
        }

        /// <summary>
        /// Obtiene las diferencias entre el contenido de dos cadenas.
        /// </summary>
        /// <param name="s1">Primera cadena a comparar.</param>
        /// <param name="s2">Segunda cadena a comparar.</param>
        /// <returns><see cref="List{DiffResultSpan}"/> con las diferencias entre el contenido de las dos cadenas comparadas.</returns>
        /// 16/10/2012 by corcoles
        public static List<DiffResultSpan> Diff(this String s1, String s2)
        {
            // Montamos una lista con los elementos a comparar
            List<String> comparedItems = new List<String> {s1, s2};

            // Realizamos la comparación de los dos elementos
            List<DiffList_CharData> versionDiffData = comparedItems.Select(item => new DiffList_CharData(item)).ToList();
            
            DiffEngine diffEngine = new DiffEngine();
            diffEngine.ProcessDiff(versionDiffData.First(), versionDiffData.Last(), DiffEngineLevel.FastImperfect);

            return diffEngine.DiffReport().Cast<DiffResultSpan>().ToList();
        }

		/// <summary>
		/// Devuelve una cadena con los indentificadores separados por comas
		/// </summary>
		/// <typeparam name="T">Tipo de elementos de la lista.</typeparam>
		/// <param name="lista">Lista de objetos de la que queremos sus identificadores</param>
		/// <param name="delimiter">Delimitador de los elementos de la cadena.</param>
		/// <returns>
		/// Cadena con los indentificadores separados por comas
		/// </returns>
		/// Generated at 30/06/2011
		public static String ToString<T>(List<T> lista, String delimiter = ",")
		{
			String id = "";
			if (lista == null) return id;

			id = lista.Aggregate(id, (current, objId) => current + (objId.ToString() + delimiter));
			if (id.Length > 0) id = id.Substring(0, id.Length - delimiter.Length);

			return id;
		}

		/// <summary>
		/// Genera una lista de elementos del tipo seleccionado con los valores que vienen en la cadena delimitados con el separador elegido.
		/// </summary>
		/// <typeparam name="T">Tipo de la lista a devolver.</typeparam>
		/// <param name="s">Cadena con los valores.</param>
		/// <param name="delimiter">Delimitador de los elementos de la cadena.</param>
		/// <returns>
		///   <see cref="List{T}"/> con los valores de la cadena transformados a lista.
		/// </returns>
		/// 11/07/2011 by fran
		public static List<T> ToList<T>(this String s, String delimiter = ",")
		{
			return (String.IsNullOrEmpty(s))
			       	? null
			       	: s.Split(',').Select(id => (T) Convert.ChangeType(id, typeof (T))).ToList();
		}

		/// <summary>
		/// Genera una cadena aleatoria
		/// </summary>
		/// <param name="size">Tamaño de la cadena a generar</param>
		/// <param name="lowerCase">Si es <c>true</c> la cadena generado sólo contiene minusculas</param>
		/// <param name="includeNumbers">Si es <c>true</c> la cadena puede incluir número, si es <c>false</c> sólo tendrá letras.</param>
		/// <returns>
		/// Cadena aleatoria con el tamaño estipulado
		/// </returns>
		public static String RandomString(Int32 size, Boolean lowerCase = false, Boolean includeNumbers = false)
		{
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (Int32 i = 0; i < size; i++)
			{
				Boolean switcher = (random.Next(0, 2) == 1);
				Char ch = Convert.ToChar(Convert.ToInt32(System.Math.Floor(26 * random.NextDouble() + 65)));
				Int32 number = random.Next(0, 10);
				builder.Append((includeNumbers && switcher) ? number.ToString(CultureInfo.InvariantCulture)[0] : ch);
			}

			return lowerCase ? builder.ToString().ToLower() : builder.ToString();
		}

		/// <summary>
		/// Convierte un valor numérico a cadena formateada en el tipo que más se ajusta a la cantidad.
		/// </summary>
		/// <typeparam name="T">Tipo de datos de la entrada a formatear</typeparam>
		/// <param name="n">Número de bytes.</param>
		/// <returns>
		///   <see cref="String"/> formateado.
		/// </returns>
		/// <exception cref="ArgumentException"> si el tipo del argumento no es numérico.</exception>
		/// 02/11/2011 by fran
		public static String ToFileSize<T>(this T n)
		{
			Int64 y = Convert.ToInt64(n);
			if (System.Math.Abs(Convert.ToDouble(n) - Convert.ToDouble(y)) > 0)
				throw new ArgumentException("Input was not an integer");

			Double max = 1024*1024*1024;
			if (y >= max)
				return String.Format("{0:0.##} Gb", y / max);
			max = 1024*1024;
			if (y >= max)
				return String.Format("{0:0.##} Mb", y / max);
			if (y >= 1024)
				return String.Format("{0:0.##} Kb", (Double)y / 1024);

			return String.Format("{0} bytes", y);
		}
    }
}
