using System;
using System.Net;
using System.Runtime.Serialization;

namespace module_10.BLL.Exceptions.Abstract
{
    [Serializable]
    public abstract class LectorException : Exception
    {
        public const int StatusCode = (int)HttpStatusCode.BadRequest;
        protected LectorException()
        {
        }

        protected LectorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected LectorException(string message) : base(message)
        {
        }

        protected LectorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
