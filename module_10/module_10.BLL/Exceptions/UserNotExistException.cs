using System;
using System.Runtime.Serialization;
using module_10.BLL.Exceptions.Abstract;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class UserNotExistException : NotExistException
    {
        public UserNotExistException()
        {
        }

        protected UserNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UserNotExistException(string message) : base(message)
        {
        }

        public UserNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
