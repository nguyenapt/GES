using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GES.Common.Exceptions;
using GES.Common.Services.Interface;
using GES.Inside.Data.Services.Interfaces;

namespace GES.Inside.Data.Services
{
    public class GesFileStorageService : IGesFileStorageService
    {
        private readonly IGesDocumentService _documentService;
        private readonly IApplicationSettingsService _applicationSettingsService;

        public GesFileStorageService(IGesDocumentService documentService,
            IApplicationSettingsService applicationSettingsService)
        {
            _documentService = documentService;
            _applicationSettingsService = applicationSettingsService;
        }

        public Stream GetStream(Guid documentId)
        {
            var document = _documentService.GetGesDocumentById(documentId);
            var hashCode = document?.HashCodeDocument;
            var fileName = document?.FileName;

            return GetFileStream(hashCode, fileName, documentId.ToString());
        }

        public Stream GetStream(long orgId, Guid documentId)
        {
            var document = _documentService.GetDocumentById(orgId, documentId);
            var hashCode = document?.HashCodeDocument;
            var fileName = document?.FileName;

            return GetFileStream(hashCode, fileName, documentId.ToString());
        }

        public Stream GetStreamFromOldSystem(string fileName)
        {
            string filePath = GetFilePathFromOldSystem(fileName);

            if (filePath != null && File.Exists(filePath))
            {
                return File.Open(filePath, FileMode.Open);
            }
            throw new GesServiceException($"The file: {fileName} is not found in the storage location.");
        }

        private Stream GetFileStream(string hashCode, string fileName, string documentId)
        {
            string filePath = null;
            if (!string.IsNullOrEmpty(hashCode))
            {
                filePath = CreateFilePath(fileName, hashCode);
            }

            if (filePath != null && File.Exists(filePath))
            {
                return File.Open(filePath, FileMode.Open);
            }
            throw new GesServiceException($"The document with Id {documentId} is not found in the storage location.");
        }


        public Stream GetFileStream(string fileName)
        {
            string folderPath = _applicationSettingsService.ExportBlobPath;

            string filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                return File.Open(filePath, FileMode.Open);
            }
            throw new GesServiceException($"The document with Name {fileName} is not found in the storage location.");
        }

        public string StoreFile(Stream fileStream, string fileName, string hashCodeDocument)
        {
            try
            {
                // create file path
                var hasdCode = hashCodeDocument ?? MakeHashCodeDocument();
                var path = CreateFilePath(fileName, hasdCode);

                // store file
                if (fileStream != null)
                {
                    var fileStreamSave = new FileStream(path, FileMode.Create, FileAccess.Write);
                    fileStream.CopyTo(fileStreamSave);
                    fileStreamSave.Dispose();
                }
                // return result
                return hasdCode;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public bool StoreFile(Stream fileStream, string fileName)
        {
            try
            {
                string filePath = GetFilePathFromOldSystem(fileName);

                // store file
                if (fileStream != null)
                {
                    var fileStreamSave = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    fileStream.CopyTo(fileStreamSave);
                    fileStreamSave.Dispose();
                }
                // return result
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string CreateFilePath(string fileName, string hashCodeDocument)
        {
            var subfixPath = string.Join("/", hashCodeDocument.ToCharArray());

            string folderPath = _applicationSettingsService.BlobPath;

            string subPath = Path.Combine(folderPath, subfixPath);

            bool exists = Directory.Exists(subPath);

            if (!exists)
                Directory.CreateDirectory(subPath);

            var path = Path.Combine(subPath, fileName);
            return path;
        }

        public string GetFilePathFromOldSystem(string fileName)
        {
            string folderPath = _applicationSettingsService.FilePathOnOldSystem;

            var path = Path.Combine(folderPath, fileName);
            return path;
        }
        
        public string GetFilePath(string fileName, string hashCodeDocument)
        {
            return CreateFilePath(fileName, hashCodeDocument);
        }

        public string GetExportFilePath(string fileName)
        {
            string folderPath = _applicationSettingsService.ExportBlobPath;

            var path = Path.Combine(folderPath, fileName);
            return path;
        }

        private string MakeHashCodeDocument()
        {
            var hashfile = CreateMd5(Guid.NewGuid().ToString());
            var hashCodeDocument = hashfile.Substring(0, 6).ToLower();

            return hashCodeDocument;
        }


        public static string CreateMd5(string input)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }
}
