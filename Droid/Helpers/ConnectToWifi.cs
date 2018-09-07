using System;
using Industria4.Helpers;
using Xamarin.Forms;

namespace Industria4.Droid.Helpers
{
    public class ConnectToWifi : IWifiConnect
    {

        public void SetupWifi()
        {
            Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionWifiSettings));
        }
    }
}
