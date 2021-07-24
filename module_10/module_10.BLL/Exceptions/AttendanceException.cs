using System;
using System.Net;
using System.Runtime.Serialization;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class AttendanceException : Exception
    {
        public const int StatusCode = (int)HttpStatusCode.BadRequest;
        public AttendanceException()
        {
        }

        protected AttendanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AttendanceException(string message) : base(message)
        {
        }

        public AttendanceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
