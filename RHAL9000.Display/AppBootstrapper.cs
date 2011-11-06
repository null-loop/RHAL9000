using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Caliburn.Micro;
using RHAL9000.Core;
using RHAL9000.Core.Configuration;
using RHAL9000.Testing.TDD;

namespace RHAL9000.Display
{
    public class AppBootstrapper : Bootstrapper<IApplicationCoreViewModel>
	{
	    private string _buildMonitorNamespace;

	    protected override void Configure()
	    {
	        // initialise our ApplicationCore
	        var configFile = new FileInfo("RHAL9000.config");

	        if (!configFile.Exists)
	            throw new BootStrapException("Cannot find config file RHAL9000.config");

            try
            {
                var typeLookups = new ITypeLookup[] {new CoreTypeLookup(), new TeamCityDataTypeLookup()};

                var typeBuilder = new DefaultTypeBuilder(typeLookups);

                var configDocument = XDocument.Load(configFile.FullName);

                var core = ApplicationCoreFactory.CreateCore(typeBuilder, configDocument);


                CaliburnSetup();
                Ioc.Configure(core, new ApplicationCoreViewModel(core));

                var pollingMonitor = Ioc.Container.GetInstance<IPollingMonitor>();

                foreach (var requiresPolling in core.DataSources.Cast<IRequiresPolling>())
                {
                    pollingMonitor.Run(requiresPolling);
                }
            }
            catch (Exception ex)
            {
                throw new BootStrapException("Exception boot strapping", ex);
            }
	    }

        private void CaliburnSetup()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(

                typeof(FrameworkElement),

                new FrameworkPropertyMetadata(

                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        protected override object GetInstance(Type serviceType, string key)
		{
		    return !string.IsNullOrEmpty(key)
		               ? Ioc.Container.GetInstance(serviceType, key)
		               : Ioc.Container.GetInstance(serviceType);
		}

	    protected override IEnumerable<object> GetAllInstances(Type serviceType)
	    {
	        return Ioc.Container.GetAllInstances(serviceType).Cast<object>();
		}

		protected override void BuildUp(object instance)
		{
            foreach (var property in instance.GetType().GetProperties().Where(p => p.CanWrite && p.PropertyType.IsPublic))
			{
                property.SetValue(instance, Ioc.Container.GetInstance(property.PropertyType), null);
			}
		}
	}
}
