using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Sekai.ViewModels;

namespace Sekai.Common
{
    public class AutoFacConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // Register View Models
            builder.RegisterType<AccountViewModel>().SingleInstance();
            builder.RegisterType<FeedPageViewModel>().SingleInstance();
            builder.RegisterType<SettingsPageViewModel>().SingleInstance();
            builder.RegisterType<MediaViewModel>().SingleInstance();
            builder.RegisterType<MainPage>();
            return builder.Build();
        }
    }
}
