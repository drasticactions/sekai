using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sekai.Common;
using Sekai.Database.Models;
using Sekai.Tools;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Windows.UI.Xaml.Controls;
using Sekai.Command;
using Sekai.Controls;
using Sekai.Database.Context;

namespace Sekai.ViewModels
{
    public class FeedPageViewModel : NotifierBase
    {
        public async Task Initialize(TwitterUserAccount user)
        {
            IsLoading = true;
            TwitterCredentials.ApplicationCredentials = TwitterCredentials.CreateCredentials(user.AccountToken, user.AccountSecret, Constants.ConsumerKey, Constants.ConsumerSecret);
            TwitterUser = await UserAsync.GetLoggedUser();
#if WINDOWS_PHONE_APP
            await CreatePivot();
#endif
            IsLoading = false;
        }
        private bool _isLoading;
        private ILoggedUser _twitterUser;
        private TimelineContext _db = new TimelineContext();
        public AddNewTimelineCommand AddNewTimelineCommand { get; set; } = new AddNewTimelineCommand();
        public NavigateToSettingsCommand NavigateToSettingsCommand { get; set; } = new NavigateToSettingsCommand();
#if WINDOWS_PHONE_APP
        private ObservableCollection<PivotItem> _pivotItems;
        public ObservableCollection<PivotItem> PivotItems
        {
            get { return _pivotItems; }
            set
            {
                SetProperty(ref _pivotItems, value);
                OnPropertyChanged();
            }
        }

        public async Task CreatePivot()
        {
            PivotItems = new ObservableCollection<PivotItem>();
            var timelineEnteries = await _db.TimelineEntries.ToListAsync();
            if (!timelineEnteries.Any())
            {
                var home = new TimelineEntry()
                {
                    AccountUserId = TwitterUser.Id,
                    Name = "Home"
                };
                await _db.AddAsync(home);
                await _db.SaveChangesAsync();
                timelineEnteries.Add(home);
            }
            foreach (var timeline in timelineEnteries)
            {
                var pivotItem = new PivotItem {Header = timeline.Name};
                var timelineControl = new TimelineUserControl();
                timelineControl.Initialize(timeline);
                pivotItem.Content = timelineControl;
                PivotItems.Add(pivotItem);
            }
        }
#endif

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }

        public ILoggedUser TwitterUser
        {
            get { return _twitterUser; }
            set
            {
                SetProperty(ref _twitterUser, value);
                OnPropertyChanged();
            }
        }

        public async Task AddHomeTimeline()
        {
            var home = new TimelineEntry()
            {
                AccountUserId = TwitterUser.Id,
                Name = "Home"
            };
            await _db.AddAsync(home);
            await _db.SaveChangesAsync();
#if WINDOWS_PHONE_APP
            await CreatePivot();
#endif
        }

        public async Task AddUserMentions()
        {
            var home = new TimelineEntry()
            {
                AccountUserId = TwitterUser.Id,
                Name = "Mentions",
                IsMentions = true
            };
            await _db.AddAsync(home);
            await _db.SaveChangesAsync();
#if WINDOWS_PHONE_APP
            await CreatePivot();
#endif
        }
#if WINDOWS_PHONE_APP
        public async Task RemoveTimeline(TimelineEntry timeline, PivotItem item)
        {
            _db.TimelineEntries.Remove(timeline);
            await _db.SaveChangesAsync();
            PivotItems.Remove(item);
        }
#endif

#if WINDOWS_APP
        public async Task RemoveTimeline(TimelineEntry timeline)
        {
            _db.TimelineEntries.Remove(timeline);
            await _db.SaveChangesAsync();
        }
#endif

    }
}
