using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Industria4.Pages
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            //LogoutButton.Margin = Device.Idiom == TargetIdiom.Desktop ? new Thickness(14, 0, 0, 0) : new Thickness(14, 0, 0, 14);
        }
    }
}
