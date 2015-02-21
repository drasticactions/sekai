using System;
using System.Collections.Generic;
using System.Text;
using Sekai.Common;
using Sekai.Views;

namespace Sekai.Command
{
    public class NavigateToSettingsCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
#if WINDOWS_PHONE_APP
            App.RootFrame.Navigate(typeof(SettingsPage));
            return;
#endif
        }
    }
}
