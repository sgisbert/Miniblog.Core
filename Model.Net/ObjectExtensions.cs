#region " Espacios de nombres "

using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

#endregion

namespace Model
{
    /// <summary>
    /// Extensión para el manejo de objetos en memoria
    /// </summary>
    /// 19/11/2012 by Sergi
    public static class ObjectExtensions
    {
        #region " Extensión de copia de objetos "

        /// <summary>
        /// Creates a deep copy of a given object instance
        /// </summary>
        /// <typeparam name="TObject">Type of a given object</typeparam>
        /// <param name="instance">Object to be cloned</param>
        /// <param name="throwInCaseOfError">
        /// A value which indicating whether exception should be thrown in case of
        /// error while cloning</param>
        /// <returns>Returns a deep copy of a given object</returns>
        /// <remarks>Uses BinarySerialization to create a true deep copy</remarks>
        public static TObject Copy<TObject>(this TObject instance, bool throwInCaseOfError = false)
            where TObject : class
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            try
            {
                using (var stream = new MemoryStream())
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, instance);
                    stream.Position = 0;
                    return (TObject) binaryFormatter.Deserialize(stream);
                }
            }
            catch
            {
                if (throwInCaseOfError)
                {
                    throw;
                }
            }

            // Si no es serializable, probamos con el método alternativo
            return instance.Clone();
        }

        #endregion

        #region " Helpers "

        /// <summary>
        /// Clones the object, and returns a reference to a cloned object.
        /// </summary>
        /// <returns>Reference to the new cloned object.</returns>
        private static TObject Clone<TObject>(this TObject instance)
            where TObject : class
        {
            var instanceType = instance.GetType();
            var objectInterfaces = instanceType.GetInterfaces();

            //First we create an instance of this specific type.
            var newObject = (TObject)Activator.CreateInstance(instanceType);
            
            // Si el objeto es una lista, recorremos sus elementos para copiarlos
            if (objectInterfaces.Contains(typeof(IList)))
            {
                foreach (var item in (IList) instance)
                    ((IList) newObject).Add(item.Copy());
            }

            // Si el objeto es un diccionario, recorremos sus elementos para copiarlos
            if (objectInterfaces.Contains(typeof (IDictionary)))
            {
                foreach (DictionaryEntry item in (IDictionary) instance)
                    ((IDictionary) newObject)[item.Key] = item.Value.Copy();
            }

            // Recorremos las propiedades del objeto
            PropertyInfo[] properties = newObject.GetType().GetProperties();
            var i = 0;
            foreach (PropertyInfo property in instanceType.GetProperties().Where(p => p.CanWrite))
            {
                var interfaces = property.PropertyType.GetInterfaces();
                try
                {
                    var value = property.GetValue(instance, null);

                    //We query if the fields support the ICloneable interface.
                    properties[i].SetValue(newObject, interfaces.Contains(typeof (ICloneable))
                                                          ? ((ICloneable) value).Clone()
                                                          : value.Copy(), null);

                    //Now we check if the object supports the IEnumerable interface, so if it does
                    //we need to enumerate all its items and check if they support the ICloneable interface.
                    if ( (!interfaces.Contains(typeof (IList)) && !interfaces.Contains(typeof (IDictionary))) || property.PropertyType.IsValueType)
                    {
                        i++;
                        continue;
                    }

                    //Get the IEnumerable interface from the field.
                    var salida = properties[i].GetValue(newObject, null);

                    var j = 0;
                    foreach (object obj in (IEnumerable) value)
                    {
                        if (interfaces.Contains(typeof (IList)))
                        {
                            // Si es IClonable, se utiliza la interfaz, y si no, se copia de nuevo
                            ((IList) salida)[j] = (obj.GetType().GetInterface("ICloneable", true) != null)
                                                      ? ((ICloneable) obj).Clone()
                                                      : obj.Copy();
                        }
                        else if (interfaces.Contains(typeof (IDictionary)))
                        {
                            var de = (DictionaryEntry) obj;
                            //Checking to see if the item support the ICloneable interface.
                            ((IDictionary) salida)[de.Key] = (de.Value.GetType().GetInterface("ICloneable", true) !=
                                                              null)
                                                                 ? ((ICloneable) de.Value).Clone()
                                                                 : de.Value.Copy();
                        }
                        j++;
                    }
                }
                catch { }

                ////This version support the IList and the IDictionary interfaces to iterate on collections.
                //if (interfaces.Contains(typeof(IList)))
                //{
                //    int j = 0;

                //    //Getting the IList interface.
                //    foreach (object obj in (IEnumerable)value)
                //    {
                //        // Si es IClonable, se utiliza la interfaz, y si no, se copia de nuevo
                //        ((IList)salida)[j] = (obj.GetType().GetInterface("ICloneable", true) != null)
                //                                 ? ((ICloneable) obj).Clone()
                //                                 : obj.Copy();
                //        j++;
                //    }
                //}
                //else if (interfaces.Contains(typeof(IDictionary)))
                //{
                //    //Getting the dictionary interface.
                //    foreach (DictionaryEntry de in (IEnumerable)value)
                //    {
                //        //Checking to see if the item support the ICloneable interface.
                //        ((IDictionary)salida)[de.Key] = (de.Value.GetType().GetInterface("ICloneable", true) != null)
                //                                            ? ((ICloneable) de.Value).Clone()
                //                                            : de.Value.Copy();
                //    }
                //}
                i++;
            }

            //We get the array of fields for the new type instance.
            FieldInfo[] fields = newObject.GetType().GetFields();
            i = 0;
            foreach (FieldInfo fi in instanceType.GetFields())
            {
                var interfaces = fi.FieldType.GetInterfaces();
                try
                {
                    var value = fi.GetValue(instance);

                    //We query if the fields supports the ICloneable interface.
                    //Now we check if the object support the IEnumerable interface, so if it does
                    //we need to enumerate all its items and check if they support the ICloneable interface.
                    fields[i].SetValue(newObject,
                                       interfaces.Contains(typeof(ICloneable))
                                           ? value.Clone()
                                           : fields[i].GetValue(instance).Copy());

                    if ((!interfaces.Contains(typeof (IList)) && !interfaces.Contains(typeof (IDictionary))) || fi.GetType().IsValueType)
                    {
                        i++;
                        continue;
                    }

                    //Get the IEnumerable interface from the field.
                    var salida = fields[i].GetValue(newObject);

                    //This version support the IList and the IDictionary interfaces to iterate on collections.
                    var j = 0;
                    foreach (object obj in (IEnumerable) value)
                    {
                        if (interfaces.Contains(typeof (IList)))
                        {
                            ((IList)salida)[j] = obj.GetType().GetInterface("ICloneable", true) != null
                                                     ? ((ICloneable)obj).Clone()
                                                     : obj.Copy();
                        }
                        else if (interfaces.Contains(typeof (IDictionary)))
                        {
                            var de = (DictionaryEntry) obj;
                            ((IDictionary)salida)[de.Key] = de.Value.GetType().GetInterface("ICloneable", true) != null
                                                                ? ((ICloneable)de.Value).Clone()
                                                                : de.Value.Copy();
                        }
                        j++;
                    }
                }
                catch { }

                //if (interfaces.Contains(typeof(IList)))
                //{
                //    int j = 0;
                //    foreach (object obj in (IEnumerable)value)
                //    {
                //        //Checking to see if the current item supports the ICloneable interface.
                //        //If it does support the ICloneable interface, we use it to set the clone of
                //        //the object in the list.
                //        ((IList)salida)[j] = obj.GetType().GetInterface("ICloneable", true) != null
                //                ? ((ICloneable)obj).Clone()
                //                : obj.Copy();
                //        j++;
                //    }
                //}
                //else if (interfaces.Contains(typeof(IDictionary)))
                //{
                //    foreach (DictionaryEntry de in (IEnumerable)value)
                //    {
                //        //Checking to see if the item supports the ICloneable interface.
                //        ((IDictionary)salida)[de.Key] = de.Value.GetType().GetInterface("ICloneable", true) != null
                //                            ? ((ICloneable) de.Value).Clone()
                //                            : de.Value.Copy();
                //    }
                //}
                i++;
            }
            return newObject;
        }

        /// <summary>
        /// Indica si el objeto es de una clase serializable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">El objeto a evaluar</param>
        /// <returns></returns>
        /// 20/11/2012 by Sergi
        private static bool IsSerializable<T>(this T obj)
            where T : class
        {
            return (typeof(T).IsSerializable || (obj is ISerializable) || (Attribute.IsDefined(typeof(T), typeof(SerializableAttribute))));
        }

        #endregion
    }
}
