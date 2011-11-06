using System.Xml.Linq;

namespace RHAL9000.Core.Configuration
{
    public interface ITypeBuilder
    {
        object Build(XElement element);
        T Build<T>(XElement element) where T : class;
    }
}