using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.Dialog;

namespace Industria4.DataServices.Fabricas
{
    public class FabricaService: IFabricaService
    {
        private readonly IRequestProvider _requestProvider;
  //      private readonly IUserDialogs _userDialogs;

        public FabricaService(IRequestProvider requestProvider )
        {
            _requestProvider = requestProvider;
   //         _userDialogs = userDialogs;
        }

        public async Task<ObservableCollection<Fabrica>> GetFabricasAsync()
        {
            ObservableCollection<Fabrica> fabricas = null;


            try
            {
                fabricas = await _requestProvider.GetAsync<ObservableCollection<Fabrica>>("api/fabricas");
            } catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return null;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return null;
            }

            return fabricas;

        }

     


        public async Task<Fabrica> SaveFabricaAsync(Fabrica fabrica)
        {
            
            var result = await _requestProvider.PostAsync("api/fabricas", fabrica);



            return result;

        }


        public async Task<Fabrica> EditFabricaAsync(Fabrica fabrica)
     {

            var result = await _requestProvider.PutAsync("api/fabricas/" + fabrica.Id, fabrica);

            return result;

     }


      public async Task<User> AddUserToFab(int idFabrica, User user)
     {

            var result = await _requestProvider.PutAsync("api/fabricas/" + idFabrica.ToString() + "/user", user);

            return result;

     }

        public async Task<User> RemoveUserToFab(int idFabrica, User user)
     {
            var result = await _requestProvider.PutAsync("api/fabricas/" + idFabrica.ToString() + "/user/remove", user);

            return result;
 
     } 

    }
}
