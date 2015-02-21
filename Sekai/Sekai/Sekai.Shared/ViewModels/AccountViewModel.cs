using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Documents;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Sekai.Common;
using Sekai.Database.Context;
using Sekai.Database.Models;
using Sekai.Tools;
using Sekai.Command;

namespace Sekai.ViewModels
{
    public class AccountViewModel : NotifierBase
    {
        public AccountViewModel() 
        {
            ClickLoginButtonCommand = new AsyncDelegateCommand(async o => { await ClickLoginButton(); },
                o => true);
        }
        private TwitterUserAccountContext _db = new TwitterUserAccountContext();
        private bool _isLoading;
        private ObservableCollection<TwitterUserAccount> _userAccounts; 
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TwitterUserAccount> UserAccounts
        {
            get { return _userAccounts; }
            set
            {
                SetProperty(ref _userAccounts, value);
                OnPropertyChanged();
            }
        }

        public NavigateToFeedPageCommand NavigateToFeedPageCommand { get; set; } = new NavigateToFeedPageCommand();
        public AsyncDelegateCommand ClickLoginButtonCommand { get; private set; }

        public async Task Initialize()
        {
            UserAccounts = _db.TwitterAccounts.ToObservableCollection();
        }

        public async Task ClickLoginButton()
        {
            IsLoading = true;
            try
            {
                string oAuth_Token = await GetTwitterRequestTokenAsync(Constants.Callback, Constants.ConsumerKey);
                string TwitterUrl = "https://api.twitter.com/oauth/authorize?oauth_token=" + oAuth_Token;

#if WINDOWS_PHONE_APP
                WebAuthenticationBroker.AuthenticateAndContinue(new Uri(TwitterUrl), new Uri(Constants.Callback), null, WebAuthenticationOptions.None);
#endif
            }
            catch
            {
                //error handling here
            }
            IsLoading = false;
        }

        public async Task AddAccount(string webAuthResultResponseData)
        {
            IsLoading = true;
            try
            {
                await GetTwitterUserNameAsync(webAuthResultResponseData);
            }
            catch (Exception)
            {
                // TODO: Set up error handler
            }
            IsLoading = false;
        }

        public async Task GetTwitterUserNameAsync(string webAuthResultResponseData)
        {
            var responseData = webAuthResultResponseData.Substring(webAuthResultResponseData.IndexOf("oauth_token", StringComparison.Ordinal));
            string requestToken = null;
            string oauthVerifier = null;
            var keyValPairs = responseData.Split('&');

            foreach (var splits in keyValPairs.Select(t => t.Split('=')))
            {
                switch (splits[0])
                {
                    case "oauth_token":
                        requestToken = splits[1];
                        break;
                    case "oauth_verifier":
                        oauthVerifier = splits[1];
                        break;
                }
            }

            const string twitterUrl = "https://api.twitter.com/oauth/access_token";

            string timeStamp = GetTimeStamp();
            string nonce = GetNonce();

            var sigBaseStringParams = "oauth_consumer_key=" + Constants.ConsumerKey;
            sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
            sigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
            sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
            sigBaseStringParams += "&" + "oauth_token=" + requestToken;
            sigBaseStringParams += "&" + "oauth_version=1.0";
            var sigBaseString = "POST&";
            sigBaseString += Uri.EscapeDataString(twitterUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

            var signature = GetSignature(sigBaseString, Constants.ConsumerSecret);

            var httpContent = new HttpStringContent("oauth_verifier=" + oauthVerifier, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            httpContent.Headers.ContentType = HttpMediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            var authorizationHeaderParams = "oauth_consumer_key=\"" + Constants.ConsumerKey + "\", oauth_nonce=\"" + nonce + "\", oauth_signature_method=\"HMAC-SHA1\", oauth_signature=\"" + Uri.EscapeDataString(signature) + "\", oauth_timestamp=\"" + timeStamp + "\", oauth_token=\"" + Uri.EscapeDataString(requestToken) + "\", oauth_version=\"1.0\"";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("OAuth", authorizationHeaderParams);
            var httpResponseMessage = await httpClient.PostAsync(new Uri(twitterUrl), httpContent);
            string response = await httpResponseMessage.Content.ReadAsStringAsync();

            var tokens = response.Split('&');
            string oauthTokenSecret = null;
            string oauthToken = null;
            string screenName = null;
            long userId = 0;
            foreach (var splits in tokens.Select(t => t.Split('=')))
            {
                switch (splits[0])
                {
                    case "screen_name":
                        screenName = splits[1];
                        break;
                    case "oauth_token":
                        oauthToken = splits[1];
                        break;
                    case "oauth_token_secret":
                        oauthTokenSecret = splits[1];
                        break;
                    case "user_id":
                        userId = Convert.ToInt64(splits[1]);
                        break;
                }
            }

            var user = new TwitterUserAccount()
            {
                AccountSecret = oauthTokenSecret,
                AccountToken = oauthToken,
                UserName = screenName,
                UserId = userId
            };

            UserAccounts.Add(user);
            await _db.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        string GetNonce()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < 32; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString().ToLower();
        }

        string GetTimeStamp()
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return Math.Round(sinceEpoch.TotalSeconds).ToString();
        }

        string GetSignature(string sigBaseString, string consumerSecretKey, string oAuthTokenSecret = null)
        {
            var keyMaterial = CryptographicBuffer.ConvertStringToBinary(consumerSecretKey + "&" + oAuthTokenSecret, BinaryStringEncoding.Utf8);
            var hmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            var macKey = hmacSha1Provider.CreateKey(keyMaterial);
            var dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(sigBaseString, BinaryStringEncoding.Utf8);
            var signatureBuffer = CryptographicEngine.Sign(macKey, dataToBeSigned);
            var signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);
            return signature;
        }

        private async Task<string> GetTwitterRequestTokenAsync(string twitterCallbackUrl, string consumerKey)
        {
            //
            // Acquiring a request token
            //
            string twitterUrl = "https://api.twitter.com/oauth/request_token";

            string nonce = GetNonce();
            string timeStamp = GetTimeStamp();
            string sigBaseStringParams = "oauth_callback=" + Uri.EscapeDataString(twitterCallbackUrl);
            sigBaseStringParams += "&" + "oauth_consumer_key=" + consumerKey;
            sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
            sigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
            sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
            sigBaseStringParams += "&" + "oauth_version=1.0";
            string sigBaseString = "GET&";
            sigBaseString += Uri.EscapeDataString(twitterUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);
            string signature = GetSignature(sigBaseString, Constants.ConsumerSecret);

            twitterUrl += "?" + sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature);
            var httpClient = new HttpClient();
            string getResponse = await httpClient.GetStringAsync(new Uri(twitterUrl));

            string requestToken = null;
            var keyValPairs = getResponse.Split('&');

            foreach (var t in keyValPairs)
            {
                string[] splits = t.Split('=');
                switch (splits[0])
                {
                    case "oauth_token":
                        requestToken = splits[1];
                        break;
                    case "oauth_token_secret":
                        break;
                }
            }

            return requestToken;
        }
    }
}
