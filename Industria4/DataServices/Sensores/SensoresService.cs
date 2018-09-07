using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Services.Dialog;

namespace Industria4.DataServices.Sensores
{
    public class SensoresService : ISensoresService
    {
        private readonly IRequestProvider _requestProvider;
//        private readonly IUserDialogs _userDialogs;

        public SensoresService(IRequestProvider requestProvider )
        {
            _requestProvider = requestProvider;
 //           _userDialogs = userDialogs;
        }

        public async Task<ObservableCollection<Sensor>> GetSensoresAsync()
        {
            ObservableCollection<Sensor> sensores = null;

          
            try
            {
                sensores = await _requestProvider.GetAsync<ObservableCollection<Sensor>>("api/sensores");
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

            return sensores;

        }

        public async Task<ObservableCollection<Sensor>> GetSensoresFromMaquinaAsync(int idMaquina)
        {
            ObservableCollection<Sensor> sensores = null;

            try
            {
                sensores = await _requestProvider.GetAsync<ObservableCollection<Sensor>>("api/sensores/maquina/"+idMaquina);
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

            return sensores;

        }

        public async Task<ObservableCollection<Sensor>> GetSensoresFromModuloAsync(int idModulo)
        {
            ObservableCollection<Sensor> sensores = null;


            try
            {
                sensores = await _requestProvider.GetAsync<ObservableCollection<Sensor>>("api/sensores/modulo/" + idModulo.ToString());
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

            return sensores;

        }

        public async Task<Sensor> SaveSensorAsync(Sensor sensor)
        {

            var result = await _requestProvider.PostAsync("api/sensores", sensor);

            return result;
        }

        public async Task<Sensor> EditSensorAsync(Sensor sensor)
        {
    
            var result = await _requestProvider.PutAsync("api/sensores/" + sensor.Id, sensor);

            return result;

        }

    }
}
