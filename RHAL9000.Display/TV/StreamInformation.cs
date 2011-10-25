using System;

namespace RHAL9000.Display.TV
{
    public class StreamInformation
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public DateTime From { get; set; }
        public DateTime Until { get; set; }
    }

    //live1.wm.skynews.servecast.net/skynews_wmlz_live300k
}