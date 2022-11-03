using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Common.Exceptions
{
    /// <summary>
    /// Define the exception that is thrown by the Data Access layer
    /// </summary>
    public class GesDbException : Exception
    {
        public GesDbException() : base()
        {
        }

        public GesDbException(string message) : base(message)
        {
        }

        public GesDbException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GesDbException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
