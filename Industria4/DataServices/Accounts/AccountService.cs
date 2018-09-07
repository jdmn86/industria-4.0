using System;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Services.Dialog;

namespace Industria4.DataServices.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IRequestProvider _requestProvider;
     //   private readonly IUserDialogs _userDialogs;


        public AccountService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
     //       _userDialogs = userDialogs;
        }

        public async Task<Role> GetRoleById(string IdUser)
        {
            Role roles = null;

            if (string.IsNullOrEmpty(Settings.AccessToken))
            {
                throw new ArgumentException("message", nameof(Settings.AccessToken));
            }

            try
            {
                roles = await _requestProvider.GetAsync<Role>("api/account/RoleFromUser/"+IdUser);
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


            return roles;

        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordBindingModel pass)
      {
            bool response;

            try
            {
                response =   await _requestProvider.PutAsync<ChangePasswordBindingModel,bool>("api/account/ChangePasswordBindingModel" ,pass);
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return false;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return false;
            }

            return response;

      } 

    }
}
