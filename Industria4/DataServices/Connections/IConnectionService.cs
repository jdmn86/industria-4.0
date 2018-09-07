using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;

namespace Industria4.DataServices.Connections
{
    public interface IConnectionService
    {
        Task<ObservableCollection<ConnectionSignalR>> GetConnectionsAsync();
        Task<ConnectionSignalR> SaveConnectionsAsync(ConnectionSignalR conn);
        Task<ConnectionSignalR> EditConnectionsAsync(ConnectionSignalR conn);
        Task<ConnectionSignalR> DeleteMessages(ConnectionSignalR conn);

    }
}
