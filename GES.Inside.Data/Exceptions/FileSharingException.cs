using GES.Common.Exceptions;
using System;

namespace GES.Inside.Data.Exceptions
{
    public sealed class FileSharingException : GesServiceException
    {
        public FileSharingException(string message, Exception inner) : base(message, inner) { }

        public FileSharingException() : base()
        {
        }

        public FileSharingException(string message) : base(message)
        {
        }

        private FileSharingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
