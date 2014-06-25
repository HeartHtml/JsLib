using System;
using System.Collections;
using System.Linq;

namespace JS.Entities
{
    /// <summary>
    /// Maps two different types.
    /// </summary>
    /// <typeparam name="T">Type that is mapped to.</typeparam>
    public static class TypeMapper<T> where T : new()
    {
        /// <summary>
        /// Maps the source to a return type.
        /// </summary>
        /// <param name="source">Soruce type that contains the data that will be mapped to the return type.</param>
        /// <returns>Returns the generic type with the source data.</returns>
        /// <remarks>
        /// TypeMapper performs a recursive deep copy of the source object into the type T object.
        /// The type T must have the same properties as the source type. 
        /// If there are any collecitons they must be of type ICollections (ex. generic version of List) and the collection must be a generic collection.
        /// </remarks>
        public static T Map(object source)
        {
            if (source == null)
            {
                return default(T);
            }

            var destination = Activator.CreateInstance<T>();

            return (T)Map(source, destination);
        }

        /// <summary>
        /// Maps the source to a destination object by reference
        /// </summary>
        /// <param name="source">Soruce type that contains the data that will be mapped to the return type.</param>
        /// <param name="destination"></param>
        /// <returns>Returns the generic type with the source data.</returns>
        /// <remarks>
        /// TypeMapper performs a recursive deep copy of the source object into the type T object.
        /// The type T must have the same properties as the source type. 
        /// If there are any collecitons they must be of type ICollections (ex. generic version of List) and the collection must be a generic collection.
        /// </remarks>
        public static void MapInPlace(object source, T destination)
        {
            if (source == null)
            {
                return;
            }

            Map(source, destination);
        }

        private static object Map(object source, object destination)
        {
            var destinationType = destination.GetType();

            var sourceType = source.GetType();

            var properties = sourceType.GetProperties();

            while (sourceType != null)
            {

                foreach (var property in properties)
                {
                    var sourcePropertyInfo = sourceType.GetProperty(property.Name);
                    var destinationPropertyInfo = destinationType.GetProperty(property.Name);

                    if (destinationPropertyInfo != null && destinationPropertyInfo.GetSetMethod(true) != null)
                    {
                        if (sourcePropertyInfo != null)
                        {
                            object sourcePropertyValue = sourcePropertyInfo.GetValue(source, null);
                            object destinationPropertyValue = destinationPropertyInfo.GetValue(destination, null);

                            if (sourcePropertyValue != null)
                            {
                                Type sourcePropertyValueType = sourcePropertyValue.GetType();

                                if (sourcePropertyValue is ICollection)
                                {
                                    var sourcePropertyList = sourcePropertyValue as ICollection;
                                    var destinationPropertyList = destinationPropertyValue as ICollection;

                                    if (destinationPropertyList != null)
                                    {
                                        Type destinationPropertyListType = destinationPropertyList.GetType();

                                        // Copy only works with generic collections
                                        if (sourcePropertyList.Count > 0 &&
                                            destinationPropertyListType.IsGenericType)
                                        {
                                            destinationPropertyList = Activator.CreateInstance(destinationPropertyListType) as ICollection;

                                            foreach (var sourceListValue in sourcePropertyList)
                                            {
                                                Type destinationPropertyListValueType = destinationPropertyListType.GetGenericArguments().FirstOrDefault();

                                                if (destinationPropertyListValueType != null)
                                                {
                                                    var destinationPropertyListValue = Activator.CreateInstance(destinationPropertyListValueType);
                                                    var mappeddestinationPropertyListValue = Map(sourceListValue, destinationPropertyListValue);
                                                    destinationPropertyListType.GetMethod("Add").Invoke(destinationPropertyList, new[] { mappeddestinationPropertyListValue });
                                                }
                                            }

                                            destinationPropertyInfo.SetValue(destination, destinationPropertyList, null);
                                        }
                                    }
                                }
                                else if (IsNestedCustomType(sourcePropertyValueType))
                                {
                                    var newDestinationValue = Activator.CreateInstance(destinationPropertyInfo.PropertyType);
                                    var destinationValue = Map(sourcePropertyValue, newDestinationValue);
                                    destinationPropertyInfo.SetValue(destination, destinationValue, null);
                                }
                                else
                                {
                                    destinationPropertyInfo.SetValue(destination, sourcePropertyValue, null);
                                }
                            }
                        }
                    }
                }

                sourceType = sourceType.BaseType;
            }

            return destination;
        }

        private static bool IsNestedCustomType(Type sourceValueType)
        {
            var result = false;

            try
            {
                var properties = sourceValueType.GetProperties();

                if (sourceValueType.Namespace != null &&
                    !sourceValueType.Namespace.StartsWith("System") &&
                    properties.Length > 0)
                {
                    result = true;
                }
            }
            catch { }

            return result;
        }
    }
}