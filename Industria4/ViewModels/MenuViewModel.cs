using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices;
using Industria4.DataServices.Users;
using Industria4.Helpers;
using Industria4.Models.Enums;
using Industria4.Services.Dialog;
using Industria4.ViewModels.Base;
using Xamarin.Forms;
using MenuItem = Industria4.Models.MenuItem;

namespace Industria4.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUsersService _usersService;
 //       private readonly IUserDialogs _userDialog;

        public ICommand ItemSelectedCommand => new Command<MenuItem>(OnSelectItem);

        public ICommand LogoutCommand => new Command(OnLogout);

        ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();
        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return menuItems;
            }
            set
            {
                menuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        private string _name;
        public string ProfileName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged(() => ProfileName);
            }
        }

        public MenuViewModel(IAuthenticationService authenticationService, IUsersService usersService )
        {
            _authenticationService = authenticationService;
            _usersService = usersService;
          //  _userDialog = userDialog;

            InitMenuItems();

        }

        public override async Task InitializeAsync(object navigationData)
        {
            ProfileName = Settings.Name;
        }

        private void InitMenuItems()
        {
            MenuItems.Add(new MenuItem
            {
                Title = "HOME",
                MenuItemType = MenuItemType.Home,
                ViewModelType = typeof(MainViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new MenuItem
            {
                Title = "ANALISE DE DADOS",
                MenuItemType = MenuItemType.AnaliseDados,
                ViewModelType = typeof(DataAnalyseViewModel),
                IsEnabled = true
            });

            if (Settings.Role.Equals("AdminGlobal"))
            {

                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE FABRICAS",
                    MenuItemType = MenuItemType.ListarFabricas,
                    ViewModelType = typeof(ListaFabricasViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "ADICIONAR FABRICA",
                    MenuItemType = MenuItemType.AddFabrica,
                    ViewModelType = typeof(FabricaViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE MAQUINAS",
                    MenuItemType = MenuItemType.ListarMaquinas,
                    ViewModelType = typeof(MaquinasViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "ADICIONAR MAQUINAS",
                    MenuItemType = MenuItemType.AddMaquina,
                    ViewModelType = typeof(MaquinaViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE MODULOS DE AQUISIÇÃO",
                    MenuItemType = MenuItemType.ListaModulosAquisicao,
                    ViewModelType = typeof(ModulosAquisicaoViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "ADICIONAR MODULOS DE AQUISIÇÃO",
                    MenuItemType = MenuItemType.AddModuloAquisicao,
                    ViewModelType = typeof(ModuloAquisicaoViewModel),
                    IsEnabled = true
                });
                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE SENSORES",
                    MenuItemType = MenuItemType.ListaSensores,
                    ViewModelType = typeof(SensoresViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "ADICIONAR SENSOR",
                    MenuItemType = MenuItemType.AddSensor,
                    ViewModelType = typeof(SensorViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE UTILIZADORES",
                    MenuItemType = MenuItemType.ListaUsers,
                    ViewModelType = typeof(UsersViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "ADICIONAR UTILIZADORES",
                    MenuItemType = MenuItemType.AddUser,
                    ViewModelType = typeof(UserViewModel),
                    IsEnabled = true
                });
            }

            if (Settings.Role.Equals("AdminLocal"))
            {
                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE FABRICAS",
                    MenuItemType = MenuItemType.ListarFabricas,
                    ViewModelType = typeof(ListaFabricasViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE MAQUINAS",
                    MenuItemType = MenuItemType.ListarMaquinas,
                    ViewModelType = typeof(MaquinasViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "ADICIONAR MAQUINAS",
                    MenuItemType = MenuItemType.AddMaquina,
                    ViewModelType = typeof(MaquinaViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE MODULOS DE AQUISIÇÃO",
                    MenuItemType = MenuItemType.ListaModulosAquisicao,
                    ViewModelType = typeof(ModulosAquisicaoViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "ADICIONAR MODULOS DE AQUISIÇÃO",
                    MenuItemType = MenuItemType.AddModuloAquisicao,
                    ViewModelType = typeof(ModuloAquisicaoViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE SENSORES",
                    MenuItemType = MenuItemType.ListaSensores,
                    ViewModelType = typeof(SensoresViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "ADICIONAR SENSOR",
                    MenuItemType = MenuItemType.AddSensor,
                    ViewModelType = typeof(SensorViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "LISTA DE UTILIZADORES",
                    MenuItemType = MenuItemType.ListaUsers,
                    ViewModelType = typeof(UsersViewModel),
                    IsEnabled = true
                });

                MenuItems.Add(new MenuItem
                {
                    Title = "ADICIONAR UTILIZADORES",
                    MenuItemType = MenuItemType.AddUser,
                    ViewModelType = typeof(UserViewModel),
                    IsEnabled = true
                });

            }

            if (Settings.Role.Equals("Func"))
            {

            }
        }

        private async void OnSelectItem(MenuItem item)
        {
            if (item.IsEnabled)
            {
                object parameter = null;

                await NavigationService.NavigateToAsync(item.ViewModelType, parameter);

            }
        }

        private async void OnLogout()
        {
            bool response = await _authenticationService.LogoutAsync();
            if(response==true){
                await NavigationService.NavigateToAsync<LoginViewModel>();     
            }
           
        }

        /*      private void OnBookingRequested(Booking booking)
              {
                  SetMenuItemStatus(MenuItemType.UpcomingRide, true);
                  SetMenuItemStatus(MenuItemType.ReportIncident, true);
              }

              private void OnBookingFinished(Booking booking)
              {
                  SetMenuItemStatus(MenuItemType.UpcomingRide, false);
              }
      */
        private void SetMenuItemStatus(MenuItemType type, bool enabled)
        {
            MenuItem logout = MenuItems.FirstOrDefault(m => m.MenuItemType == type);

            if (logout != null)
            {
                logout.IsEnabled = enabled;
            }
        }

    }
}