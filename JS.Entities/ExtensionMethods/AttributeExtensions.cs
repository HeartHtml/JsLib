using System.Xml.Serialization;

namespace JS.Entities.ExtensionMethods
{
    /// <summary>
    /// Extension methods for class attributes
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        /// Gets a string value in the form of an xml start tag with the element name of the attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>A string</returns>
        public static string XmlStartTag(this XmlRootAttribute attribute)
        {
            return string.Format("<{0}>", attribute.ElementName);
        }

        /// <summary>
        /// Gets a string value in the form of an xml start tag with the element name of the attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>A string</returns>
        public static string XmlCloseTag(this XmlRootAttribute attribute)
        {
            return string.Format("</{0}>", attribute.ElementName);
        }
    }
}
