using System;
using System.Runtime.Serialization;

namespace Drive.Identity.Services
{
    [Serializable]
    internal class AuthRequestException : Exception
    {
        public AuthRequestException()
        {
        }

        public AuthRequestException(string message) : base(message)
        {
        }

        public AuthRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AuthRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}