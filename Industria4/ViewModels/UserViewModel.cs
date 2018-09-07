using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.Accounts;
using Industria4.DataServices.Roles;
using Industria4.DataServices.Users;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Validations;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    
    public class UserViewModel : ViewModelBase
    {
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;
        private readonly IAccountService _accountService;


        public UserViewModel(IUsersService usersService, IRolesService rolesService, IAccountService accountService)
        {
            _usersService = usersService;
            _rolesService = rolesService;
            _accountService = accountService;

            ValidateNomeCommand = new Command(() => ValidateNome());
            ValidateNumeroFuncionarioCommand = new Command(() => ValidateNumeroFuncionario());
            ValidateEmailCommand = new Command(() => ValidateEmail());

            AddValidations();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            Roles = new ObservableCollection<Role>(await _rolesService.GetRolesAsync());

            if (navigationData is User)
            {
                user = navigationData as User;

                Role = await _accountService.GetRoleById(user.IdUser);

                foreach (var item in Roles)
                {
                    if (item.Name.Equals(Role.Name))
                    {
                        SelectedRole = item;

                    }
                }

                IsSaving = false;
                if (Settings.idUser.Equals(user.IdUser))
                {
                    ButtonPassword = true;
                }

                IsMudarPassword = false;

            }
            else
            {
                user = new User();
                user.AspNetUser = new AspNetUser();

                IsSaving = true;
                IsMudarPassword = false;
                ButtonPassword = false;

            }
        }

        #region Control Views definition

        private bool _IsValid = false;
        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; RaisePropertyChanged(() => IsValid); }
        }


        public bool _isEnable { get; private set; }
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                _isEnable = value;
                RaisePropertyChanged(() => IsEnable);
            }
        }

        public bool _isSaving;
        public bool IsSaving
        {
            get { return _isSaving; }
            set
            {
                _isSaving = value;
                RaisePropertyChanged(() => IsSaving);
            }
        }

        public bool _isMudarPassword;
        public bool IsMudarPassword
        {
            get { return _isMudarPassword; }
            set
            {
                _isMudarPassword = value;
                RaisePropertyChanged(() => IsMudarPassword);
            }
        }

        public bool _buttonPassword;
        public bool ButtonPassword
        {
            get { return _buttonPassword; }
            set
            {
                _buttonPassword = value;
                RaisePropertyChanged(() => ButtonPassword);
            }
        }


        #endregion

        #region Model definition
        public User _user;
        public User user
        {
            get { return _user; }
            set
            {
                _user = (User)value;
                _user.AspNetUser = AspNetUser;
                Nome.Value = value.Nome;
                NumeroFuncionario.Value = value.NumeroFuncionario.ToString();

                RaisePropertyChanged(() => user);

                //   RaisePropertyChanged(() => AspNetUser);
            }
        }

        public AspNetUser _aspNetUser;
        public AspNetUser AspNetUser
        {
            get { return _aspNetUser; }
            set
            {
                _aspNetUser = (AspNetUser)value;
                Email.Value = value.Email;
                UserName.Value = value.UserName;
                /*     RaisePropertyChanged(() => Email);
                    UserName = value.UserName;
                      RaisePropertyChanged(() => UserName);
    */
                RaisePropertyChanged(() => AspNetUser);
                //    user.AspNetUser = (AspNetUser)value;
                //     RaisePropertyChanged(() => user);
            }
        }

        public ValidatableObject<string> _Email = new ValidatableObject<string>();
        public ValidatableObject<string> Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                RaisePropertyChanged(() => Email);

            }
        }

        public ValidatableObject<string> _Nome = new ValidatableObject<string>();
        public ValidatableObject<string> Nome
        {
            get { return _Nome; }
            set
            {
                _Nome = value;
                RaisePropertyChanged(() => Nome);

            }
        }

        public ValidatableObject<string> _NumeroFuncionario = new ValidatableObject<string>();
        public ValidatableObject<string> NumeroFuncionario
        {
            get { return _NumeroFuncionario; }
            set
            {
                _NumeroFuncionario = value;
                RaisePropertyChanged(() => NumeroFuncionario);

            }
        }

        public ValidatableObject<string> _UserName = new ValidatableObject<string>();
        public ValidatableObject<string> UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        public Role _Role { get; set; }
        public Role Role
        {
            get { return _Role; }
            set
            {
                _Role = value;
                RaisePropertyChanged(() => Role);

            }
        }


        public string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public string _confirmpassword;
        public string ConfirmPassword
        {
            get { return _confirmpassword; }
            set
            {
                _confirmpassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        public string _oldPassword { get; private set; }
        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                _oldPassword = value;
                RaisePropertyChanged(() => OldPassword);
            }
        }

       
       
        #endregion


        #region Data definition

        private ObservableCollection<Role> _roles { get; set; }
        public ObservableCollection<Role> Roles
        {
            get
            {
                return _roles;
            }
            set
            {
                _roles = value;
                RaisePropertyChanged(() => Roles);
            }
        }

        public Role _SelectedRole { get; private set; }
        public Role SelectedRole
        {
            get { return _SelectedRole; }
            set
            {
                //Method which notifies the property change
                _SelectedRole = value;
                RaisePropertyChanged(() => SelectedRole);
            }
        }

        #endregion

        #region Commands definition

        public ICommand MudarPasswordCommand
        {
            get
            {
                return new Command(async () =>
                {
                    ButtonPassword = false;
                    IsMudarPassword = true;

                });
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new Command(async ( ) =>
                {
                    IsValid = Validate();

                    if (IsValid)
                    {

                        AspNetUser aspNetUserAux;

                        if (IsSaving == true)
                        {
                            //registo AspNetUser

                            aspNetUserAux = new AspNetUser
                            {

                                Email = Email.Value,
                                Password = Password,
                                ConfirmPassword = ConfirmPassword,
                                RoleUser = SelectedRole.Name,
                                UserName = user.AspNetUser.UserName

                            };
                            responseRegister response = await _usersService.RegisterUserAsync(aspNetUserAux);
                            //     Settings.Username = Username;
                            //    Settings.Password = Password;

                            var modelUser = new User
                            {
                                IdUser = response.IdUser,
                                NumeroFuncionario = int.Parse( NumeroFuncionario.Value),
                                Nome = Nome.Value,

                            };

                            var responseSaveUser = await _usersService.saveUserAsync(modelUser);

                            if (responseSaveUser == null)
                            {
                                Debug.WriteLine("erro");
                            }
                            await NavigationService.NavigateToAsync<UsersViewModel>();

                        }
                        else
                        {
                            if (IsMudarPassword == false)
                            {
                                var model = new User
                                {
                                    Id = _user.Id,
                                    IdUser = _user.IdUser,
                                    NumeroFuncionario = _user.NumeroFuncionario,
                                    Nome = _user.Nome

                                };
                                User aux = await _usersService.editUserAsync(model);

                                if (aux != null)
                                {
                                    await NavigationService.NavigateToAsync<UsersViewModel>();

                                }
                            }
                        }
                        if (IsMudarPassword == true)
                        {
                            var pass = new ChangePasswordBindingModel
                            {
                                OldPassword = OldPassword,
                                NewPassword = Password,
                                ConfirmPassword = ConfirmPassword

                            };
                            await _accountService.ChangePasswordAsync(pass);
                            await NavigationService.NavigateToAsync<UsersViewModel>();
                        }
                    }
                });
            }
        }

        private bool Validate()
        {
            bool isValidNome = ValidateNome();
            bool isValidNumeroFuncionario = ValidateNumeroFuncionario();
            bool isValidEmail = ValidateEmail();

            return isValidNome && isValidNumeroFuncionario && isValidEmail ;
        }

        private void AddValidations()
        {
            _Nome.Validations.Add(new IsBetweenValueOfVarChar250Rule<string> { ValidationMessage = "Nome Utilizador should by between less 250 caracteres" });
            _Nome.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Nome Utilizador should not by empty" });
            _NumeroFuncionario.Validations.Add(new IsBetweenValueOfVarChar250Rule<string> { ValidationMessage = "Numero Funcionario should less 250 caracteres caracteres" });
            _NumeroFuncionario.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Numero Funcionario should not by empty" });
            _Email.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Email should not by empty" });
            _Email.Validations.Add(new EmailRule<string> { ValidationMessage = "Email should not by empty" });
        }


        public ICommand ValidateNomeCommand { get; private set; }
        private bool ValidateNome()
        {
            return _Nome.Validate();
        }

        public ICommand ValidateNumeroFuncionarioCommand { get; private set; }
        private bool ValidateNumeroFuncionario()
        {
            return _NumeroFuncionario.Validate();
        }

        public ICommand ValidateEmailCommand { get; private set; }
        private bool ValidateEmail()
        {
            return _Email.Validate();
        }

    }
}
#endregion