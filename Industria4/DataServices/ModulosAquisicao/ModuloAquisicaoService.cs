using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Services.Dialog;
using Industria4.Models;

namespace Industria4.DataServices.ModulosAquisicao
{
    public class ModuloAquisicaoService : IModuloAquisicaoService
    {
        private readonly IRequestProvider _requestProvider;
    //    private readonly IUserDialogs _userDialogs;

        public ModuloAquisicaoService(IRequestProvider requestProvider )
        {
            _requestProvider = requestProvider;
 //           _userDialogs = userDialogs;
        }


        public async Task<ObservableCollection<ModuloAquisicao>> GetModulosAquisicaoAsync()
        {
            ObservableCollection<ModuloAquisicao> modulosAquisicao = null;

         

            try
            {
                modulosAquisicao = await _requestProvider.GetAsync<ObservableCollection<ModuloAquisicao>>("api/modulosaquisicao");
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

            return modulosAquisicao;

        }

        public async Task<ObservableCollection<ModuloAquisicao>> GetModulosAquisicaoFromMaquinaAsync(int idMaquina)
        {
            ObservableCollection<ModuloAquisicao> modulosAquisicao = null;

            try
            {
                modulosAquisicao = await _requestProvider.GetAsync<ObservableCollection<ModuloAquisicao>>("api/modulosaquisicao/maquina/"+idMaquina);
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

            return modulosAquisicao;

        }

        public async Task<ModuloAquisicao> SaveModulosAquisicaoAsync(ModuloAquisicao moduloAquisicao)
        {
            ModuloAquisicao result;
       
          

            try
            {
                result = await _requestProvider.PostAsync("api/modulosaquisicao",moduloAquisicao);
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
            return result;

        }

        public async Task<ModuloAquisicao> EditModulosAquisicaoAsync(ModuloAquisicao moduloAquisicao)
        {
       

            ModuloAquisicao result;

            if (string.IsNullOrEmpty(Settings.AccessToken))
            {
                throw new ArgumentException("message", nameof(Settings.AccessToken));
            }

            try
            {
                result = await _requestProvider.PutAsync("api/modulosaquisicao/"+moduloAquisicao.Id , moduloAquisicao);
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

            return result;


        }
    }
}
