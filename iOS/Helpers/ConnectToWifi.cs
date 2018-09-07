using System;
using Industria4.Helpers;
using Xamarin.Forms;

namespace Industria4.iOS.Helpers
{
    public class ConnectToWifi : IWifiConnect
    {
        public void SetupWifi()
        {
            Device.OpenUri(new Uri(string.Format("App-prefs:root=WIFI")));
        }
    }
}
