using System;
using System.Globalization;
using System.IO;
using JS.Entities.ExtensionMethods;

namespace JS.Business
{
    /// <summary>
    /// Encapsulates Download Logic
    /// </summary>
    public static class DownloadManager
    {
        /// <summary>
        /// Content Disposition header string
        /// </summary>
        public static string ContentDispositon
        {
            get { return "Content-Disposition"; }
        }

        /// <summary>
        /// Content Length header string
        /// </summary>
        public static string ContentLength
        {
            get { return "Content-Length"; }
        }

        /// <summary>
        /// Get MIME Type from FileExtension
        /// </summary>
        /// <param name="fileExtension" />
        /// <returns>The MIME Type</returns>
        public static string GetMIMEType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";

                case ".txt":
                case ".sql":
                    return "text/plain";

                case ".doc":
                case ".docx":
                    return "application/ms-word";

                case ".tiff":
                case ".tif":
                    return "image/tiff";

                case ".asf":
                    return "video/x-ms-asf";

                case ".avi":
                    return "video/avi";

                case ".zip":
                    return "application/zip";

                case ".xls":
                case ".xlsx":
                case ".csv":
                    return "application/vnd.ms-excel";

                case ".gif":
                    return "image/gif";

                case ".jpg":
                case "jpeg":
                    return "image/jpeg";

                case ".bmp":
                    return "image/bmp";

                case ".wav":
                    return "audio/wav";

                case ".mp3":
                    return "audio/mpeg3";

                case ".mpg":
                case "mpeg":
                    return "video/mpeg";

                case ".rtf":
                    return "application/rtf";

                case ".asp":
                    return "text/asp";

                case ".pdf":
                    return "application/pdf";

                case ".fdf":
                    return "application/vnd.fdf";

                case ".ppt":
                case ".pptx":
                    return "application/mspowerpoint";

                case ".dwg":
                    return "image/vnd.dwg";

                case ".msg":
                    return "application/msoutlook";

                case ".xml":
                case ".sdxl":
                    return "application/xml";

                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";

                default:
                    return "application/octet-stream";
            }
        }

        /// <summary>
        /// Gets the Content Disposition
        /// </summary>
        /// <param name="file" />
        /// <returns>The content disposition for a file</returns>
        public static string GetContentDisposition(FileInfo file)
        {
            return GetContentDisposition(file.Name.RemoveInvalidCharacters());
        }

        /// <summary>
        /// Gets the Content Disposition
        /// </summary>
        /// <param name="fileName" />
        /// <returns>The content disposition for a file</returns>
        public static string GetContentDisposition(string fileName)
        {
            return string.Format("attachment; filename={0}", fileName.RemoveInvalidCharacters());
        }

        /// <summary>
        /// Gets the Content Length
        /// </summary>
        /// <param name="file" />
        /// <returns>The content length for a file</returns>
        public static string GetContentLength(FileInfo file)
        {
            return file.Length.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the Content Length
        /// </summary>
        /// <param name="fileBytes" />
        /// <returns>The content length for a file</returns>
        public static string GetContentLength(byte[] fileBytes)
        {
            return fileBytes.Length.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a Unique-ified File Path for the Message attachment
        /// </summary>
        /// <param name="filePath" />
        /// <returns>A Unique file path to use</returns>
        public static string CreateUniqueFileName(string filePath)
        {
            string uniqueFilePath = filePath;
            string fileExtension = String.Empty;

            if (!uniqueFilePath.IsNullOrWhiteSpace())
            {
                int lastIndexOfPeriod = uniqueFilePath.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase);
                if (lastIndexOfPeriod > 0)
                {
                    fileExtension = uniqueFilePath.Substring(lastIndexOfPeriod + 1);
                    uniqueFilePath = uniqueFilePath.Remove(lastIndexOfPeriod);
                }

                uniqueFilePath = string.Format("{0}_{1}.{2}",
                                               uniqueFilePath.RemoveInvalidCharacters(),
                                               DateTime.Now.ToString("MMddyyyyHHmmssffff"),
                                               fileExtension);
            }

            return uniqueFilePath;
        }

        /// <summary>
        /// Exports a file to the response
        /// </summary>
        /// <param name="response">Response object from web page or web control</param>
        /// <param name="fileData">Raw data of file</param>
        /// <param name="fileName">Name of file</param>
        /// <param name="fileExtension">Extension of file for MIME purposes</param>
        public static void ExportFileToHttpResponse(System.Web.HttpResponse response,
                                                    byte[] fileData,
                                                    string fileName,
                                                    string fileExtension)
        {
            response.ClearContent();
            response.ContentType = GetMIMEType(fileExtension);
            response.AddHeader(ContentDispositon, GetContentDisposition(fileName.RemoveInvalidCharacters()));
            response.AddHeader(ContentLength, fileData.Length.ToString(CultureInfo.InvariantCulture));
            response.BinaryWrite(fileData);
            response.Flush();
        }
    }
}
