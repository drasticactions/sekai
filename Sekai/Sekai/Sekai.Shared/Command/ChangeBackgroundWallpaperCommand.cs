using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Pickers;
using Sekai.Common;

namespace Sekai.Command
{
    public class ChangeBackgroundWallpaperCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".gif");
#if WINDOWS_PHONE_APP
            openPicker.PickSingleFileAndContinue();
#endif
        }
    }
}
