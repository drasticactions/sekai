using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using LinqToTwitter;
using Sekai.Commands;
using Sekai.Common;
using Sekai.ScrollingCollection;

namespace Sekai.ViewModels
{
    public class MainPageViewModel : NotifierBase
    {
        public void Initialize()
        {
            TimelineScrollingCollection = new TimelineScrollingCollection()
            {
                StatusType = StatusType.Home
            };
        }
        public NavigateToSettingsCommand NavigateToSettingsCommand { get; set; } = new NavigateToSettingsCommand();
        private TimelineScrollingCollection _timelineScrollingCollection;
        private PinAuthorizer _authorizer;

        public TwitterContext TwitterContext { get; set; }

        private User _user;
        public PinAuthorizer Authorizer
        {
            get { return _authorizer; }
            set
            {
                SetProperty(ref _authorizer, value);
                OnPropertyChanged();
            }
        }

        public TimelineScrollingCollection TimelineScrollingCollection
        {
            get { return _timelineScrollingCollection; }
            set
            {
                SetProperty(ref _timelineScrollingCollection, value);
                OnPropertyChanged();
            }
        }

        public User User
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value);
                OnPropertyChanged();
            }
        }

        
    }
}
