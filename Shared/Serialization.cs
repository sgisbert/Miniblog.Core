using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;


namespace Shared
{
    /// <summary>
    /// Utilidades de serialización
    /// </summary>
    /// 19/05/2011 by sergi
    public static class Serialization
    {
        #region " XML "

        /// <summary>
        /// Method to convert a custom Object to an XML file
        /// </summary>
        /// <typeparam name="T">Tipo de datos a serializar</typeparam>
        /// <param name="pObject">Object that is to be serialized to XML</param>
        /// <param name="path">Ruta del archivo XML de salida</param>
        /// <returns>
        /// True si se ha serializado correctamente. False si da error
        /// </returns>
        public static Boolean ToXML<T>(this Object pObject, string path)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(T));
                TextWriter w = new StreamWriter(path, false, Encoding.UTF8);
                s.Serialize(w, pObject);
                w.Close();
                return true;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <typeparam name="T">Tipo de datos a serializar</typeparam>
        /// <param name="pObject">Object that is to be serialized to XML</param>
        /// <returns>XML string</returns>
        /// 22/07/2010 by sergi
        public static String ToXML<T>(this Object pObject)
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            StringBuilder b = new StringBuilder();
            using (StringWriter wr = new StringWriter(b))
            {
                x.Serialize(wr, pObject);
            }
            return b.ToString();
        }

        /// <summary>
        /// Method to reconstruct an Object from XML string
        /// </summary>
        /// <typeparam name="T">Tipo de datos a serializar</typeparam>
        /// <param name="pXmlizedString">Cadena XML o ruta del archivo XML de entrada.</param>
        /// <returns></returns>
        public static T FromXML<T>(String pXmlizedString)
        {
            try
            {
                String xml = pXmlizedString;
                if (File.Exists(pXmlizedString))
                    xml = FileUtils.ReadFromFile(pXmlizedString);
                var xs = new XmlSerializer(typeof(T));
                var memoryStream = new MemoryStream(StringToUTF8ByteArray(xml));
                var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                return (T)xs.Deserialize(memoryStream);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Method to reconstruct an Object from an XML file
        /// </summary>
        /// <typeparam name="T">Tipo de datos a serializar</typeparam>
        /// <param name="path">The p xmlized string.</param>
        /// <returns></returns>
        public static T FromXML<T>(Uri path)
        {
            return (T)FromXML<T>(FileUtils.ReadFromFile(path.ToString()));
        }


        #endregion

        #region " JSON "

        /// <summary>
        /// Convierte el objeto a un archivo json.
        /// </summary>
        /// <param name="obj">El Objeto a convertir a JSON</param>
        /// <param name="path">El archivo de salida</param>
        /// <param name="nullValue">Incluir los atributos nulos</param>
        /// <param name="defaultValue">Incluir los atributos con valores por defecto</param>
        /// <param name="format">Dar formato al archivo de salida</param>
        /// <returns>True si se ha serializado correctamente. False si da error</returns>
        /// 22/07/2010 by sergi
        public static Boolean ToJson(this Object obj, string path, NullValueHandling nullValue = NullValueHandling.Ignore, DefaultValueHandling defaultValue = DefaultValueHandling.Ignore, Newtonsoft.Json.Formatting format = Newtonsoft.Json.Formatting.None)
        {
            try
            {
                JsonSerializer json = new JsonSerializer();
                json.NullValueHandling = nullValue;
                json.DefaultValueHandling = defaultValue;

                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        writer.Formatting = format;
                        json.Serialize(writer, obj);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Convierte el objeto a un String json.
        /// </summary>
        /// <param name="obj">El Objeto a convertir a JSON</param>
        /// <param name="nullValue">Incluir los atributos nulos</param>
        /// <param name="defaultValue">Incluir los atributos con valores por defecto</param>
        /// <param name="format">Dar formato al archivo de salida</param>
        /// <returns>String con el contenido JSON</returns>
        /// 22/07/2010 by sergi
        public static String ToJson(this Object obj, NullValueHandling nullValue = NullValueHandling.Ignore, DefaultValueHandling defaultValue = DefaultValueHandling.Ignore, Newtonsoft.Json.Formatting format = Newtonsoft.Json.Formatting.None)
        {
            try
            {
                JsonSerializer json = new JsonSerializer();
                json.NullValueHandling = nullValue;
                json.DefaultValueHandling = defaultValue;

                StringBuilder b = new StringBuilder();
                using (StringWriter wr = new StringWriter(b))
                {
                    using (JsonWriter writer = new JsonTextWriter(wr))
                    {
                        writer.Formatting = format;
                        json.Serialize(writer, obj);
                    }
                }
                return b.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Devuelve un objeto a partir de una cadena JSON
        /// </summary>
        /// <typeparam name="T">Tipo de datos a deserializar.</typeparam>
        /// <param name="pString">Cadena JSON o ruta del archivo JSON de entrada.</param>
        /// <returns></returns>
        /// 22/07/2010 by sergi
        public static T FromJson<T>(string pString)
        {
            String json = pString;
            if (File.Exists(pString))
                json = FileUtils.ReadFromFile(pString);

            return (T)JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Devuelve un objeto a partir de una cadena JSON y su Type
        /// </summary>
        /// <param name="pString">>Cadena JSON.</param>
        /// <param name="tipo">Tipo de datos a deserializar</param>
        /// <returns></returns>
        /// 06/09/2010 by sergi
        public static object FromJson(string pString, Type tipo)
        {
            return JsonConvert.DeserializeObject(pString, tipo);
        }
        #endregion

        #region " Métodos auxiliares "
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            var encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            var encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
        #endregion 
    }
}
