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
            var tweets =
               await
               (from tweet in Locator.ViewModels.MainPageVm.TwitterContext.Status
                where tweet.Type == StatusType
                select tweet)
               .ToListAsync();
            tweets.Reverse	()
            IsLoading = false;
            return new LoadMoreItemsResult { Count = count };
        }
        public bool HasMoreItems { get; private set; }
    }
}
