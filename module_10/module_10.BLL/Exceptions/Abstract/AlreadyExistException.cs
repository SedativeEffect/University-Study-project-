using System;
using System.Net;
using System.Runtime.Serialization;

namespace module_10.BLL.Exceptions.Abstract
{
    [Serializable]
    public abstract class AlreadyExistException : Exception
    {
        public const int StatusCode = (int)HttpStatusCode.BadRequest;

        protected AlreadyExistException()
        {
        }

        protected AlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected AlreadyExistException(string message) : base(message)
        {
        }

        protected AlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
