using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Services.Dialog;

namespace Industria4.DataServices.TipoPorta
{
    public class TiposPortaService : ITiposPortaService
    {
        private readonly IRequestProvider _requestProvider;
 //       private readonly IUserDialogs _userDialogs;

        public TiposPortaService(IRequestProvider requestProvider )
        {
            _requestProvider = requestProvider;
//            _userDialogs = userDialogs;
        }

        public async Task<ObservableCollection<TiposPorta>> GetTiposPortaAsync()
        {
            ObservableCollection<TiposPorta> tipos = null;

           
            try
            {
                tipos = await _requestProvider.GetAsync<ObservableCollection<TiposPorta>>("api/TiposPorta");
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

            return tipos;

        }


 /*       public async Task<TiposPorta> SaveSensorAsync(TiposPorta tipo)
        {

            var result = await _requestProvider.PostAsync("api/sensores", sensor);

            return result;
        }

        public async Task<TiposPorta> EditSensorAsync(TiposPorta tipo)
        {

            var result = await _requestProvider.PutAsync("api/sensores/" + sensor.Id, sensor);

            return result;

        }
        */
    }
}
