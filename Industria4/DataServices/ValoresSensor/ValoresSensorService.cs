using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.Dialog;
using Newtonsoft.Json;
using Syncfusion.SfChart.XForms;

namespace Industria4.DataServices.DataSensores
{
    public class ValoresSensorService : IValoresSensorService
    {
        private readonly IRequestProvider _requestProvider;
//        private readonly IUserDialogs _userDialogs;


        public ValoresSensorService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
 //           _userDialogs = userDialogs;

        }


        public async Task<ObservableCollection<ValoresSensores>> GetValoresSensorFromSensorAsync(int idSensor, string DateInit, string TimeInit, string DateEnd, string TimeEnd )
        {
            ObservableCollection<ValoresSensores> valoresSensor = null;

          
            try
            {
                valoresSensor = await _requestProvider.GetAsync<ObservableCollection<ValoresSensores>>("api/ValoresSensores/"+ idSensor+"/"+DateInit+"/"+TimeInit+"/"+DateEnd+"/"+TimeEnd);
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


            return valoresSensor;
        }


        public async Task<ObservableCollection<ValoresSensoresContagem>> GetValoresSensorFromSensorContagemAsync(int idSensor, string DateInit, string DateEnd)
        {
            ObservableCollection<ValoresSensoresContagem> valoresSensor = null;

            try
            {
                valoresSensor = await _requestProvider.GetAsync<ObservableCollection<ValoresSensoresContagem>>("api/ValoresSensores/contagem/" + idSensor + "/" + DateInit + "/" + DateEnd );
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


            return valoresSensor;
        }

        public async Task<decimal> GetTotalFromSensorContagemAsync(int idSensor)
        {
            decimal total = 0;

            try
            {
                total = await _requestProvider.GetAsync<decimal>("api/ValoresSensores/contagem/lastreset/" + idSensor );
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return 0;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return 0;
            }


            return total;
        }

        public async Task<Sensor> ResetSensorContagemAsync(int idSensor, Sensor sensor)
        {

           
            var result = await _requestProvider.PutAsync("api/ValoresSensores/contagem/reset/" + idSensor,sensor);
       

            return result;
        }

     
    }
}
