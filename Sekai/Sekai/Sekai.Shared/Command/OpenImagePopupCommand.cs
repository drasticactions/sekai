using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Sekai.Common;
using Sekai.Controls;
using Sekai.Tools;
using Sekai.Views;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Sekai.Command
{
    public class OpenImagePopupCommand : AlwaysExecutableCommand
    {
        public override async void Execute(object parameter)
        {
            var item = (ItemClickEventArgs) parameter;
            var tweet = (ITweet) item.ClickedItem;
            var mediaEntities = tweet.IsRetweet ? tweet.RetweetedTweet.Media : tweet.Media;
            if (!mediaEntities.Any())
            {
                return;
            }
            Locator.ViewModels.MediaPageVm.InitializeFlipView(mediaEntities, tweet);
            App.RootFrame.Navigate(typeof (ImageViewerPage));
        }
    }
}
