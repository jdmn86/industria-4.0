using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Industria4.Helpers;
using Industria4.Resources;
using Industria4.Services.Navigation;
using Industria4.Services.SignalR;
using Industria4.ViewModels.Base;
using Plugin.Multilingual;
using Plugin.Notifications;
using Xamarin.Forms;

namespace Industria4
{
    public partial class App : Application
    {
        private INavigationService navigationService;
        public static IWifiConnect connectwifi { get; set; }

		public App()
        {
           
            CrossNotifications.Current.RequestPermission();
            //  _navigationService = navigationService;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTMwMDRAMzEzNjJlMzIyZTMwUkZvMCs0aCtRQ2kyUG9FRlZUaWhGSVZja0lCY1k4Qi9UQkJCUS92eHluZz0 =");
            InitializeComponent();

       /*     if (!VirtualizationManager.IsInitialized)
            {
                VirtualizationManager.Instance.UIThreadExcecuteAction = (a) => Device.BeginInvokeOnMainThread(a);
                Device.StartTimer(
                    TimeSpan.FromSeconds(1),
                    () => {
                        VirtualizationManager.Instance.ProcessActions(); return true;
                    });
            }
*/
            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;


        CrossNotifications.Current.RequestPermission();


            if (Device.OS == TargetPlatform.Windows)
            {
                InitNavigation();
            }

        }

        protected override async void OnStart()
        {
            base.OnStart();

            Debug.WriteLine("APP OnStart");

            if (Device.OS != TargetPlatform.Windows)
            {
                await InitNavigation();
            }
        }

        private Task InitNavigation()
        {
            Debug.WriteLine("APP InitNavigation");

            var navigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
            
            return navigationService.InitializeAsync();
        }


        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            //    if(Settings.expires_on < DateTime.Now.AddMinutes(1)){
            //        Settings.RemoveAll();
            //        navigationService.NavigateToAsync<LoginViewModel>();
            //    }

        }
    }
}
