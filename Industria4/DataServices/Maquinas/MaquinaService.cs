using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.Dialog;

namespace Industria4.DataServices.Maquinas
{
    public class MaquinaService :IMaquinaService
    {   
        private readonly IRequestProvider _requestProvider;
  //      private readonly IUserDialogs _userDialog;

        public MaquinaService(IRequestProvider requestProvider )
        {
            _requestProvider = requestProvider;
   //         _userDialog = userDialog;
        }

        public async Task<ObservableCollection<Maquina>> GetMaquinasFromFabAsync(int id)
        {
            ObservableCollection<Maquina> maquinas = null;

            try
            {
                maquinas = await _requestProvider.GetAsync<ObservableCollection<Maquina>>("api/maquinas/fabrica/" + id.ToString());
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

            return maquinas;
        }

        public async Task<Maquina> SaveMaquinaAsync(Maquina maq)
        {
            var result = await _requestProvider.PostAsync("api/maquinas", maq);
            Debug.WriteLine("Save maquina");
            return result;

        }

        public async Task<Maquina> EditMaquinaAsync(Maquina maq)
        {
            var result = await _requestProvider.PutAsync("api/maquinas/" + maq.Id, maq);

            return result;

        } 

        public async Task<ObservableCollection<Maquina>> GetMaquinasAsync()
        {
            ObservableCollection<Maquina> maquinas = null;

          
            try
            {
            maquinas = await _requestProvider.GetAsync<ObservableCollection<Maquina>>("api/maquinas");
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

            return maquinas;

        }


        public async Task<ObservableCollection<Maquina>> GetMaquinasFromUserAsync(string idUser)
        {
            ObservableCollection<Maquina> maquinas = null;

            if (string.IsNullOrEmpty(Settings.AccessToken))
            {
                throw new ArgumentException("message", nameof(Settings.AccessToken));
            }
            try
            {
                maquinas = await _requestProvider.GetAsync<ObservableCollection<Maquina>>("api/maquinas/user/" + idUser.ToString());
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

            return maquinas;

        }

        public async Task<User> AddUserToMaq(int idMaq, User user)
        {

            var result = await _requestProvider.PutAsync("api/maquinas/" + idMaq.ToString() + "/user", user);

            return result;

        }

        public async Task<User> RemoveUserToMaq(int idMaquina, User user)
        {
            var result = await _requestProvider.PutAsync("api/maquinas/" + idMaquina.ToString() + "/user/remove", user);

            return result;

        } 
    }
}
