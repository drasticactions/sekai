using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Sekai.Tools;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Logic.Model.Parameters;

namespace Sekai.ScrollingCollection
{
    public class TimelineScrollingCollection : ObservableCollection<ITweet>, ISupportIncrementalLoading
    {
        private long _maxId;
        private long _sinceId;
        private bool _isLoading;
        public string UserName { get; set; }
        public bool UserMentions { get; set; }
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
            try
            {
                await GetTweets();
            }
            catch (Exception ex)
            {
                SekaiDebugger.SendMessageDialogAsync("Failed to get tweets", "Twitter API Limit Hit!");
            }
            IsLoading = false;
            return new LoadMoreItemsResult { Count = count };
        }

        private async Task GetTweets()
        {
            if (_maxId == 0)
            {
                IEnumerable<ITweet> tweets;
                if (UserMentions)
                {
                    tweets = await GetFreshMentions();
                }
                else
                {
                    tweets = await GetFreshStatus();
                }
                foreach (var tweet in tweets)
                {
                    Add(tweet);
                }
            }
            else
            {
                IEnumerable<ITweet> tweets;
                if (UserMentions)
                {
                    tweets = await GetMaxIdMentions();
                }
                else
                {
                    tweets = await GetMaxIdStatus();
                }
                foreach (var tweet in tweets)
                {
                    Add(tweet);
                }
            }
        }

        private async Task<IEnumerable<ITweet>> GetFreshMentions()
        {
            var tweets = await TimelineAsync.GetMentionsTimeline();
            if (tweets.Any())
            {
                _maxId = tweets.Last().Id;
            }
            return tweets;
        }

        private async Task<IEnumerable<ITweet>> GetFreshStatus()
        {
            var tweets = await TimelineAsync.GetHomeTimeline();
            if (tweets.Any())
            {
                _maxId = tweets.Last().Id;
            }
            return tweets;
        }

        private async Task<IEnumerable<ITweet>> GetMaxIdStatus()
        {
            var tweets = await TimelineAsync.GetHomeTimeline(new HomeTimelineRequestParameters() {MaxId = _maxId});
            if (tweets.Any())
            {
                _maxId = tweets.Last().Id;
            }
            return tweets;
        }

        private async Task<IEnumerable<ITweet>> GetMaxIdMentions()
        {
            var tweets = await TimelineAsync.GetMentionsTimeline(new MentionsTimelineRequestParameters() { MaxId = _maxId });
            if (tweets.Any())
            {
                _maxId = tweets.Last().Id;
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
            var tweets = await TimelineAsync.GetHomeTimeline(new HomeTimelineRequestParameters() { SinceId = this.First().Id });
            if (!tweets.Any())
            {
                IsLoading = false;
                return;
            }
            var results = tweets.Reverse();
            foreach (var tweet in results)
            {
                Insert(0, tweet);
            }
            IsLoading = false;
        }

        public bool HasMoreItems { get; private set; }
    }
}
