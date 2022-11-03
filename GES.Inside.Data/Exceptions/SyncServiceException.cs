using GES.Common.Exceptions;
using System;

namespace GES.Inside.Data.Exceptions
{
    /// <summary>
    /// Define the exception class that will be thrown when the synchronization have exception
    /// </summary>
    public sealed class SyncServiceException : GesServiceException
    {
        public SyncServiceException(string message, Exception inner) : base(message, inner)
        {
        }

        public SyncServiceException() : base()
        {
        }

        public SyncServiceException(string message) : base(message)
        {
        }

        private SyncServiceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
