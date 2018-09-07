using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Syncfusion.SfPicker.XForms.Droid;
using Industria4.Droid.Helpers;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net.Security;
using Xamarin.Android.Net;
using Android.Content.Res;
using Java.Security.Cert;
using System.Text;

namespace Industria4.Droid
{
    [Activity(Label = "Industria4.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            App.connectwifi = new ConnectToWifi();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            AndroidClientHandler clientHandler = new AndroidClientHandler();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Ssl3;

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, policyErrors) => { return true; };
          //  ServicePointManager.ServerCertificateValidationCallback =certBytes;
                           

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
        //    UserDialogs.Init(this);
            //new SfPickerRenderer();


            LoadApplication(new App());
        }
    }
}
