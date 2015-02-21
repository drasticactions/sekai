using System;
using System.Collections.Generic;
using System.Text;
using Sekai.Command;
using Sekai.Common;

namespace Sekai.ViewModels
{
    public class SettingsPageViewModel : NotifierBase
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
        public ChangeBackgroundWallpaperCommand ChangeBackgroundWallpaperCommand { get; set; } = new ChangeBackgroundWallpaperCommand();
    }
}
