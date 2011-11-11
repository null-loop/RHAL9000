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
using RHAL9000.Monitors.Builds;

namespace RHAL9000.Display
{
    public class AppBootstrapper : Bootstrapper<IShell>
	{
	    private string _buildMonitorNamespace;
        private IApplicationCore _core;
        private IPollingMonitor _pollingMonitor;

        protected override void Configure()
	    {
	        // initialise our ApplicationCore
	        var configFile = new FileInfo("RHAL9000.config");

	        if (!configFile.Exists)
	            throw new BootStrapException("Cannot find config file RHAL9000.config");

            try
            {
                //TODO:Why is this still here?
                var typeLookups = new ITypeLookup[] {new CoreTypeLookup(), new TeamCityDataTypeLookup(), new CoreDisplayTypeLookup()};

                var typeBuilder = new DefaultTypeBuilder(typeLookups);

                var configDocument = XDocument.Load(configFile.FullName);

                _core = ApplicationCoreFactory.CreateCore(typeBuilder, configDocument);

                CaliburnSetup();
                Ioc.RegisterFromCore(_core, new ApplicationCoreViewModel(_core));

                foreach (var requiresPolling in _core.DataSources.Cast<IRequiresPolling>())
                {
                    Ioc.Container.GetInstance<IPollingMonitor>().Run(requiresPolling);
                }
            }
            catch (Exception ex)
            {
                throw new BootStrapException("Exception boot strapping", ex);
            }
	    }

        protected override void OnExit(object sender, EventArgs e)
        {
            // shutdown monitors...
            foreach (var polling in _core.DataSources.Cast<IRequiresPolling>())
            {
                Ioc.Container.GetInstance<IPollingMonitor>().Stop(polling);
            }

            base.OnExit(sender, e);
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
