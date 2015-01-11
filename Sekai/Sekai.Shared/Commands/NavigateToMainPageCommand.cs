
using Sekai.Common;
using Sekai.Views;

namespace Sekai.Commands
{
    public class NavigateToMainPageCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            Locator.ViewModels.MainPageVm.Initialize();
            App.RootFrame.Navigate(typeof (MainPage));
        }
    }
}
