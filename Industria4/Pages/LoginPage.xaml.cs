using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Industria4.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //   var osVersionString = ViewModelLocator.Instance.Resolve<IOperatingSystemVersionProvider>()
            //       .GetOperatingSystemVersionString();

            /*        if (Device.OS == TargetPlatform.iOS && osVersionString == "10.0.2")
                    {
                        SignInButton.BackgroundColor = Color.Black;
                    }  */
        }
    }
}
