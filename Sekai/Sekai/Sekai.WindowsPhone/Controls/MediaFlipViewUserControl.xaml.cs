using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Sekai.ViewModels;
using Tweetinvi.Core.Interfaces.Models.Entities;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sekai.Controls
{
    public sealed partial class MediaFlipViewUserControl : UserControl
    {
        private MediaViewModel _vm;
        private Popup _popup;
        public MediaFlipViewUserControl()
        {
            this.InitializeComponent();
        }

        public void Initialize(List<IMediaEntity> mediaEntities, Popup popup)
        {
            _popup = popup;
            _vm = (MediaViewModel)DataContext;
            _vm.InitializeFlipView(mediaEntities);
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = false;
        }
    }
}
