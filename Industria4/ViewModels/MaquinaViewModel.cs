using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.Fabricas;
using Industria4.DataServices.Maquinas;
using Industria4.DataServices.ModulosAquisicao;
using Industria4.DataServices.Users;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.Dialog;
using Industria4.Validations;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class MaquinaViewModel : ViewModelBase
    {
        private readonly IFabricaService _fabricaService;
        private readonly IUsersService _usersService;
        private readonly IMaquinaService _maquinaService;
        private readonly IModuloAquisicaoService _modulosAquisicaoService;
     //   private readonly IDialogService _dialogService;

        public MaquinaViewModel(IFabricaService fabricaService,
                        IModuloAquisicaoService modulosAquisicaoService,
                                IMaquinaService maquinaService, IUsersService usersService)
                             //   IDialogService dialogService)
        {
            _fabricaService = fabricaService;
            _usersService = usersService;
      //      _dialogService = dialogService;
            _maquinaService = maquinaService;
            _modulosAquisicaoService = modulosAquisicaoService;


            BindingComandRemoveUserMaq = new Command(async (e) => RemoveUserMaq(e));
            GetUserDetailsCommand = new Command<User>(async (item) => await GetUserDetailsAsync(item));
            GetModuloAquisicaoDetailsCommand = new Command<ModuloAquisicao>(async (item) => await GetModuloAquisicaoDetailsAsync(item));

            ValidateCodigoMaquinaCommand = new Command(() => ValidateCodigoMaquina());
            ValidateNomeMaquinaCommand = new Command(() => ValidateNomeMaquina());

            AddValidations();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            Fabricas = await _fabricaService.GetFabricasAsync();

            if (navigationData is Maquina)
            {
                Maquina = navigationData as Maquina;

                NewMaquina = false;
                EditMaquina = true;
                IsAddUser = true;

                foreach (var item in Fabricas)
                {
                    if (item.Id.Equals(Maquina.IdFabrica))
                    {
                        SelectedItem = item;
                    }
                }

                UsersOutThisFab = await _usersService.GetUsersOutThisFabricaAsync(Maquina.IdFabrica);

                ModulosAquisicao = await _modulosAquisicaoService.GetModulosAquisicaoFromMaquinaAsync(Maquina.Id);

            }  
            else
            {
                 Maquina = new Maquina();

                NewMaquina = true;
                EditMaquina = false;
                IsAddUser = false;
            }

            //  await base.InitializeAsync(navigationData);
        }

        #region Control Views definition

        private bool _IsValid = false;
        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; RaisePropertyChanged(() => IsValid); }
        }

        public bool _NewMaquina = true;
        public bool NewMaquina
        {
            get { return _NewMaquina; }
            set
            {
                _NewMaquina = value;
                RaisePropertyChanged(() => NewMaquina);
            }
        }

        public bool _EditMaquina = false;
        public bool EditMaquina
        {
            get { return _EditMaquina; }
            set
            {
                _EditMaquina = value;
                RaisePropertyChanged(() => EditMaquina);
            }
        }

        private bool _isAddUser;
        public bool IsAddUser
        {
            get { return _isAddUser; }
            set
            {
                _isAddUser = value;
                RaisePropertyChanged(() => IsAddUser);
            }
        }

        private bool _viewAddUserToMaquina;
        public bool ViewAddUserToMaquina
        {
            get { return _viewAddUserToMaquina; }
            set
            {
                _viewAddUserToMaquina = value;
                RaisePropertyChanged(() => ViewAddUserToMaquina);
            }
        }


        #endregion

        #region Model definition

        private Maquina _Maquina { get; set; }
        public Maquina Maquina
        {
            get { return _Maquina; }
            set
            {
                _Maquina = (Maquina)value;

                CodigoMaquina.Value = value.CodigoMaquina;
                NomeMaquina.Value = value.NomeMaquina;
                IdFabrica = value.IdFabrica;
                CreatedAt = value.CreatedAt;
                Active = value.Active;

                RaisePropertyChanged(() => Maquina);


            }
        }

        private ValidatableObject<string> _CodigoMaquina = new ValidatableObject<string>();
        public ValidatableObject<string> CodigoMaquina
        {
            get
            {
                return _CodigoMaquina;
            }
            set
            {
                _CodigoMaquina = value;
                RaisePropertyChanged(() => CodigoMaquina);
            }
        }

        private ValidatableObject<string> _NomeMaquina = new ValidatableObject<string>();
        public ValidatableObject<string> NomeMaquina
        {
            get
            {
                return _NomeMaquina;
            }
            set
            {
                _NomeMaquina = value;
                RaisePropertyChanged(() => NomeMaquina);
            }
        }

        private int _IdFabrica;
        public int IdFabrica
        {
            get
            {
                return _IdFabrica;
            }
            set
            {
                _IdFabrica = value;
                RaisePropertyChanged(() => IdFabrica);
            }
        }

        private DateTime _CreatedAt { get; set; }
        public DateTime CreatedAt
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

        private bool _Active { get; set; }
        public bool Active
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



        #endregion

        #region Data definition

        private ObservableCollection<Fabrica> _fabrica = new ObservableCollection<Fabrica>();
        public ObservableCollection<Fabrica> Fabricas
        {
            get { return _fabrica; }
            set
            {
                _fabrica = value;
                RaisePropertyChanged(() => Fabricas);
            }
        }

        public Fabrica _selectedItem { get; private set; }
        public Fabrica SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        private ObservableCollection<User> _users = new ObservableCollection<User>();
        public ObservableCollection<User> ListUsers
        {
            get { return _users; }
            set
            {
                _users = value;
                RaisePropertyChanged(() => ListUsers);
            }
        }

        private ObservableCollection<User> _usersOutThisFab = new ObservableCollection<User>();
        public ObservableCollection<User> UsersOutThisFab
        {
            get
            {
                return _usersOutThisFab;
            }
            set
            {
                _usersOutThisFab = value;
                RaisePropertyChanged(() => UsersOutThisFab);
            }
        }


        public User _selectedUserToAdd { get; private set; }
        public User SelectedUserToAdd
        {
            get { return _selectedUserToAdd; }
            set
            {
                _selectedUserToAdd = value;
                RaisePropertyChanged(() => SelectedUserToAdd);
            }
        }

        public ObservableCollection<ModuloAquisicao> _modulosAquisicao = new ObservableCollection<ModuloAquisicao>();
        public ObservableCollection<ModuloAquisicao> ModulosAquisicao
        {
            get { return _modulosAquisicao; }
            set
            {
                _modulosAquisicao = value;
                RaisePropertyChanged(() => ModulosAquisicao);
            }
        }


        #endregion

        #region Commands definition


        public ICommand BindingComandRemoveUserMaq { get; private set; }
        public async void RemoveUserMaq(object e)
        {
 /*           bool result = await _userDialogs.ConfirmAsync("Tem a certeza que pretende Remover?", "Remover Utilizador");
            if (result == false)
            {
                Debug.WriteLine("result : " + result);
                return;
            }
*/
            var userARemover = (e as User);
            await _maquinaService.RemoveUserToMaq(Maquina.Id, userARemover);
            ListUsers.Clear();
            ListUsers = await _usersService.GetUsersFromMaquinaAsync(Maquina.Id);
            UsersOutThisFab.Add(userARemover);
        }

        public ICommand AddUserToMaq
        {
            get
            {
                return new Command(async () =>
                {
                    IsAddUser = true;
                    ViewAddUserToMaquina = false;
                    await _maquinaService.AddUserToMaq(Maquina.Id, SelectedUserToAdd);
                    ListUsers.Clear();
                    ListUsers = await _usersService.GetUsersFromMaquinaAsync(Maquina.Id);
                    UsersOutThisFab.Remove(SelectedUserToAdd);
                });
            }
        }



        public ICommand ButtonShowAddUser
        {
            get
            {
                return new Command(async () =>
                {
                    IsAddUser = false;
                    ViewAddUserToMaquina = true;
                });
            }
        }

        public ICommand SaveMaquina
        {
            get
            {
                return new Command(async () =>
                {

                    IsValid = Validate();

                    if (IsValid)
                    {
                        Maquina maquina;

                        bool result = await App.Current.MainPage.DisplayAlert("Tem a certeza que pretende Guardar?", "Guardar Maquina", "Sim", "Cancel");
                        if (result == false)
                        {
                            Debug.WriteLine("result : " + result);
                            return;
                        }

                        if (NewMaquina == true)
                        {

                            maquina = new Maquina
                            {
                      
                                CodigoMaquina = CodigoMaquina.Value,
                                NomeMaquina = NomeMaquina.Value,
                                IdFabrica = SelectedItem.Id,
                                CreatedAt = DateTime.Now,
                                Active = Active
                            };

                            var response = await _maquinaService.SaveMaquinaAsync(maquina);

                            if (response != null)
                            {
                                await NavigationService.NavigateToAsync<MaquinasViewModel>();

                            }
                        }
                        else
                        {

                            maquina = new Maquina
                            {
                                Id = Maquina.Id,
                                CodigoMaquina = CodigoMaquina.Value,
                                NomeMaquina = NomeMaquina.Value,
                                IdFabrica = SelectedItem.Id,
                              //  CreatedAt = DateTime.Now,
                                Active = Active
                            };

                            var response = await _maquinaService.EditMaquinaAsync(maquina);

                            if (response != null)
                            {
                                await NavigationService.NavigateToAsync<MaquinasViewModel>();

                            }

                        }
                    }
                });
            }
        }

   //     public ICommand GetModuloAquisicaoDetailsCommand => new Command<ModuloAquisicao>(async (item) => await GetModuloAquisicaoDetailsAsync(item));

        public ICommand GetModuloAquisicaoDetailsCommand { get; private set; }
        private async Task GetModuloAquisicaoDetailsAsync(ModuloAquisicao modulo)
        {
            await NavigationService.NavigateToAsync<ModuloAquisicaoViewModel>(modulo);
        }

    //    public ICommand GetUserDetailsCommand => new Command<User>(async (item) => await GetUserDetailsAsync(item));
        public ICommand GetUserDetailsCommand { get; private set; }
        private async Task GetUserDetailsAsync(User user)
        {

            await NavigationService.NavigateToAsync<UserViewModel>(user);

        }

        private bool Validate()
        {
            bool isValidCodigoMaquina = ValidateCodigoMaquina();
            bool isValidNomeMaquina = ValidateNomeMaquina();
           
            return isValidCodigoMaquina && isValidNomeMaquina;
        }

        private void AddValidations()
        {
            _CodigoMaquina.Validations.Add(new IsBetweenValueOfByteRule<string> { ValidationMessage = "Codigo Maquina should by between 0-255" });
            _CodigoMaquina.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Codigo Maquina should not by empty" });
            _NomeMaquina.Validations.Add(new IsBetweenValueOfVarChar250Rule<string> { ValidationMessage = "Nome Maquina should by between 0-250 caracteres" });
            _NomeMaquina.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Nome Maquina should not by empty" });

        }

        public ICommand ValidateCodigoMaquinaCommand { get; private set; }
        private bool ValidateCodigoMaquina()
        {
            return _CodigoMaquina.Validate();
        }

        public ICommand ValidateNomeMaquinaCommand { get; private set; }
        private bool ValidateNomeMaquina()
        {
            return _NomeMaquina.Validate();
        }


        #endregion


    }
}
