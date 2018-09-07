using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models.User;

namespace Industria4.DataServices
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IRequestProvider _requestProvider;

        public bool IsAuthenticated => !string.IsNullOrEmpty(Settings.AccessToken);

        public AuthenticationService(IRequestProvider requestProvider)
        { 
            Debug.WriteLine("AuthenticationService init with IRequestProvider" );
            _requestProvider = requestProvider;
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var auth = new AuthenticationRequest
            {
                username = userName,
                password = password,
                grant_type = "password"
            };

    //        UriBuilder builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
    //        builder.Path = "api/login";

    //        string uri = builder.ToString();

            try
            {
                AuthenticationResponse authenticationInfo = await _requestProvider.PostAsyncToken<AuthenticationRequest, AuthenticationResponse>("Token", auth);

                Settings.AccessToken = authenticationInfo.access_token;
                Settings.expires_on = DateTime.Now.AddSeconds(authenticationInfo.expires_in);
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


            return true;
        }

        public async Task<bool> LogoutAsync()
        {

            try
            {
                _requestProvider.PostAsync("logout");
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
             

            Settings.RemoveAll();

            return true;
        }

        public int GetCurrentUserId()
        {
            // return Settings.UserId;
            return 0;
        }

    }
}
