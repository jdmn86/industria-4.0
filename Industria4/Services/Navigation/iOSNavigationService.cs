using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Industria4.DataServices;
using Industria4.Helpers;
using Industria4.Pages;
using Industria4.Services.Dialog;
using Industria4.Services.SignalR;
using Industria4.ViewModels;
using Industria4.ViewModels.Base;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Industria4.Services.Navigation
{
    public class iOSNavigationService: NavigationService
    {
        private Type _requestedPageType;
        private object _requestedNavigationParameter;

        //private readonly IUserDialogs _userDialogs;

        public iOSNavigationService(IAuthenticationService authenticationService, ISIgnalRService sIgnalRService ) : base(authenticationService,sIgnalRService)
        {
            CreatePageViewModelMappings();

            MessagingCenter.Subscribe<iOSMainPage>(this, MessengerKeys.iOSMainPageCurrentChanged, OnMainPageCurrentChanged);
        }

        public override Task RemoveLastFromBackStackAsync()
        {
            var mainPage = CurrentApplication.MainPage as iOSMainPage;

            if (mainPage != null)
            {
                var currentNavigation = mainPage.CurrentPage as CustomNavigationPage;

            /*    if (currentNavigation?.CurrentPage is BookingPage)
                {
                    return mainPage.ClearNavigationForPage(typeof(HomePage));
                }*/
            }

            return Task.FromResult(true);
        }

        protected override async Task InternalNavigateToAsync(Type _viewModelType, object parameter)
        {
//descomentar pk não consigo ligar wifi
/*            if (CrossConnectivity.Current.IsConnected)
            {
                //await DisplayAlert("Connexion Test", " Error Your are not connected", "OK");
                App.connectwifi.SetupWifi();
            }
*/
            var viewModelType = _viewModelType;

            if (Settings.expires_on < DateTime.Now.Date)
            {
                Debug.WriteLine("entrou no navigation com expired");
                Debug.WriteLine("Setting date :" + Settings.expires_on.ToString());
                Debug.WriteLine("DateTime.Now :" + DateTime.Now.AddMinutes(1).ToString());

                // await _dialogService.ShowAlertAsync("O seu login expirou", "Error", "Ok");

                viewModelType = typeof(LoginViewModel);
                parameter = null;
            }

            Page page = CreateAndBindPage(viewModelType, parameter);
            _requestedPageType = page.GetType();
            _requestedNavigationParameter = parameter;

            if (page is iOSMainPage)
            {
                InitalizeMainPage(page as iOSMainPage);
                await InitializeTabPageCurrentPageViewModelAsync(parameter);
            }
            else if (page is LoginPage)
            {
                CurrentApplication.MainPage = new CustomNavigationPage(page);
                await InitializePageViewModelAsync(page, parameter);
            }
            else if (CurrentApplication.MainPage is iOSMainPage)
            {
                var mainPage = CurrentApplication.MainPage as iOSMainPage;

                await mainPage.CurrentPage.Navigation.PushAsync(page); 
                await InitializePageViewModelAsync(page, parameter);
            /*    bool tabPageFound = mainPage.TrySetCurrentPage(page);

                if (!tabPageFound)
                {
                    await mainPage.CurrentPage.Navigation.PushAsync(page);
                    await InitializePageViewModelAsync(page, parameter);
                }*/
            }
            else
            {
                var navigationPage = CurrentApplication.MainPage as CustomNavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    CurrentApplication.MainPage = new CustomNavigationPage(page);
                }

                await InitializePageViewModelAsync(page, parameter);
            }
        }

        private Task InitializePageViewModelAsync(Page page, object parameter)
        {
            return (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private Task InitializeTabPageCurrentPageViewModelAsync(object parameter)
        {
            var mainPage = CurrentApplication.MainPage as iOSMainPage;
            return ((mainPage.CurrentPage as CustomNavigationPage).CurrentPage.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private void InitalizeMainPage(iOSMainPage mainPage)
        {
            CurrentApplication.MainPage = mainPage;

            var homePage = CreateAndBindPage(typeof(HomeViewModel), null);
            mainPage.AddPage(homePage, "Home");

            var dataAnalyse = CreateAndBindPage(typeof(DataAnalyseViewModel), null);
            mainPage.AddPage(dataAnalyse, "Data Analyse");


            if (Settings.Role.Equals("AdminGlobal"))
            {
                var listaFabricasPage = CreateAndBindPage(typeof(ListaFabricasViewModel), null);
                mainPage.AddPage(listaFabricasPage, "Lista Fabricas");

                var FabricaPage = CreateAndBindPage(typeof(FabricaViewModel), null);
                mainPage.AddPage(FabricaPage, "Adicionar Fabrica");

                var listaMaquinasPage = CreateAndBindPage(typeof(MaquinasViewModel), null);
                mainPage.AddPage(listaMaquinasPage, "Lista Maquinas");

                var MaquinaPage = CreateAndBindPage(typeof(MaquinaViewModel), null);
                mainPage.AddPage(MaquinaPage, "Adicionar Maquina");

                var listaModuloPage = CreateAndBindPage(typeof(ModuloAquisicaoViewModel), null);
                mainPage.AddPage(listaModuloPage, "Lista Modulos Aquisição");

                var ModuloPage = CreateAndBindPage(typeof(ModuloAquisicaoViewModel), null);
                mainPage.AddPage(ModuloPage, "Adicionar Modulo Aquisição");

                var listaSensoresPage = CreateAndBindPage(typeof(SensoresViewModel), null);
                mainPage.AddPage(listaSensoresPage, "Lista Sensores");

                var sensorPage = CreateAndBindPage(typeof(SensorViewModel), null);
                mainPage.AddPage(sensorPage, "Adicionar sensor");

                var listaUserPage = CreateAndBindPage(typeof(UsersViewModel), null);
                mainPage.AddPage(listaUserPage, "Lista Users");

                var userPage = CreateAndBindPage(typeof(UserViewModel), null);
                mainPage.AddPage(userPage, "Adicionar User");

            }
            if (Settings.Role.Equals("AdminLocal"))
            {
                var listaFabricasPage = CreateAndBindPage(typeof(ListaFabricasViewModel), null);
                mainPage.AddPage(listaFabricasPage, "Lista Fabricas");

                var listaMaquinasPage = CreateAndBindPage(typeof(MaquinasViewModel), null);
                mainPage.AddPage(listaMaquinasPage, "Lista Maquinas");

                var MaquinaPage = CreateAndBindPage(typeof(MaquinaViewModel), null);
                mainPage.AddPage(MaquinaPage, "Adicionar Maquina");

                var listaModuloPage = CreateAndBindPage(typeof(ModuloAquisicaoViewModel), null);
                mainPage.AddPage(listaModuloPage, "Lista Modulos Aquisição");

                var ModuloPage = CreateAndBindPage(typeof(ModuloAquisicaoViewModel), null);
                mainPage.AddPage(ModuloPage, "Adicionar Modulo Aquisição");

                var listaSensoresPage = CreateAndBindPage(typeof(SensoresViewModel), null);
                mainPage.AddPage(listaSensoresPage, "Lista Sensores");

                var sensorPage = CreateAndBindPage(typeof(SensorViewModel), null);
                mainPage.AddPage(sensorPage, "Adicionar sensor");

                var listaUserPage = CreateAndBindPage(typeof(UsersViewModel), null);
                mainPage.AddPage(listaUserPage, "Lista Users");

                var userPage = CreateAndBindPage(typeof(UserViewModel), null);
                mainPage.AddPage(userPage, "Adicionar User");

            }

            if (Settings.Role.Equals("Func")){
                
            }
        }

        private async void OnTabPageAppearing(object sender, EventArgs e)
        {
            await InitializeTabPageCurrentPageViewModelAsync(null);
        }

        private void CreatePageViewModelMappings()
        {
            if (_mappings.ContainsKey(typeof(MainViewModel)))
            {
                _mappings[typeof(MainViewModel)] = typeof(iOSMainPage);
            }
            else
            {
                _mappings.Add(typeof(MainViewModel), typeof(iOSMainPage));
            }
        }

        private async void OnMainPageCurrentChanged(iOSMainPage mainPage)
        {
            object parameter = null;

            CustomNavigationPage navigation = mainPage.CurrentPage as CustomNavigationPage;

       /*     if (navigation.CurrentPage is BookingPage && _requestedPageType == typeof(BookingPage))
            {
                parameter = _requestedNavigationParameter;

                _requestedNavigationParameter = null;
                _requestedPageType = null;
            } */

            await InitializeTabPageCurrentPageViewModelAsync(parameter);
        }
    }
}