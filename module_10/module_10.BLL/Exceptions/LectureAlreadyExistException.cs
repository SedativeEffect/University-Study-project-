using System;
using System.Runtime.Serialization;
using module_10.BLL.Exceptions.Abstract;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class LectureAlreadyExistException : AlreadyExistException
    {
        public LectureAlreadyExistException()
        {
        }

        public LectureAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public LectureAlreadyExistException(string message) : base(message)
        {
        }

        public LectureAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
