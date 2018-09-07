using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Industria4.Pages
{
    public partial class ListaMaquinasPage : ContentPage
    {
        public ListaMaquinasPage()
        {
            InitializeComponent();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            //   ((MasterDetailPage)App.Current.MainPage).Detail = new NavigationPage(new Views.MaquinaPage(((ListView)sender).SelectedItem as Maquina));

        }
    }
}
