using System.IO;

namespace GES.Inside.Data.Services.Interfaces
{
    /// <summary>
    /// Define the service's interface that hold and control the sharing file system.
    /// </summary>
    public interface ISharingFileSystemService
    {
        /// <summary>
        /// Method to download file from sharing disk system with specific path.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="safeRead">
        /// Parameter check need safe execute.
        /// True - Need open connection before read & close after read complete
        /// False - Not need to control the connection
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exceptions.FileSharingException">FileSharingException</exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        Stream ReadFile(string fileName, bool safeRead = true);

        /// <summary>
        /// Open the connection to remote file sharing system and execute action.
        /// After that the client will close connection
        /// </summary>
        /// <param name="action">Action will be need remote connection</param>
        /// <exception cref="Exceptions.FileSharingException">FileSharingException</exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        void SafeExecute(System.Action action);

        /// <summary>
        /// Open the connection to remote file sharing system and execute action.
        /// After that the client will close connection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <exception cref="Exceptions.FileSharingException">FileSharingException</exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        T SafeExecute<T>(System.Func<T> function);
    }
}
