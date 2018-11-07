using System;
using System.Runtime.Serialization;

namespace Eitiem
{
    [Serializable]
    internal class MalformedObjectData : Exception
    {
        public MalformedObjectData()
        {
        }

        public MalformedObjectData(string message) : base(message)
        {
        }

        public MalformedObjectData(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MalformedObjectData(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}