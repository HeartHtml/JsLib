using System;
using System.Configuration;

namespace JS.Data
{
    /// <summary>
    /// Base DAO
    /// </summary>
    [Serializable]
    public abstract class BaseDao
    {
        /// <summary>
        /// Connection for Reporting mirror of the accounts database
        /// </summary>
        protected static string JSConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["JSConnectionString"].ConnectionString;
            }
        }

    }
}
