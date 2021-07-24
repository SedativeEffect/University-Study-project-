using System;
using System.Runtime.Serialization;
using module_10.BLL.Exceptions.Abstract;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class HomeworkAlreadyExistException : AlreadyExistException
    {
        public HomeworkAlreadyExistException()
        {
        }

        public HomeworkAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public HomeworkAlreadyExistException(string message) : base(message)
        {
        }

        public HomeworkAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
