using System;
using System.Linq;

namespace JS.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlName : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string SqlColumn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public SqlName(string name)
        {
            SqlColumn = name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetSqlName(object t)
        {
            string value = string.Empty;
            Attribute[] attrs = GetCustomAttributes(t.GetType());  // Reflection.
            foreach (SqlName a in attrs.OfType<SqlName>())
            {
                value = a.SqlColumn;
            }
            return value;
        }
    }

}

