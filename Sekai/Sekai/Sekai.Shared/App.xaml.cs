using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Autofac;
using Sekai.Common;
using Sekai.Database.Context;
using Sekai.Tools;
using Sekai.Views;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Sekai
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
        public ContinuationManager continuationManager { get; private set; }
#endif
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        public static IContainer Container;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
#if WINDOWS_APP
            RequestedTheme = ApplicationTheme.Light;
#endif
#if WINDOWS_PHONE_APP
            RequestedTheme = ApplicationTheme.Dark;
                        var type = typeof(Microsoft.Data.Entity.Relational.RelationalDatabase).GetTypeInfo()
                .Assembly.GetType("Microsoft.Data.Entity.Relational.Strings");
            WindowsRuntimeResourceManager.InjectIntoResxGeneratedApplicationResourcesClass(type);
#endif
            try
            {
                using (var db = new TwitterUserAccountContext())
                {
                    // Migrations are not yet enabled in EF7, so use an
                    // API to create the database if it doesn't exist
                    db.Database.EnsureCreated();
                }
            }
            catch (Exception)
            {


            }
            try
            {
                using (var db = new TimelineContext())
                {
                    // Migrations are not yet enabled in EF7, so use an
                    // API to create the database if it doesn't exist
                    db.Database.EnsureCreated();
                }
            }
            catch (Exception)
            {


            }
            Container = AutoFacConfiguration.Configure();
        }

        public static Frame RootFrame;
        private Frame CreateRootFrame()
        {
            RootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                RootFrame = new Frame();

                // Set the default language
                RootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                // Place the frame in the current Window
                Window.Current.Content = RootFrame;
            }

            return RootFrame;
        }

        private async Task RestoreStatusAsync(ApplicationExecutionState previousExecutionState)
        {
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (previousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException)
                {
                    //Something went wrong restoring state.
                    //Assume there is no state and continue
                }
            }
        }
#if WINDOWS_PHONE_APP
        /// <summary>
        /// Handle OnActivated event to deal with File Open/Save continuation activation kinds
        /// </summary>
        /// <param name="e">Application activated event arguments, it can be casted to proper sub-type based on ActivationKind</param>
        protected async override void OnActivated(IActivatedEventArgs e)
        {
            base.OnActivated(e);

            continuationManager = new ContinuationManager();

            RootFrame = CreateRootFrame();
            await RestoreStatusAsync(e.PreviousExecutionState);

            if (RootFrame.Content == null)
            {
                RootFrame.Navigate(typeof(AccountPage));
            }

            var continuationEventArgs = e as IContinuationActivatedEventArgs;
            if (continuationEventArgs != null)
            {
                var accountPage = RootFrame.Content as AccountPage;
                if (accountPage != null)
                {
                    // Call ContinuationManager to handle continuation activation
                   accountPage.ContinueWebAuthentication(continuationEventArgs as WebAuthenticationBrokerContinuationEventArgs);
                }
            }

            Window.Current.Activate();
        }
#endif
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            RootFrame = Window.Current.Content as Frame;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                RootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                RootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = RootFrame;
            }

            if (RootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (RootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in RootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                RootFrame.ContentTransitions = null;
                RootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif
                if (_localSettings.Values.ContainsKey(Constants.BackgroundWallpaper))
                {
                    var hasWallpaper = (bool)_localSettings.Values[Constants.BackgroundWallpaper];
                    if (hasWallpaper)
                    {
                        var storageFolder = ApplicationData.Current.LocalFolder;
                        var result = await storageFolder.FileExistsAsync(Constants.WallpaperFilename);
                        StorageFile file;
                        if (result)
                        {
                            file = await storageFolder.GetFileAsync(Constants.WallpaperFilename);

                        }
                        else
                        {
                            string CountriesFile = @"Assets\plugins.png";
                            StorageFolder InstallationFolder = Package.Current.InstalledLocation;
                            file = await InstallationFolder.GetFileAsync(CountriesFile);
                        }

                        using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            var bitmapImage = new BitmapImage();
                            var brush = new ImageBrush();
                            await bitmapImage.SetSourceAsync(fileStream);
                            brush.ImageSource = bitmapImage;
                            brush.Stretch = Stretch.UniformToFill;
                            RootFrame.Background = brush;
                        }
                    }
                    else
                    {
#if WINDOWS_APP
                        App.RootFrame.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
#endif
#if WINDOWS_PHONE_APP
                        App.RootFrame.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
#endif
                    }
                }
                else
                {
#if WINDOWS_APP
                        App.RootFrame.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
#endif
#if WINDOWS_PHONE_APP
                    App.RootFrame.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
#endif
                }
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!RootFrame.Navigate(typeof(AccountPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            RootFrame = sender as Frame;
            RootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            RootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}