using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using JS.Entities.ExtensionMethods;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.ServiceLocation;

namespace JS.Business.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventViewerManager
    {
        private static string ConfigurationFileTemplateName
        {
            get
            {
                return "EnterpriseLibraryLoggingTemplate.config";
            }
        }

        private static string ConfigurationFileName
        {
            get
            {
                return "EnterpriseLibraryLogging.config";
            }
        }

        private static string ApplicationNameToken
        {
            get
            {
                return "@@ApplicationName";
            }
        }

        /// <summary>
        /// Logs a message to the Event Log on the client machine
        /// </summary>
        /// <param name="message"></param>
        public static void LogMessage(string message)
        {
            string callingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;

            string callingAssemblyPath = string.Empty;

            if (HttpContext.Current == null)
            {
                callingAssemblyPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            }
            else
            {
                string physicalApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                if (!string.IsNullOrWhiteSpace(physicalApplicationPath))
                {
                    callingAssemblyPath = Path.Combine(physicalApplicationPath, "bin");
                }
            }

            LogMessageForAssembly(message, callingAssemblyPath, callingAssemblyName);
        }

        private static void LogMessageForAssembly(string message, string callingAssemblyPath, string callingAssemblyName)
        {
            string configurationFilePath = GetConfigurationFilePath(callingAssemblyPath, callingAssemblyName);

            if (!configurationFilePath.IsNullOrWhiteSpace())
            {
                FileConfigurationSource config = new FileConfigurationSource(configurationFilePath);

                IServiceLocator container = EnterpriseLibraryContainer.CreateDefaultContainer(config);

                LogWriter logWriter = container.GetInstance<LogWriter>();

                LogEntry entry = new LogEntry
                {
                    Priority = 1,
                    Message = message,
                    Severity = TraceEventType.Information
                };

                logWriter.Write(entry);
            }
        }

        private static string GetConfigurationFilePath(string callingAssemblyPath, string callingAssemblyName, bool replaceExistingFile = false)
        {
            string configurationFilePath = Path.Combine(callingAssemblyPath, ConfigurationFileName);

            if (!replaceExistingFile && File.Exists(configurationFilePath))
            {
                return configurationFilePath;
            }

            string templateFilePath = Path.Combine(callingAssemblyPath, ConfigurationFileTemplateName);

            if (!templateFilePath.IsNullOrWhiteSpace() && File.Exists(templateFilePath))
            {
                using (StreamReader reader = new StreamReader(templateFilePath))
                {
                    string allLines = reader.ReadToEnd();

                    allLines = allLines.SafeReplace(ApplicationNameToken, callingAssemblyName);

                    File.WriteAllText(configurationFilePath, allLines);

                    return configurationFilePath;
                }
            }

            return string.Empty;
        }
    }
}
