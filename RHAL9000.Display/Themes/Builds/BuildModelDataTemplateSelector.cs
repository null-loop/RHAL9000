using System.Windows;
using System.Windows.Controls;
using RHAL9000.Monitors.Builds;

namespace RHAL9000.Display.Themes.Builds
{
    public class BuildModelDataTemplateSelector : DataTemplateSelector 
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var buildModel = item as BuildModel;
            var pres = container as ContentPresenter;
			
            DataTemplate template = null;

            if (buildModel==null)
            {
                return pres.FindResource("BuildModelSuccessDataTemplate") as DataTemplate;
            }

            if (buildModel.IsRunning)
            {
                template = pres.FindResource("BuildModelRunningDataTemplate") as DataTemplate;
            }
            else
            {
                switch (buildModel.Status)
                {
                    case BuildStatus.Error :
                        template = pres.FindResource("BuildModelFailureDataTemplate") as DataTemplate;
                        break;
                    case BuildStatus.Success :
                        template = pres.FindResource("BuildModelSuccessDataTemplate") as DataTemplate;
                        break;
                    case BuildStatus.Failure :
                        template = pres.FindResource("BuildModelFailureDataTemplate") as DataTemplate;
                        break;
                    default : 
                        template = pres.FindResource("BuildModelUnknownStatusDataTemplate") as DataTemplate;
                        break;
                }
            }

            return template;
        }
    }
}
