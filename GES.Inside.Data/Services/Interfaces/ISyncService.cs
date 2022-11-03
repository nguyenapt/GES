using GES.Common.Exceptions;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface ISyncService
    {
        /// <summary>
        /// Synchronize the data.
        /// </summary>
        /// <exception cref="GesServiceException">Service that have exception</exception>
        /// <exception cref="Exceptions.SyncServiceException">Sync that have exception</exception>
        void Sync();
    }
}
