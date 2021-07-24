using System;
using System.Net;
using System.Runtime.Serialization;

namespace module_10.BLL.Exceptions.Abstract
{
    [Serializable]
    public abstract class NotExistException : Exception
    {
        public const int StatusCode = (int)HttpStatusCode.NotFound;

        protected NotExistException()
        {
        }

        protected NotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected NotExistException(string message) : base(message)
        {
        }

        protected NotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
