using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Sekai.Common;
using Sekai.Database.Models;
using Sekai.Tools;
using Sekai.Views;

namespace Sekai.Command
{
    public class NavigateToFeedPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs;
            if (args == null)
            {
                SekaiDebugger.SendMessageDialogAsync("Navigation failed", new Exception("Arguments are null"));
                return;
            }

            var user = (TwitterUserAccount)args.ClickedItem;
            App.RootFrame.Navigate(typeof (FeedPage));
            await Locator.ViewModels.FeedPageVm.Initialize(user);
        }
    }
}
