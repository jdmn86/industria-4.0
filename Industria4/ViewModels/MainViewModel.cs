using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Industria4.Services.SignalR;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
       // public SignalRService SignalRClient = new SignalRService();

        private MenuViewModel _menuViewModel;

        public MenuViewModel MenuViewModel
        {
            get
            {
                return _menuViewModel;
            }

            set
            {
                _menuViewModel = value;
                RaisePropertyChanged(() => MenuViewModel);
            }
        }

        public MainViewModel(MenuViewModel menuViewModel)
        {
            _menuViewModel = menuViewModel;
        }

        public override Task InitializeAsync(object navigationData)
        {
       /*     SignalRClient.Start().ContinueWith(task => {
                if (task.IsFaulted)
                    Debug.WriteLine("An error occurred when trying to connect to SignalR: " + task.Exception.InnerExceptions[0].Message);
                //MainPage.DisplayAlert("Error", "An error occurred when trying to connect to SignalR: " + task.Exception.InnerExceptions[0].Message, "OK");
            });


            //try to reconnect every 10 seconds, just in case
            Device.StartTimer(TimeSpan.FromSeconds(10), () => {
                if (!SignalRClient.IsConnectedOrConnecting)
                    SignalRClient.Start();

                return true;
            });
*/



            return Task.WhenAll
                   (
                       _menuViewModel.InitializeAsync(navigationData),
                       NavigationService.NavigateToAsync<HomeViewModel>()
                   );
        }

    }

}
