using System;
using System.Net;
using System.Runtime.Serialization;
using module_10.BLL.Exceptions.Abstract;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class UserAlreadyExistException : AlreadyExistException
    {
        public UserAlreadyExistException()
        {
        }

        public UserAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UserAlreadyExistException(string message) : base(message)
        {
        }

        public UserAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
