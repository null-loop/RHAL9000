using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RHAL9000.Core
{
    //TODO:Refactor this out. 
    public class XmlConfiguration
    {
        public static IEnumerable<ViewConfiguration> LoadConfigurationFromFile(FileInfo configurationFile)
        {
            if (!configurationFile.Exists) throw new FileNotFoundException("Cannot find configuration file", configurationFile.FullName);

            using (var reader = new StreamReader(configurationFile.OpenRead()))
            {
                return LoadFromElement(XDocument.Load(reader).Root);
            }
        }

        private static IEnumerable<ViewConfiguration> LoadFromElement(XElement element)
        {
            return element.Elements("View").Select(viewElement => new ViewConfiguration()
                                                                      {
                                                                          Name = viewElement.Attribute("name").Value,
                                                                          Parameters = viewElement.Elements().Where(e => e.Name.LocalName != "View").ToDictionary(e => e.Name.LocalName, e => e.Value),
                                                                          SubViews = viewElement.Elements("View").Count() > 0 ? LoadFromElement(viewElement) : null
                                                                      });
        }
    }
}
