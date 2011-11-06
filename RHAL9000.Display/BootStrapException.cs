using System;
using System.Runtime.Serialization;

namespace RHAL9000.Display
{
    [Serializable]
    public class BootStrapException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public BootStrapException()
        {
        }

        public BootStrapException(string message) : base(message)
        {
        }

        public BootStrapException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BootStrapException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}