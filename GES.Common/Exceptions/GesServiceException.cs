using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Common.Exceptions
{
    /// <summary>
    /// Define the class that is thrown by the Service layer
    /// </summary>
    [Serializable]
    public class GesServiceException : Exception
    {
        public GesServiceException() : base()
        {
        }

        public GesServiceException(string message) : base(message)
        {
        }

        public GesServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GesServiceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
