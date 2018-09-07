using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices;
using Industria4.DataServices.Base;
using Industria4.DataServices.Users;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.Dialog;
using Industria4.Validations;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        private ValidatableObject<string> _userName;
        private ValidatableObject<string> _password;

        private bool _isValid;
        private bool _isEnabled;
        private bool _isLogin;
        private User user;

        private readonly IAuthenticationService _authenticationService;
        private readonly IUsersService _usersService;
      //  private readonly IDialogService _dialogService;

        public LoginViewModel(IAuthenticationService authenticationService, IUsersService usersService )
        {
            _authenticationService = authenticationService;
            _usersService = usersService;
  //         _dialogService = dialogService;

            _userName = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            AddValidations();
        }

        public bool IsLogin
        {
            get
            {
                return _isLogin;
            }
            set
            {
                _isLogin = value;
                RaisePropertyChanged(() => IsLogin);
            }
        }

        public ValidatableObject<string> UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        public ValidatableObject<string> Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        public ICommand ValidateUserNameCommand => new Command(() => ValidateUserName());

        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());

        private bool ValidateUserName()
        {
            return _userName.Validate();
        }

        private bool ValidatePassword()
        {
            return _password.Validate();
        }

        public ICommand SignInCommand => new Command(SignInAsync);

        //    public ICommand GoToSignUpCommand => new Command(GoToSignUp);

  /*      public ICommand ValidateCommand
        {
            get { return new Command(() => Enable()); }
        }
*/
        public async void SignInAsync()
        {
            IsBusy = true;
       //     IsValid = true;
            IsLogin = true;
            bool isValid = Validate();
            bool isAuthenticated = false;

            if (isValid)
            {
                try
                {

                    isAuthenticated = await _authenticationService.LoginAsync(UserName.Value, Password.Value);
                }
                catch (ServiceAuthenticationException ex)
                {
                    await App.Current.MainPage.DisplayAlert("Verifique as credenciais. ","Erro na autenticação.", "Ok");
                    await NavigationService.NavigateToAsync<LoginViewModel>();      
            
                }
             

            }
            else
            {
               // IsValid = false;
                IsLogin = false;
            }

            if (isAuthenticated)
            {
                bool isDataOk = await GetUserDataAsync();

                Debug.WriteLine("esta autenticado  ");
                if (isDataOk)
                {
                    Debug.WriteLine("await NavigationService.NavigateToAsync<MainViewModel>(); ");
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }

            }

            IsBusy = false;
            Debug.WriteLine("SignInAsync fim");
        }

        private async Task<bool> GetUserDataAsync()
        {
            try
            {
                user = await _usersService.GetCurrentProfileAsync();

                Settings.Name = user.Nome;
                Settings.idUser = user.IdUser;
                Settings.Username = _userName.Value;
                Settings.Password = _password.Value;
                Settings.id = user.Id;

                //role
                Role role = await _usersService.GetCurrentRoleAsync();
                Settings.Role = role.Name;

                return true;
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
           //     await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching user data with exception: {ex}");
            }
            return false;
        }

        private bool Validate()
        {
            bool isValidUser = ValidateUserName();
            bool isValidPassword = ValidatePassword();

            return isValidUser && isValidPassword;
        }

 /*      private void Enable()
        {
            IsEnabled = !string.IsNullOrEmpty(UserName.Value) &&
                !string.IsNullOrEmpty(Password.Value);
        }
*/
        private void AddValidations()
        {
            _userName.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Username should not be empty" });
            _password.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Password should not be empty" });
        }

        public override Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}