using GES.Common.Helpers;
using GES.Common.Models;
using GES.Common.Services.Interface;
using GES.Inside.Data.Exceptions;
using GES.Inside.Data.Services.Interfaces;
using Renci.SshNet;
using System;
using System.IO;

namespace GES.Inside.Data.Services
{
    public class SFTPFileSystemService : ISharingFileSystemService, IDisposable
    {
        private readonly IApplicationSettingsService _appSettingService;
        private readonly SftpClient _client;
        private bool disposing;

        public SFTPFileSystemService(IApplicationSettingsService applicationSettingService)
        {
            this._appSettingService = applicationSettingService;
            this._client = this.CreateClient();
        }

        public void Dispose()
        {
            if(_client != null && !disposing)
            {
                disposing = true;

                this.Disconnect();

                this._client.Dispose();
            }
        }

        public Stream ReadFile(string fileName, bool safeRead = true)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName), "File path cannot be null!");

            MemoryStream writeStream = null;

            try
            {
                return safeRead ? this.SafeExecute<Stream>(() => WriteFileToStream(writeStream, fileName)) : WriteFileToStream(writeStream, fileName);
            }
            catch (FileSharingException)
            {
                HandleFileStreamError(writeStream);

                throw;
            }
            catch (Exception ex)
            {
                HandleFileStreamError(writeStream);

                throw new FileSharingException("Can not download the file via SFTP.", ex);
            }
        }

        public void SafeExecute(Action action)
        {
            Guard.EnsureArgumentNotNull(action, "The action cannot be null.");

            try
            {
                this.Connect();

                action();
            }
            catch(FileSharingException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FileSharingException("Can not download the file via SFTP.", ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        public T SafeExecute<T>(Func<T> function)
        {
            Guard.EnsureArgumentNotNull(function, "The function cannot be null.");

            try
            {
                this.Connect();

                return function();
            }
            catch (FileSharingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FileSharingException("Can not download the file via SFTP.", ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        private void HandleFileStreamError(MemoryStream stream)
        {
            if (stream != null && (stream.CanWrite || stream.CanRead))
            {
                // Close stream
                stream.Close();
            }
        }

        private void Connect()
        {
            if (_client != null && !_client.IsConnected)
                _client.Connect();
        }

        private void Disconnect()
        {
            if (_client != null && _client.IsConnected)
                _client.Disconnect();
        }

        private SftpClient CreateClient()
        {
            try
            {
                var authorizationInfo = this._appSettingService.GetSetting<SftpConnectionInfo>();

                if (authorizationInfo == null)
                    return null;

                var connectionInfo = new ConnectionInfo(authorizationInfo.Host, int.Parse(authorizationInfo.Port), authorizationInfo.UserName,
                    new PasswordAuthenticationMethod(authorizationInfo.UserName, authorizationInfo.Password));

                return new SftpClient(connectionInfo);
            }
            catch(Exception ex)
            {
                throw new FileSharingException("Can not create SFTP client.", ex);
            }
        }

        private Stream WriteFileToStream(Stream writeStream, string filePath)
        {
            writeStream = new MemoryStream();
            _client.DownloadFile(filePath, writeStream);

            // Seek to the beginning to read.
            writeStream.Position = 0;

            return writeStream;
        }
    }
}
