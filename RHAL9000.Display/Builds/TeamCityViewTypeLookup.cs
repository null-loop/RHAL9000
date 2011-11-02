using RHAL9000.Core;
using RHAL9000.Core.Configuration;

namespace RHAL9000.Display.Builds
{
    public class TeamCityViewTypeLookup : TypeLookup
    {
        public TeamCityViewTypeLookup()
        {
            Register<TeamCityBuildsViewModel>("TeamCityBuilds");
        }
    }
}