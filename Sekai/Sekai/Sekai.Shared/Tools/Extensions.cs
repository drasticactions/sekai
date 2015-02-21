using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;

namespace Sekai.Tools
{
    static class Extensions
    {

        public static Task ShowAsync(this Popup popup)
        {

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            EventHandler<object> onClosed = null;
            onClosed = (s, e) => {
                popup.Closed -= onClosed;
                tcs.SetResult(true);
            };
            popup.Closed += onClosed;
            popup.IsOpen = true;

            return tcs.Task;
        }

    }
}
