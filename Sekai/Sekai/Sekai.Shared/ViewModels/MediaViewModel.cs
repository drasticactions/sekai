using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.UI.Xaml.Controls;
using Sekai.Common;
using Sekai.Controls;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Sekai.ViewModels
{
    public class MediaViewModel : NotifierBase
    {
        private bool _isLoading;
        private List<IMediaEntity> _items;
        private ITweet _tweet;
        public ITweet Tweet
        {
            get { return _tweet; }
            set
            {
                SetProperty(ref _tweet, value);
                OnPropertyChanged();
            }
        }
        public List<IMediaEntity> Items
        {
            get { return _items; }
            set
            {
                SetProperty(ref _items, value);
                OnPropertyChanged();
            }
        }

        public void InitializeFlipView(List<IMediaEntity> mediaEntities, ITweet tweet)
        {
            Items = new List<IMediaEntity>();
            Tweet = tweet;
            Items = mediaEntities;
        }


        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }
    }
}
