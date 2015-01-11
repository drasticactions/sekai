using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel;
using Autofac;
using Sekai.Common;
using Sekai.Properties;
using Sekai.ViewModels;

namespace Sekai.Locator
{
    public class ViewModels
    {
        public ViewModels()
        {
            if (DesignMode.DesignModeEnabled)
            {
                App.Container = AutoFacConfiguration.Configure();
            }
        }

        public static MainPageViewModel MainPageVm => App.Container.Resolve<MainPageViewModel>();

        public static MessagePageViewModel MessagePageVm => App.Container.Resolve<MessagePageViewModel>();

        public static SettingsPageViewModel SettingPageVm => App.Container.Resolve<SettingsPageViewModel>();
    }
}
