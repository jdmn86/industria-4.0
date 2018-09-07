using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.Fabricas;
using Industria4.DataServices.Maquinas;
using Industria4.DataServices.Users;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Validations;
using Industria4.ViewModels.Base;
using Plugin.Notifications;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class FabricaViewModel : ViewModelBase
    {
        private readonly IFabricaService _fabricaService;
        private readonly IUsersService _usersService;
        private readonly IMaquinaService _maquinaService;
   //     private readonly IUserDialogs _userDialogs;

        public FabricaViewModel(IFabricaService fabricaService, 
                                IUsersService usersService, 
                                IMaquinaService maquinaService)
                                //IUserDialogs userDialogs)
        {
            _fabricaService = fabricaService;
            _usersService = usersService;
            _maquinaService = maquinaService;
//            _userDialogs = userDialogs;

            BindingComandRemoveUserFab = new Command(async (e) => RemoveUserFab(e));
            GetMaquinaDetailsCommand = new Command<Maquina>(async (t) => GetMaquinaDetailsAsync(t));
            GetUserDetailsCommand = new Command<User>(async (item) => await GetUserDetailsAsync(item));
           
            ValidateCodigofabricaCommand  = new Command(() => ValidateCodigoFabrica());
            ValidateCodigoGatewayfabricaCommand = new Command(() => ValidateCodigoGateway());
            ValidateNomefabricaCommand = new Command(() => ValidateNomeFabrica());
            ValidateLocationfabricaCommand = new Command(() => ValidateNomeFabrica());
            ValidateTelefonefabricaCommand = new Command(() => ValidateTelefoneFabrica());
            ValidateIpGatewayfabricaCommand = new Command(() => ValidateIpGatewayFabrica());

            AddValidations();
        }

        public override async Task InitializeAsync(object navigationData)
        {

            if (Settings.Role.Equals("AdminLocal"))
            {
                visibleToAdminLocal = false;
                enableToAdminLocal = false;
            }

            if (navigationData is Fabrica)
            {
                Fabrica = navigationData as Fabrica;

                NewFabrica = false;
                EditFabrica = true;
                BtnShowAddUser = true;
             //   ExpandColunm = 1;

                UsersFabrica = await _usersService.GetUsersFromFabricaAsync(Fabrica.Id);

                MaquinasFabrica = await _maquinaService.GetMaquinasFromFabAsync(Fabrica.Id);

                UsersOutFabrica = await _usersService.GetUsersOutThisFabricaAsync(Fabrica.Id);

            }
            else
            {
                Fabrica = new Fabrica();

                NewFabrica = true;
                EditFabrica = false;
                BtnShowAddUser = false;
               // ExpandColunm = 3;
            }
        }


        #region Control Views definition
        private bool _IsValid = false;
        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; RaisePropertyChanged(() => IsValid); }
        }

        private bool _NewFabrica = true;
        public bool NewFabrica
        {
            get { return _NewFabrica; }
            set { _NewFabrica = value; RaisePropertyChanged(() => NewFabrica); }
        }

        private bool _EditFabrica = false;
        public bool EditFabrica
        {
            get { return _EditFabrica; }
            set { _EditFabrica = value; RaisePropertyChanged(() => EditFabrica); }
        }


        private bool _ShowAddUserBtn;
        public bool BtnShowAddUser
        {
            get { return _ShowAddUserBtn; }
            set { _ShowAddUserBtn = value; RaisePropertyChanged(() => BtnShowAddUser); }
        }

        private bool _viewAddUserToFabrica;
        public bool ViewAddUserToFabrica
        {
            get { return _viewAddUserToFabrica; }
            set { _viewAddUserToFabrica = value; RaisePropertyChanged(() => ViewAddUserToFabrica); }
        }

        private bool _visibleToAdminLocal = true;
        public bool visibleToAdminLocal
        {
            get
            {
                return _visibleToAdminLocal;
            }
            set
            {
                _visibleToAdminLocal = value;
                RaisePropertyChanged(() => visibleToAdminLocal);
            }
        }

        private bool _enableToAdminLocal = true;
        public bool enableToAdminLocal
        {
            get
            {
                return _enableToAdminLocal;
            }
            set
            {
                _enableToAdminLocal = value;
                RaisePropertyChanged(() => enableToAdminLocal);
            }
        }

        private User _selectedUserToAdd;
        public User SelectedUserToAdd
        {
            get { return _selectedUserToAdd; }
            set { _selectedUserToAdd = value; RaisePropertyChanged(() => SelectedUserToAdd); }
        }

        #endregion

        #region Model definition
        private Fabrica _Fabrica { get; set; }
        public Fabrica Fabrica
        {
            get { return _Fabrica; }
            set
            {
                _Fabrica = (Fabrica)value;

                CodigoFabrica.Value = value.CodigoFabrica;
                NomeFabrica.Value = value.NomeFabrica;
                Localizacao.Value = value.Localizacao;
                Telefone.Value = value.Telefone;
                IpGateway.Value = value.IpGateway;
                Active.Value = value.Active;
                CodigoGateway.Value = value.CodigoGateway;
                RaisePropertyChanged(() => Fabrica);

              
            }
        }


        private ValidatableObject<string> _CodigoFabrica = new ValidatableObject<string>();
        public ValidatableObject<string> CodigoFabrica
        {
            get
            {
                return _CodigoFabrica;
            }
            set
            {
                _CodigoFabrica = value;
                RaisePropertyChanged(() => CodigoFabrica);
            }
        }

        private ValidatableObject<string> _NomeFabrica = new ValidatableObject<string>();
        public ValidatableObject<string> NomeFabrica
        {
            get
            {
                return _NomeFabrica;
            }
            set
            {
                _NomeFabrica = value;
                RaisePropertyChanged(() => NomeFabrica);
            }
        }

        private ValidatableObject<string> _Localizacao = new ValidatableObject<string>();
        public ValidatableObject<string> Localizacao
        {
            get
            {
                return _Localizacao;
            }
            set
            {
                _Localizacao = value;
                RaisePropertyChanged(() => Localizacao);
            }
        }

        private ValidatableObject<string> _Telefone = new ValidatableObject<string>();
        public ValidatableObject<string> Telefone
        {
            get
            {
                return _Telefone;
            }
            set
            {
                _Telefone = value;
                RaisePropertyChanged(() => Telefone);
            }
        }

        private ValidatableObject<string> _IpGateway = new ValidatableObject<string>();
        public ValidatableObject<string> IpGateway
        {
            get
            {
                return _IpGateway;
            }
            set
            {
                _IpGateway = value;
                RaisePropertyChanged(() => IpGateway);
            }
        }

        private ValidatableObject<DateTime> _CreatedAt = new ValidatableObject<DateTime>();
        public ValidatableObject<DateTime> CreatedAt
        {
            get
            {
                return _CreatedAt;
            }
            set
            {
                _CreatedAt = value;
                RaisePropertyChanged(() => CreatedAt);
            }
        }

        private ValidatableObject<bool> _Active = new ValidatableObject<bool>();
        public ValidatableObject<bool> Active
        {
            get
            {
                return _Active;
            }
            set
            {
                _Active = value;
                RaisePropertyChanged(() => Active);
            }
        }

        private ValidatableObject<string> _CodigoGateway = new ValidatableObject<string>();
        public ValidatableObject<string> CodigoGateway
        {
            get
            {
                return _CodigoGateway;
            }
            set
            {
                _CodigoGateway = value;
                RaisePropertyChanged(() => CodigoGateway);
            }
        }
        #endregion

        #region Data definition


        private ObservableCollection<Maquina> _maquinas = new ObservableCollection<Maquina>();
        public ObservableCollection<Maquina> MaquinasFabrica
        {
            get { return _maquinas; }
            set
            {
                _maquinas = value;
                RaisePropertyChanged(() => MaquinasFabrica);
            }
        }

        private ObservableCollection<User> _users = new ObservableCollection<User>();
        public ObservableCollection<User> UsersFabrica
        {
            get { return _users; }
            set
            {
                _users = value;
                RaisePropertyChanged(() => UsersFabrica);
            }
        }

        private ObservableCollection<User> _usersOutThisFab = new ObservableCollection<User>();
        public ObservableCollection<User> UsersOutFabrica
        {
            get
            {
                return _usersOutThisFab;
            }
            set
            {
                _usersOutThisFab = value;
                RaisePropertyChanged(() => UsersOutFabrica);
            }
        }

        private ListView _listaUserss;
        public ListView ListaUserss
        {
            get
            {
                return _listaUserss;
            }
            set
            {
                _listaUserss = value;
                RaisePropertyChanged(() => ListaUserss);
            }
        }

        #endregion

        #region Commands definition


        public ICommand BindingComandRemoveUserFab { get; private set; }
        public async void RemoveUserFab(object e)
        {
/*            bool result = await _userDialogs.ConfirmAsync("Tem a certeza que pretende Remover?", "Remover Utilizador");
            if (result == false)
            {
                Debug.WriteLine("result : " + result);
                return;
            }
*/
            var userARemover = (e as User);
            await _fabricaService.RemoveUserToFab(Fabrica.Id, userARemover);
            UsersFabrica.Clear();
            UsersFabrica = await _usersService.GetUsersFromFabricaAsync(Fabrica.Id);
            UsersOutFabrica.Clear();
            UsersOutFabrica = await _usersService.GetUsersOutThisFabricaAsync(Fabrica.Id);
        }


        public ICommand SaveFabrica
        {
            get
            {
                return new Command(async () =>
                {
                    IsValid = Validate();

                    if (IsValid)
                    {

                        Fabrica fabr;

                        bool result = await App.Current.MainPage.DisplayAlert("Tem a certeza que pretende Guardar?", "Guardar Fabrica", "Ok","Cancelar");
                        if (result == false)
                        {
                            Debug.WriteLine("result : " + result);
                            return;
                        }

                        if (NewFabrica == true)
                        {
                            fabr = new Fabrica
                            {
                                Id = Fabrica.Id,
                                CodigoFabrica = CodigoFabrica.Value,
                                NomeFabrica = NomeFabrica.Value,
                                Localizacao = Localizacao.Value,
                                Telefone = Telefone.Value,
                                IpGateway = IpGateway.Value,
                                //   CreatedAt = DateTime.Now,
                                Active = Active.Value,
                                CodigoGateway = CodigoGateway.Value

                            };
                            var response = await _fabricaService.EditFabricaAsync(fabr);

                            if (response != null)
                            {
                                await NavigationService.NavigateToAsync<ListaFabricasViewModel>();
                            }
                        }
                        else
                        {
                            fabr = new Fabrica
                            {
                                Id = 0,
                                CodigoFabrica = CodigoFabrica.Value,
                                NomeFabrica = NomeFabrica.Value,
                                Localizacao = Localizacao.Value,
                                Telefone = Telefone.Value,
                                IpGateway = IpGateway.Value,
                                CreatedAt = DateTime.Now,
                                Active = Active.Value,
                                CodigoGateway = CodigoGateway.Value

                            };
                            var response = await _fabricaService.SaveFabricaAsync(fabr);

                            if (response != null)
                            {
                                await NavigationService.NavigateToAsync<ListaFabricasViewModel>();
                            }
                        }
                    }
                });
            }
        }

        public ICommand ButtonShowAddUser
        {
            get
            {
                return new Command(async () =>
                {
                    BtnShowAddUser = false;
                    ViewAddUserToFabrica = true;
                });
            }
        }

        public ICommand AddUserToFab
        {
            get
            {
                return new Command(async () =>
                {
                    BtnShowAddUser = true;
                    ViewAddUserToFabrica = false;
                    await _fabricaService.AddUserToFab(Fabrica.Id, SelectedUserToAdd);
                    UsersFabrica.Clear();
                    UsersFabrica = await _usersService.GetUsersFromFabricaAsync(Fabrica.Id);
                    UsersOutFabrica.Clear();
                    UsersOutFabrica = await _usersService.GetUsersOutThisFabricaAsync(Fabrica.Id);
                });
            }
        }

        public ICommand GetMaquinaDetailsCommand { get; private set; }
        private async Task GetMaquinaDetailsAsync(Maquina maq)
        {

            await NavigationService.NavigateToAsync<MaquinaViewModel>(maq);

        }

        public ICommand GetUserDetailsCommand { get; private set; }
        private async Task GetUserDetailsAsync(User user)
        {

            await NavigationService.NavigateToAsync<UserViewModel>(user);

        }

        private bool Validate()
        {
            bool isValidCodigoFabrica = ValidateCodigoFabrica();
            bool isValidNomeFabrica = ValidateNomeFabrica();
            bool isValidCodigoGateway = ValidateCodigoGateway();
            bool isValidTelefone = ValidateTelefoneFabrica();
            bool isValidIpGateway = ValidateIpGatewayFabrica();

            return isValidCodigoFabrica && isValidNomeFabrica && isValidCodigoGateway && isValidTelefone &&isValidIpGateway;
        }

        private void AddValidations()
        {
            _CodigoFabrica.Validations.Add(new IsBetweenValueOfByteRule<string> { ValidationMessage = "Codigo Fabrica should by between 0-255" });
            _CodigoFabrica.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Codigo Fabrica should not by empty" });
            _CodigoGateway.Validations.Add(new IsBetweenValueOfByteRule<string> { ValidationMessage = "Codigo Gateway should by between 0-255" });
            _CodigoGateway.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Codigo Gateway should not by empty" });
            _NomeFabrica.Validations.Add(new IsBetweenValueOfVarChar250Rule<string> { ValidationMessage = "Nome Fabrica should by between 0-250 caracteres" });
            _NomeFabrica.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Nome Fabrica should not by empty" });
            _Localizacao.Validations.Add(new IsBetweenValueOfVarChar250Rule<string> { ValidationMessage = "Localização da Fabrica should not by empty" });
            _Localizacao.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Localização should not by empty" });
            _Telefone.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Telefone da Fabrica should not by empty" });
            _Telefone.Validations.Add(new IsValueOfTelefone9Rule<string> { ValidationMessage = "Numero Telefone da Fabrica should have 9 digits" });
            _IpGateway.Validations.Add(new IsBetweenValueOfVarChar250Rule<string> { ValidationMessage = "Localização da Fabrica should not by empty" });
            _IpGateway.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Localização should not by empty" });
        }

        public ICommand ValidateCodigofabricaCommand { get; private set; }
        private bool ValidateCodigoFabrica()
        {
            return _CodigoFabrica.Validate();
        }

        public ICommand ValidateCodigoGatewayfabricaCommand { get; private set; }
        private bool ValidateCodigoGateway()
        {
            return _CodigoGateway.Validate();
        }

        public ICommand ValidateNomefabricaCommand { get; private set; }
        private bool ValidateNomeFabrica()
        {
            return _NomeFabrica.Validate();
        }

        public ICommand ValidateLocationfabricaCommand { get; private set; }
        private bool ValidateLocationFabrica()
        {
            return _Localizacao.Validate();
        }

        public ICommand ValidateTelefonefabricaCommand { get; private set; }
        private bool ValidateTelefoneFabrica()
        {
            return _Telefone.Validate();
        }

        public ICommand ValidateIpGatewayfabricaCommand { get; private set; }
        private bool ValidateIpGatewayFabrica()
        {
            return _IpGateway.Validate();
        }


        #endregion

    }
}