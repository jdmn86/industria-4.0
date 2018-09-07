using System;
using System.Diagnostics;
using Industria4.DataServices;
using Industria4.DataServices.Accounts;
using Industria4.DataServices.Base;
using Industria4.DataServices.DataSensores;
using Industria4.DataServices.Fabricas;
using Industria4.DataServices.Grandezas;
using Industria4.DataServices.Maquinas;
using Industria4.DataServices.Messages;
using Industria4.DataServices.ModulosAquisicao;
using Industria4.DataServices.Roles;
using Industria4.DataServices.Rooms;
using Industria4.DataServices.Sensores;
using Industria4.DataServices.TipoPorta;
using Industria4.DataServices.Users;
using Industria4.Services.Dialog;
using Industria4.Services.Navigation;
using Industria4.Services.SignalR;
using Unity;
using Unity.Lifetime;
using Xamarin.Forms;

namespace Industria4.ViewModels.Base
{
    public class ViewModelLocator
    {
        private readonly UnityContainer _unityContainer;

        private static readonly ViewModelLocator _instance = new ViewModelLocator();

        public static ViewModelLocator Instance
        {
            get
            {
                return _instance;
            }
        }

        public ViewModelLocator()
        {
            Debug.WriteLine("ViewModelLocator create");
            _unityContainer = new UnityContainer();

            // Providers

            _unityContainer.RegisterType<IRequestProvider, RequestProvider>();


            // Services
          //  _unityContainer.RegisterType<IDialogService, DialogService>();
    
             RegisterSingleton<INavigationService, NavigationService>();
           
            RegisterSingleton<ISIgnalRService,SignalRService>();

        //    _unityContainer.RegisterType<IUserDialogs>();
     //       RegisterSingleton<IUserDialogs>(UserDialogs.Instance);
               
          //  RegisterSingleton<IDialogService, DialogService>();

  //    _unityContainer.RegisterInstance<IUserDialogs>(UserDialogs.Instance);
       //     RegisterSingleton<IUserDialogs,UserDialogs>(UserDialogs.Instance);

         //   _unityContainer.RegisterInstance<IUserDialogs>(UserDialogs.Instance);
            //       _unityContainer.RegisterType<IDialogService,DialogService>();
      
            // Data services

            // _unityContainer.RegisterType<IIdentityService, IdentityService>();
            _unityContainer.RegisterType<IFabricaService, FabricaService>();
            _unityContainer.RegisterType<IAuthenticationService, AuthenticationService>();
            _unityContainer.RegisterType<IUsersService, UsersService>();
            _unityContainer.RegisterType<IMaquinaService, MaquinaService>();
            _unityContainer.RegisterType<IModuloAquisicaoService, ModuloAquisicaoService>();
            _unityContainer.RegisterType<ISensoresService, SensoresService>();
            _unityContainer.RegisterType<ITiposPortaService, TiposPortaService>();
            _unityContainer.RegisterType<ITiposGrandezasService, TiposGrandezasService>();
            _unityContainer.RegisterType<IAccountService, AccountService>();
            _unityContainer.RegisterType<IRolesService, RolesService>();
            _unityContainer.RegisterType<IValoresSensorService, ValoresSensorService>();
            _unityContainer.RegisterType<IRoomsService, RoomsService>();
            _unityContainer.RegisterType<IMessagesService, MessagesService>();


            // View models

            _unityContainer.RegisterType<LoginViewModel>();
            _unityContainer.RegisterType<MainViewModel>();
            _unityContainer.RegisterType<MenuViewModel>();
            _unityContainer.RegisterType<ViewModelBase>();
            _unityContainer.RegisterType<HomeViewModel>();
            _unityContainer.RegisterType<ListaFabricasViewModel>();
            _unityContainer.RegisterType<FabricaViewModel>();
            _unityContainer.RegisterType<MaquinasViewModel>();
            _unityContainer.RegisterType<MaquinaViewModel>();
            _unityContainer.RegisterType<ModulosAquisicaoViewModel>();
            _unityContainer.RegisterType<ModuloAquisicaoViewModel>();
            _unityContainer.RegisterType<SensoresViewModel>();
            _unityContainer.RegisterType<SensorViewModel>();
            _unityContainer.RegisterType<UsersViewModel>();
            _unityContainer.RegisterType<UserViewModel>();
            _unityContainer.RegisterType<DataAnalyseViewModel>();

            //  _unityContainer.RegisterType<MainViewModel>(new ContainerControlledLifetimeManager());

            //   ServiceLocator.SetLocatorProvider(() => (CommonServiceLocator.IServiceLocator)_unityContainer);
        }


        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public void Register<T>(T instance)
        {
            _unityContainer.RegisterInstance<T>(instance);
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>();
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }

    }
}