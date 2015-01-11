using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using LinqToTwitter;
using System.ComponentModel;
using System.Linq;

namespace Sekai.ScrollingCollection
{
    public class TimelineScrollingCollection : ObservableCollection<Status>, ISupportIncrementalLoading
    {
        public StatusType StatusType { get; set; }
        private ulong _maxId;
        private ulong _sinceId;
        private bool _isLoading;

        public TimelineScrollingCollection()
        {
            HasMoreItems = true;
            IsLoading = false;
        }
        public bool IsLoading
        {
            get { return _isLoading; }

            private set
            {
                _isLoading = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLoading"));
            }
        }
        private int _cursor;
        public int Cursor
        {
            get { return _cursor; }

            private set
            {
                _cursor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Cursor"));
            }
        }
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            IsLoading = true;
            List<Status> tweets;
            if (_maxId == 0)
            {
                tweets = await GetFreshStatus();
            }
            else
            {
                tweets = await GetMaxIdStatus();
            }
            foreach (var tweet in tweets)
            {
                Add(tweet);
            }
            IsLoading = false;
            return new LoadMoreItemsResult { Count = count };
        }

        private async Task<List<Status>> GetMaxIdStatus()
        {
            var tweets =
               await
               (from tweet in Locator.ViewModels.MainPageVm.TwitterContext.Status
                where tweet.Type == StatusType && tweet.MaxID == _maxId
                select tweet)
               .ToListAsync();
            if (tweets.Any())
            {
                _maxId = tweets.Last().StatusID;
            }
            return tweets;
        }

        private async Task<List<Status>> GetFreshStatus()
        {
            var tweets =
               await
               (from tweet in Locator.ViewModels.MainPageVm.TwitterContext.Status
                where tweet.Type == StatusType
                select tweet)
               .ToListAsync();
            if (tweets.Any())
            {
                _maxId = tweets.Last().StatusID;
            }
            return tweets;
        }

        public async Task RefreshTweets()
        {
            if (!this.Any())
            {
                return;
            }
            IsLoading = true;
            var tweets =
               await
               (from tweet in Locator.ViewModels.MainPageVm.TwitterContext.Status
                where tweet.Type == StatusType && tweet.SinceID == this.First().StatusID
                select tweet)
               .ToListAsync();
            if (!tweets.Any())
            {
                IsLoading = false;
                return;
            }
            tweets.Reverse();
            foreach (var tweet in tweets)
            {
               Insert(0, tweet);
            }
            IsLoading = false;
        }
        public bool HasMoreItems { get; private set; }
    }
}
