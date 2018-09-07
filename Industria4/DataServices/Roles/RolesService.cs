using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Services.Dialog;

namespace Industria4.DataServices.Roles
{
    public class RolesService : IRolesService
    {
        private readonly IRequestProvider _requestProvider;
//        private readonly IUserDialogs _userDialogs;

        public RolesService(IRequestProvider requestProvider )
        {
            _requestProvider = requestProvider;
 //           _userDialogs = userDialogs;
        }

        public async Task<ObservableCollection<Role>> GetRolesAsync()
        {
            ObservableCollection<Role> roles = null;

         
            try
            {
                roles = await _requestProvider.GetAsync<ObservableCollection<Role>>("api/role");
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

    }
}
