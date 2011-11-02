using System;
using System.Xml.Linq;

namespace RHAL9000.Core
{
    public interface IXmlAccessor
    {
        Uri BaseUrl { get; }
        XElement GetXml(string path);
        XElement GetXml();
    }
}