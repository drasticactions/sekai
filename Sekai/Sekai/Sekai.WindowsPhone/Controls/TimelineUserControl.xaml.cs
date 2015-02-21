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
using Sekai.Database.Models;
using Sekai.ScrollingCollection;
using Sekai.Tools;
using Sekai.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Sekai.Controls
{
    public sealed partial class TimelineUserControl : UserControl
    {
        private TimelineViewModel _vm;
        public TimelineUserControl()
        {
            this.InitializeComponent();
        }

        public void Initialize(TimelineEntry entry)
        {
            _vm = (TimelineViewModel)DataContext;
            _vm.Timeline = new TimelineScrollingCollection {UserMentions = entry.IsMentions};
            _vm.TimelineEntry = entry;
        }

        private async void PullToRefreshPanel_OnPullToRefresh(object sender, EventArgs e)
        {
            try
            {
                await _vm.Timeline.RefreshTweets();
            }
            catch (Exception ex)
            {
                SekaiDebugger.SendMessageDialogAsync("Failed to load tweets", ex);
            }
        }
    }
}
