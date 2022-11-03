using System.IO;
using System;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IGesFileStorageService 
    {
        Stream GetStream(Guid documentId);
        Stream GetStream(long orgId, Guid documentId);
        Stream GetStreamFromOldSystem(string fileName);

        string StoreFile(Stream fileStream, string fileName, string hashCodeDocument);
        bool StoreFile(Stream fileStream, string fileName);
        Stream GetFileStream(string fileName);
        string GetFilePathFromOldSystem(string fileName);
        string GetExportFilePath(string fileName);
        string GetFilePath(string fileName, string hashCodeDocument);

    }
}
