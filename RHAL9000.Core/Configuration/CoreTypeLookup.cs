using System;

namespace RHAL9000.Core.Configuration
{
    public class CoreTypeLookup : TypeLookup
    {
        public CoreTypeLookup()
        {
            Register<Uri>("Uri");
            Register<string>("Username");
            Register<string>("Password");
        }

    }
}