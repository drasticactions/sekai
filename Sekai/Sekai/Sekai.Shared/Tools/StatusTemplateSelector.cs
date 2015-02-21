using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Tweetinvi.Core.Interfaces;

namespace Sekai.Tools
{
    public class StatusTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTimelineStatusTemplate { get; set; }
        public DataTemplate RetweetTimelineStatusTemplate { get; set; }
        public DataTemplate MentionTimelineStatusTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item,
            DependencyObject container)
        {
            var feedItem = item as ITweet;
            if (feedItem == null) return null;
            // Is retweeted status
            if (feedItem.IsRetweet)
            {
                return RetweetTimelineStatusTemplate;
            }
            return feedItem.Text.Contains(Locator.ViewModels.FeedPageVm.TwitterUser.ScreenName) ? MentionTimelineStatusTemplate : DefaultTimelineStatusTemplate;
        }
    }
}
