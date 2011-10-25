using System.Xml.Linq;

namespace RHAL9000.Core
{
    public interface IXmlAccessor
    {
        string BaseUrl { get; }
        XElement GetXml(string path);
        XElement GetXml();
    }
}