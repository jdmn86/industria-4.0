using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.Dialog;

namespace Industria4.DataServices.Users
{
    public class UsersService : IUsersService
    {
        private readonly IRequestProvider _requestProvider;
 //       private readonly IUserDialogs _userDialogs;

        public UsersService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
 //           _userDialogs = userDialogs;
        }


        public Task<User> GetCurrentProfileAsync()
        {

            return _requestProvider.GetAsync<User>("api/users/me");
        }

        public Task<Role> GetCurrentRoleAsync()
        {
            return _requestProvider.GetAsync<Role>("api/account/RoleFromUser/"+Settings.idUser);
        }

        public async Task<ObservableCollection<User>> GetApiUserAsync()
        {
            ObservableCollection<User> users = null;


            try
            {
                users = await _requestProvider.GetAsync<ObservableCollection<User>>("api/users");
                Debug.WriteLine("count lista users : " + users.Count);
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return null;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return null;
            }

            return users;

        }

        public async Task<ObservableCollection<UserChat>> GetUsersChatAsync()
        {
            ObservableCollection<UserChat> users = null;

            if (string.IsNullOrEmpty(Settings.AccessToken))
            {
                throw new ArgumentException("message", nameof(Settings.AccessToken));
            }

            try
            {
                users = await _requestProvider.GetAsync<ObservableCollection<UserChat>>("api/users/GetUsersChat");
                Debug.WriteLine("count lista users : " + users.Count);
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return null;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return null;
            }
            return users;

        }

        public async Task<ObservableCollection<User>> GetUsersFromFabricaAsync(int id)
      {

        ObservableCollection<User> listUsers = null;

        if (string.IsNullOrEmpty(Settings.AccessToken))
        {
            throw new ArgumentException("message", nameof(Settings.AccessToken));
        }

        try
        {
            listUsers =  await _requestProvider.GetAsync<ObservableCollection<User>>( "api/users/fabrica/" + id.ToString());
        }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return null;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return null;
            }

        return listUsers;
       
      }

        public async Task<ObservableCollection<User>> GetUsersOutThisFabricaAsync(int id)
      {
            ObservableCollection<User> listUsers = null;

        
            try
            {
                listUsers = await _requestProvider.GetAsync<ObservableCollection<User>>( "api/users/fabrica/out/" + id.ToString());
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return null;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return null;
            }

            return listUsers;
      }

        public async Task<ObservableCollection<User>> GetUsersFromMaquinaAsync(int id)
      {
            ObservableCollection<User> listUsers = null;

         
            try
            {
                listUsers =  await _requestProvider.GetAsync<ObservableCollection<User>>("api/users/maquina/" + id.ToString());
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return null;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return null;
            }
            return listUsers;
         
      }

        public async Task<ObservableCollection<User>> GetUsersOutThisMaquinaAsync(int id)
      {
            ObservableCollection<User> listUsers = null;

            if (string.IsNullOrEmpty(Settings.AccessToken))
            {
                throw new ArgumentException("message", nameof(Settings.AccessToken));
            }

            try
            {
                listUsers =   await _requestProvider.GetAsync<ObservableCollection<User>>("api/users/maquina/out/" + id.ToString());
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return null;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return null;
            }

            return listUsers;

      } 

        public async Task<responseRegister> RegisterUserAsync(AspNetUser aspUser)
      {
           

            try
            {
                var response =   await _requestProvider.PostAsync<AspNetUser,responseRegister>("api/Account/Register",aspUser);
                return response;
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                aspUser = null;

//                await _userDialogs.AlertAsync(exception.Message.ToString(), "Error", "Ok");
            }

            return null;

      } 

        public async Task<User> saveUserAsync(User user)
      {
            
            try
            {
              var  response =   await _requestProvider.PostAsync<User>("api/users",user);
                return response;
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return null;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return null;
            }

            return null;

      } 

        public async Task<User> editUserAsync(User user)
      {
  
             var   response =   await _requestProvider.PutAsync<User>("api/users/" + user.Id,user);

            return response;
          

      } 


    /*    public async Task<User> blockUserAsync(User user)
      {

        var model = new User
            {
                Id = IdIn,
                IdUser = IdUserIn,
                NumeroFuncionario = NumeroFuncionarioIn,
                Nome = NomeIn,

            };

            if (string.IsNullOrEmpty(Settings.AccessToken))
            {
                throw new ArgumentException("message", nameof(Settings.AccessToken));
            }

            try
            {
                user =   await _requestProvider.PutAsync<User>("api/users/" + user.Id,user);
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                user = null;

                await _dialogService.ShowAlertAsync(exception.Message.ToString(), "Error", "Ok");
            }

            return user;

      } */
       
    }
}
