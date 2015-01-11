using System;
using System.Collections.Generic;
using System.Text;
using LinqToTwitter;
using Sekai.Common;

namespace Sekai.ViewModels
{
    public class MessagePageViewModel : NotifierBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }

        private Status _status;

        public Status Status
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);
                OnPropertyChanged();
            }
        }
    }
}
