using System.IO;
using System.Linq;
using System.Collections.Generic;
using Caliburn.Micro;
using RHAL9000.Core;

namespace RHAL9000.Display 
{
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell, ICalScreen
    {

        public ShellViewModel()
        {
            DisplayName = "HAL9001";

            var config = XmlConfiguration.LoadConfigurationFromFile(new FileInfo("RHAL9000.Views.Configuration.xml")).ToArray();

            CreateViews(config, this);

            //Items.Add(new ExampleChartViewModel());
        }

        private static void CreateViews(IEnumerable<ViewConfiguration> config, ICalScreen target)
        {
            foreach (var viewConfig in config)
            {
                var view = Ioc.Container.GetInstance<ICalScreen>(viewConfig.Name);

                var poller = view as IPollingViewModel;

                if (poller != null) view = new PollingContainerViewModel(poller);

                view.Configure(viewConfig.Parameters);

                if (viewConfig.SubViews!=null && viewConfig.SubViews.Count() > 0)
                {
                    CreateViews(viewConfig.SubViews, view);
                }

                target.AddItem(view);
            }
        }

        public void Configure(Dictionary<string, string> dictionary)
        {
        }

        public void AddItem(object item)
        {
            var s = item as IScreen;
            if (s != null) Items.Add(s);
        }
    }
}
