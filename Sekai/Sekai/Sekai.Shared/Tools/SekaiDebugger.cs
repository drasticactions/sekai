using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Sekai.Tools
{
    public static class SekaiDebugger
    {
        public static async Task SendMessageDialogAsync(string message, Exception ex)
        {
            var dialog = new MessageDialog((string.Concat(message, Environment.NewLine, Environment.NewLine, ex.Message)));
            await dialog.ShowAsync();
        }

        public static async Task SendMessageDialogAsync(string message, string debug)
        {
            var dialog = new MessageDialog((string.Concat(message, Environment.NewLine, Environment.NewLine, debug)));
            await dialog.ShowAsync();
        }
    }
}
