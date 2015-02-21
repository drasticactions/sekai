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

        public static AccountViewModel AccountPageVm => App.Container.Resolve<AccountViewModel>();

        public static FeedPageViewModel FeedPageVm => App.Container.Resolve<FeedPageViewModel>();

        public static SettingsPageViewModel SettingPageVm => App.Container.Resolve<SettingsPageViewModel>();

        public static MediaViewModel MediaPageVm => App.Container.Resolve<MediaViewModel>();

        //public static MessagePageViewModel MessagePageVm => App.Container.Resolve<MessagePageViewModel>();

        //public static SettingsPageViewModel SettingPageVm => App.Container.Resolve<SettingsPageViewModel>();
    }
}
