using System;
using System.Runtime.Serialization;

namespace RHAL9000.Core
{
    [Serializable]
    public class MissingLookupException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MissingLookupException()
        {
        }

        public MissingLookupException(string message) : base(message)
        {
        }

        public MissingLookupException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingLookupException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}