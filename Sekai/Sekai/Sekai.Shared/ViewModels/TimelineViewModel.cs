using System;
using System.Collections.Generic;
using System.Text;
using Sekai.Command;
using Sekai.Common;
using Sekai.Database.Models;
using Sekai.ScrollingCollection;

namespace Sekai.ViewModels
{
    public class TimelineViewModel : NotifierBase
    {
        public TimelineEntry TimelineEntry { get; set; }
        private TimelineScrollingCollection _timeline;
        public OpenImagePopupCommand OpenImagePopupCommand { get; set; } = new OpenImagePopupCommand();
        public TimelineScrollingCollection Timeline
        {
            get { return _timeline; }
            set
            {
                SetProperty(ref _timeline, value);
                OnPropertyChanged();
            }
        }
    }
}
