using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Services.Dialog;

namespace Industria4.DataServices.Connections
{
    public class ConnectionService : IConnectionService
    {
        private readonly IRequestProvider _requestProvider;
     //   private readonly IUserDialogs _userDialogs;

        public ConnectionService(IRequestProvider requestProvider )
        {
            _requestProvider = requestProvider;
       //     _userDialogs = userDialogs;
        }

        public async Task<ObservableCollection<ConnectionSignalR>> GetConnectionsAsync()
        {
            ObservableCollection<ConnectionSignalR> conns = null;

            if (string.IsNullOrEmpty(Settings.AccessToken))
            {
                throw new ArgumentException("message", nameof(Settings.AccessToken));
            }

            try
            {
                conns = await _requestProvider.GetAsync<ObservableCollection<ConnectionSignalR>>("api/Connections");
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                conns = null;

 //               await _userDialogs.AlertAsync(exception.Message.ToString(), "Error", "Ok");
            }

            return conns;
        }

        public async Task<ConnectionSignalR> SaveConnectionsAsync(ConnectionSignalR conn)
        {

            var result = await _requestProvider.PostAsync("api/Connections", conn);

            return result;

        }

        public async Task<ConnectionSignalR> EditConnectionsAsync(ConnectionSignalR conn)
        {

            var result = await _requestProvider.PutAsync("api/Connections/" + conn.ConnectionId, conn);

            return result;

        }

        public async Task<ConnectionSignalR> DeleteMessages(ConnectionSignalR conn)
     {
            var result = await _requestProvider.DeleteAsync<ConnectionSignalR>("api/Connections/" + conn.ConnectionId.ToString() );

            return result;
 
     } 

    }
}
