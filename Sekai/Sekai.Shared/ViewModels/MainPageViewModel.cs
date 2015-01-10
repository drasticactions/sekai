using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using LinqToTwitter;
using Sekai.Common;

namespace Sekai.ViewModels
{
    public class MainPageViewModel : NotifierBase
    {
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
