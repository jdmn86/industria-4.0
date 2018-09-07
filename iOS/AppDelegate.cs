using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Foundation;
using Industria4.iOS.Helpers;
using Industria4.Services.Navigation;
using Industria4.ViewModels.Base;
using Syncfusion.SfPicker.XForms.iOS;
using UIKit;
using UserNotifications;

namespace Industria4.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                    (approved, error) => { });

                // Watch for notifications while app is active
                UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                        UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                        new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            App.connectwifi = new ConnectToWifi();

      

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, policyErrors) => { return true; };
            
            global::Xamarin.Forms.Forms.Init();
     //       SfPickerRenderer.Init();


            ViewModelLocator.Instance.RegisterSingleton<INavigationService, iOSNavigationService>();
            LoadApplication(new App());


            return base.FinishedLaunching(app, options);
        }

       
    }
}
