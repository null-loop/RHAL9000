using System;
using System.Linq;
using System.Xml.Linq;
using Caliburn.Micro;

namespace RHAL9000.Core.Configuration
{
    public static class ApplicationCoreFactory
    {
        public static IApplicationCore CreateCore(ITypeBuilder builder, XDocument configuration)
        {
            var root = configuration.Root;

            if (root==null) throw new InvalidOperationException("No root element");

            var dataSources = root.Element("DataSources").Elements().Select(builder.Build<IDataSource>);
            var outlooks = root.Element("Outlooks").Elements().Select(builder.Build<IScreen>);

            return new ApplicationCore(dataSources, outlooks);
        }
    }
}