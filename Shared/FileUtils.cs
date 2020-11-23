#region " Espacios de nombres "

using System;
using System.IO;

#endregion

namespace Shared
{
    /// <summary>
    /// Utilidades de ficheros
    /// </summary>
    /// 19/05/2011 by sergi
    public static class FileUtils
    {
        /// <summary>
        /// Lee el archivo completo
        /// </summary>
        /// <param name="filename">Nombre del archivo</param>
        /// <returns>String con el contenido del archivo</returns>
        /// 22/07/2010 by sergi
        public static String ReadFromFile(string filename)
        {
            try
            {
                if (File.Exists(filename))
                    using (StreamReader sr = new StreamReader(filename))
                        return sr.ReadToEnd();

                throw new FileNotFoundException("El archivo '" + filename + "' no existe en la ruta proporcionada.");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Devuelve el nombre del archivo sin la extensión
        /// </summary>
        /// <param name="file">El objeto FileInfo.</param>
        /// <returns></returns>
        /// 19/10/2010 by sergi
        public static String GetNameOnly(this FileInfo file)
        {
            return file.Name.Split('.')[0];
        }

		/// <summary>
		/// Copia el contenido del <see cref="Stream"/> en otro <see cref="Stream"/>.
		/// <para>El <see cref="Stream"/> de destino no se cierra.</para>
		/// </summary>
		/// <param name="s"><see cref="Stream"/> con el contenido a duplicar.</param>
		/// <param name="output"><see cref="Stream"/> destino del contenido.</param>
		/// 23/12/2011 by fran
		public static void CopyToStream(this Stream s, Stream output)
		{
			byte[] buffer = new byte[8 * 1024];
			int len;
			while ((len = s.Read(buffer, 0, buffer.Length)) > 0)
				output.Write(buffer, 0, len);
		}

		/// <summary>
        /// Pasa el contenido del <see cref="Stream"/> a un array de <see cref="byte"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> con el contenido del archivo.</param>
        /// <returns>Array de <see cref="byte"/> del archivo.</returns>
        /// 06/10/2011 by fran
        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] buffer = new byte[16*1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                return ms.ToArray();
            }
        }
    }
}
