using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace GES.Common.Helpers
{
    public static class UtilHelper
    {
        public static string CreateSafeNameNoSpace(string str)
        {
            var rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(str, "").Replace(" ", "-");
        }

        public static string CreateSafeFileName(string fileNameWithoutExtension, string fileExtension)
        {
            return string.Format(CreateSafeNameNoSpace(fileNameWithoutExtension) + "_{0}" + fileExtension, DateTime.UtcNow.ToString(Configurations.Configurations.DateTimeFormat));
        }

        public static bool IsEmailAddress(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            var pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            if (Regex.IsMatch(email, pattern))
            {
                return true;
            }

            return false;
        }

        public static string GetFileExtension(string fileName)
        {
            Exceptions.Guard.AgainstNullArgument(nameof(fileName), fileName);

            var result = "";

            var lastIndexofDot = fileName.LastIndexOf(".", StringComparison.Ordinal);
            if (lastIndexofDot > 0)
            {
                result = fileName.Substring(lastIndexofDot);
            }
            return result.Replace(".", "").ToLower();
        }
        
        public static byte[] ImageToByte(string fileName)
        {
            if (!File.Exists(fileName) || (new FileInfo(fileName).Length <=0) ) return null;
            
            var img = Image.FromFile(fileName);
            var converter = new ImageConverter();
            
            return (byte[])converter.ConvertTo(img, typeof(byte[]));

        }
        
        public static void CopyMultiple(string sourceFilePath, params string[] destinationPaths)
        {
            if (string.IsNullOrEmpty(sourceFilePath)) throw new ArgumentException("A source file must be specified.", nameof(sourceFilePath));

            if (destinationPaths == null || destinationPaths.Length == 0) throw new ArgumentException("At least one destination file must be specified.", nameof(destinationPaths));

            Parallel.ForEach(destinationPaths, new ParallelOptions(),
                destinationPath =>
                {
                    using (var source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var destination = new FileStream(destinationPath, FileMode.Create))
                    {
                        var buffer = new byte[1024];
                        int read;

                        while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            destination.Write(buffer, 0, read);
                        }
                    }

                });
        }
        
        public static string GetFilePath(string configFolderPath, string fileName, string hashCodeDocument)
        {
            var subPath = configFolderPath;
            
            if (!string.IsNullOrEmpty(hashCodeDocument))
            {
                var subfixPath = string.Join("/", hashCodeDocument.ToCharArray());
                subPath = Path.Combine(configFolderPath, subfixPath);
            }

            var exists = Directory.Exists(subPath);

            if (!exists)
                Directory.CreateDirectory(subPath);

            var path = Path.Combine(subPath, fileName);
            return path;
        }
    }

    public static class Guard
    {
        /// <summary>
        /// Check the argument is not null
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="message"></param>
        /// <exception cref="System.ArgumentNullException">Throw when argument is null.</exception>
        public static void EnsureArgumentNotNull(object argument, string message)
        {
            if (argument == null)
                throw new ArgumentNullException(message);
        }
    }
}
