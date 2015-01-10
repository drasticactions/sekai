using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Sekai.ViewModels;
using Sekai.Views;

namespace Sekai.Common
{
    public class AutoFacConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // Register View Models
            builder.RegisterType<MainPageViewModel>().SingleInstance();

            builder.RegisterType<MainPage>();
            return builder.Build();
        }
    }
}
