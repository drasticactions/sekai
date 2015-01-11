using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using LinqToTwitter;
using Newtonsoft.Json;
using Sekai.Commands;
using Sekai.Common;
using Sekai.Tools;
using Sekai.Views;

namespace Sekai.ViewModels
{
    public class LoginPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private string _password;
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        public NavigateToMainPageCommand NavigateToMainPageCommand { get; set; } = new NavigateToMainPageCommand();
        public string Pin
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
                ClickLoginButtonCommand.RaiseCanExecuteChanged();
            }
        }

        public LoginPageViewModel()
        {
            ClickLoginButtonCommand = new AsyncDelegateCommand(async o => { await ClickLoginButton(); }, o => CanClickLoginButton);
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

        public bool CanClickLoginButton => !string.IsNullOrWhiteSpace(Pin);

        public AsyncDelegateCommand ClickLoginButtonCommand { get; private set; }
        public event EventHandler<EventArgs> LoginSuccessful;
        public event EventHandler<EventArgs> LoginFailed;

        public async Task ClickLoginButton()
        {
            bool loginResult;
            IsLoading = true;
            await Locator.ViewModels.MainPageVm.Authorizer.CompleteAuthorizeAsync(Pin);
            var verifyResponse =
            await
                (from acct in Locator.ViewModels.MainPageVm.TwitterContext.Account
                 where acct.Type == AccountType.VerifyCredentials
                 select acct)
                .SingleOrDefaultAsync();

            if (verifyResponse?.User != null)
            {
                Locator.ViewModels.MainPageVm.User = verifyResponse.User;
                _localSettings.Values[Constants.Credential] = JsonConvert.SerializeObject(Locator.ViewModels.MainPageVm.Authorizer.CredentialStore);
                loginResult = true;
            }
            else
            {
                loginResult = false;
            }
            IsLoading = false;
            RaiseEvent(loginResult ? LoginSuccessful : LoginFailed, EventArgs.Empty);
        }
    }
}
