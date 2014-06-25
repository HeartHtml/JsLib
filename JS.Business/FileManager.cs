using System.IO;

namespace JS.Business
{
    /// <summary>
    /// Class that creates and converts byte arrays into files
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Writes a byte array into a file on disk.
        /// </summary>
        /// <param name="input">The byte array to write to disk.</param>
        /// <param name="fileUrl">Location of the file to create.</param>
        /// <returns>True if the file is created without error and false otherwise.</returns>
        public static bool ConvertByteArrayToFile(byte[] input, string fileUrl)
        {
            // Validate the input.
            if (input == null || string.IsNullOrEmpty(fileUrl))
            {
                return false;
            }

            try
            {
                // Make sure the path to the file location exists, and create it if needed.
                if (fileUrl.Contains("\\") || fileUrl.Contains("/"))
                {
                    // Make sure all the path delimiters are the same character and trim off the
                    // file name to get the folder path.
                    string filePath = fileUrl.Replace('/', '\\');
                    int index = filePath.LastIndexOf('\\');
                    filePath = filePath.Substring(0, index);

                    // If the folder path doesn't exist, create it.
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                }

                // Create the requested file.
                using (FileStream file = new FileStream(fileUrl, FileMode.Create, FileAccess.Write))
                {
                    // Write the byte array to the file.
                    using (BinaryWriter writer = new BinaryWriter(file))
                    {
                        writer.Write(input);
                        writer.Flush();
                        writer.Close();
                    }
                    file.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts the file pointed to by fileUrl into a byte array.
        /// </summary>
        /// <param name="fileUrl">URL of the file to be converted into a byte array.</param>
        /// <returns>A byte array representing the requested file.  An error returns an empty array.</returns>
        public static byte[] ConvertFileToByteArray(string fileUrl)
        {
            try
            {
                return File.Exists(fileUrl) ? File.ReadAllBytes(fileUrl) : new byte[0];
            }
            catch
            {
                return new byte[0];
            }
        }
    }
}
