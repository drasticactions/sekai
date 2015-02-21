using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.UI.Xaml.Controls;
using Sekai.Common;
using Sekai.Controls;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Sekai.ViewModels
{
    public class MediaViewModel : NotifierBase
    {
        private bool _isLoading;
        private List<IMediaEntity> _items;

        public List<IMediaEntity> Items
        {
            get { return _items; }
            set
            {
                SetProperty(ref _items, value);
                OnPropertyChanged();
            }
        }

        public void InitializeFlipView(List<IMediaEntity> mediaEntities)
        {
            Items = new List<IMediaEntity>();
            foreach (var element in mediaEntities)
            {
                Items.Add(element);
                Items.Add(element);
            }
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
