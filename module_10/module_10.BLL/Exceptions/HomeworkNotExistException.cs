using System;
using System.Runtime.Serialization;
using module_10.BLL.Exceptions.Abstract;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class HomeworkNotExistException : NotExistException
    {
        public HomeworkNotExistException()
        {
        }

        protected HomeworkNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public HomeworkNotExistException(string message) : base(message)
        {
        }

        public HomeworkNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
