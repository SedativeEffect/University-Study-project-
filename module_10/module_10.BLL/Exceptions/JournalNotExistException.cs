using System;
using System.Runtime.Serialization;
using module_10.BLL.Exceptions.Abstract;

namespace module_10.BLL.Exceptions
{
    [Serializable]
    public class JournalNotExistException : NotExistException
    {
        public JournalNotExistException()
        {
        }

        public JournalNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public JournalNotExistException(string message) : base(message)
        {
        }

        public JournalNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
