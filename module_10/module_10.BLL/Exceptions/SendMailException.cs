using System;
using System.Net;
using System.Runtime.Serialization;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class SendMailException : Exception
    {
        public const int StatusCode = (int)HttpStatusCode.InternalServerError;
        public SendMailException()
        {
        }

        protected SendMailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SendMailException(string message) : base(message)
        {
        }

        public SendMailException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
