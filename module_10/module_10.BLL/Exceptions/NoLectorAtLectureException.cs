using System;
using System.Runtime.Serialization;
using module_10.BLL.Exceptions.Abstract;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class NoLectorAtLectureException : LectorException
    {
        public NoLectorAtLectureException()
        {
        }

        public NoLectorAtLectureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NoLectorAtLectureException(string message) : base(message)
        {
        }

        public NoLectorAtLectureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
