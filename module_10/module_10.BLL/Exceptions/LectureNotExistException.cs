using System;
using System.Runtime.Serialization;
using module_10.BLL.Exceptions.Abstract;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class LectureNotExistException : NotExistException
    {
        public LectureNotExistException()
        {
        }

        public LectureNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public LectureNotExistException(string message) : base(message)
        {
        }

        public LectureNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
