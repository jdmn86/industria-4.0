using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Industria4.DataServices;
using Industria4.Helpers;
using Industria4.Pages;
using Industria4.Services.Dialog;
using Industria4.Services.SignalR;
using Industria4.ViewModels;
using Industria4.ViewModels.Base;
using Industria4.Views;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Industria4.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IAuthenticationService _authenticationService;
     //   private readonly IUserDialogs _userDialogs;

        protected readonly Dictionary<Type, Type> _mappings;

        protected Application CurrentApplication
        {
            get
            {
                return Application.Current;
            }
        }

        public NavigationService(IAuthenticationService authenticationService, ISIgnalRService sIgnalRService )
        {
            _authenticationService = authenticationService;
       //     _userDialogs = userDialogs;

            _mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
         //   CreateMessengerSubscriptions();
        }

        public Task InitializeAsync()
        {
            Debug.WriteLine("NavigationService InitializeAsync");

           if (_authenticationService.IsAuthenticated)
            {
                if (Settings.expires_on > DateTime.Now){
                    Debug.WriteLine("NavigationService GO MainViewModel");
                    return NavigateToAsync<MainViewModel>();      
                }else{
                    Debug.WriteLine("NavigationService GO LoginViewModel");
                    return NavigateToAsync<LoginViewModel>();
                }
              
            }
            else
            { 
                Debug.WriteLine("NavigationService GO LoginViewModel");
                return NavigateToAsync<LoginViewModel>();
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is MainPage)
            {
                var mainPage = CurrentApplication.MainPage as MainPage;
                await mainPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
           
            var mainPage = CurrentApplication.MainPage as MainPage;


            if (mainPage != null)
            {
               
                mainPage.Detail.Navigation.RemovePage(
                    mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type _viewModelType, object parameter)
        {
/*            if (CrossConnectivity.Current.IsConnected)
            {
                //await DisplayAlert("Connexion Test", " Error Your are not connected", "OK");
                App.connectwifi.SetupWifi();
            }
*/
            var viewModelType = _viewModelType;

            //if (Settings.expires_on < DateTime.Today)
            if (Settings.expires_on < DateTime.Now)
            {
                Debug.WriteLine("entrou no navigation com expired");
                Debug.WriteLine("Setting date :" + Settings.expires_on.ToString());
                Debug.WriteLine("DateTime.Now :" + DateTime.Now.AddMinutes(1).ToString());

               // await _dialogService.ShowAlertAsync("O seu login expirou", "Error", "Ok");

                viewModelType = typeof(LoginViewModel);
                parameter = null;
           }

            Page page = CreateAndBindPage(viewModelType, parameter);

            if (page is MainPage)
            {
                CurrentApplication.MainPage = page;
            }
            else if (page is LoginPage)
            {
                CurrentApplication.MainPage = new CustomNavigationPage(page);
            }
            else if (CurrentApplication.MainPage is MainPage)
            {
                var mainPage = CurrentApplication.MainPage as MainPage;

                var navigationPage = mainPage.Detail as CustomNavigationPage;

                if (navigationPage != null)
                {

                    // await navigationPage.PushAsync(page);
                    navigationPage = new CustomNavigationPage(page);
                    mainPage.Detail = navigationPage;

                }
                else
                {
            

                    navigationPage = new CustomNavigationPage(page);
                    //Application.Current.MainPage = new CustomNavigationPage(page);
                    mainPage.Detail = navigationPage;
                }

                mainPage.IsPresented = false;
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
                }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

                if (pageType == null)
                {
                    throw new Exception($"Mapping type for {viewModelType} is not a page");
                }

            Page page = Activator.CreateInstance(pageType) as Page;
                ViewModelBase viewModel = ViewModelLocator.Instance.Resolve(viewModelType) as ViewModelBase;
                page.BindingContext = viewModel;

            if (page is IPageWithParameters)
                {
                    ((IPageWithParameters)page).InitializeWith(parameter);
                }
                
            return page;
        }

        private void CreatePageViewModelMappings()
        {
         //   _mappings.Add(typeof(CustomRideViewModel), typeof(CustomRidePage));
        //    _mappings.Add(typeof(EventSummaryViewModel), typeof(EventSummaryPage));
            _mappings.Add(typeof(HomeViewModel), typeof(HomePage));
            _mappings.Add(typeof(LoginViewModel), typeof(LoginPage));

            if (Device.OS == TargetPlatform.Windows)
            {
                //   _mappings.Add(typeof(SignUpViewModel), typeof(UwpSignUpPage));

                if (Device.Idiom == TargetIdiom.Desktop)
                {
                    //      _mappings.Add(typeof(UwpMyRidesViewModel), typeof(UwpMyRidesPage));
                    //     _mappings.Add(typeof(ProfileViewModel), typeof(UwpProfilePage));
                }
                else
                {
                    //        _mappings.Add(typeof(MyRidesViewModel), typeof(MyRidesPage));
                    //       _mappings.Add(typeof(ProfileViewModel), typeof(ProfilePage));
                }
            }
            else
            {
                //       _mappings.Add(typeof(SignUpViewModel), typeof(SignUpPage));
                //       _mappings.Add(typeof(MyRidesViewModel), typeof(MyRidesPage));
                //       _mappings.Add(typeof(ProfileViewModel), typeof(ProfilePage));
            }

         //   _mappings.Add(typeof(ReportIncidentViewModel), typeof(ReportIncidentPage));
         //   _mappings.Add(typeof(BookingViewModel), typeof(BookingPage));
            _mappings.Add(typeof(MainViewModel), typeof(MainPage));
            _mappings.Add(typeof(ListaFabricasViewModel), typeof(ListaFabricasPage));
            _mappings.Add(typeof(FabricaViewModel), typeof(FabricaPage));
            _mappings.Add(typeof(MaquinasViewModel), typeof(ListaMaquinasPage));
            _mappings.Add(typeof(MaquinaViewModel), typeof(MaquinaPage));
            _mappings.Add(typeof(ModulosAquisicaoViewModel), typeof(ListaModulosAquisicaoPage));
            _mappings.Add(typeof(ModuloAquisicaoViewModel), typeof(ModuloAquisicaoPage));
            _mappings.Add(typeof(SensoresViewModel), typeof(ListaSensoresPage));
            _mappings.Add(typeof(SensorViewModel), typeof(SensorPage));
            _mappings.Add(typeof(UsersViewModel), typeof(ListaUsersPage));
            _mappings.Add(typeof(UserViewModel), typeof(UserPage));
            _mappings.Add(typeof(DataAnalyseViewModel), typeof(DataAnalysePage));
        }

 /*       private void CreateMessengerSubscriptions()
        {
            MessagingCenter.Subscribe<ReportedIssue>(this, MessengerKeys.GoBackFromReportRequest, GoBackFromReportRequested);
        }

        private async void GoBackFromReportRequested(ReportedIssue issue)
        {
            if (Device.OS != TargetPlatform.iOS)
            {
                await NavigateBackAsync();
            }
        }*/

      
    }
}
