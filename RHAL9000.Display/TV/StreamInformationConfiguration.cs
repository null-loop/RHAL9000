using System;

namespace RHAL9000.Display.TV
{
    public class StreamInformationConfiguration
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public TimeSpan LifeTime { get; set; }
    }
}