using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RHAL9000.Core;

namespace RHAL9000.Display
{
	public class AppBootstrapper : Bootstrapper<IShell>
	{
	    private string _buildMonitorNamespace;

	    protected override void Configure()
		{
           FrameworkElement.LanguageProperty.OverrideMetadata(

                            typeof(FrameworkElement),

                            new FrameworkPropertyMetadata(

                                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            Ioc.Configure();
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
