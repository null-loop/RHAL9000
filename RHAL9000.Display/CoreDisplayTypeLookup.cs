using RHAL9000.Core.Configuration;

namespace RHAL9000.Display
{
    public class CoreDisplayTypeLookup : TypeLookup
    {
        public CoreDisplayTypeLookup()
        {
            Register<ApplicationCoreViewModel>("ApplicationCore");
        }
    }
}
