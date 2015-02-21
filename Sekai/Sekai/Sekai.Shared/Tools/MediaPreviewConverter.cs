using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Data;
using Tweetinvi.Core.Interfaces;

namespace Sekai.Tools
{
    public class MediaPreviewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var tweet = (ITweet)value;
            if (tweet.IsRetweet)
            {
                return !tweet.RetweetedTweet.Media.Any() ? string.Empty : tweet.RetweetedTweet.Media.First().MediaURL;
            }
            return !tweet.Media.Any() ? string.Empty : tweet.Media.First().MediaURL;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
