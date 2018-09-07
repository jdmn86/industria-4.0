using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;
using Industria4.Models.User;

namespace Industria4.DataServices.Fabricas
{
    public interface IFabricaService
    {
        Task<ObservableCollection<Fabrica>> GetFabricasAsync();

        Task<Fabrica> SaveFabricaAsync(Fabrica fabrica);

        Task<Fabrica> EditFabricaAsync(Fabrica fabrica);

        Task<User> AddUserToFab(int idFabrica, User user);

        Task<User> RemoveUserToFab(int idFabrica, User user);

    }
}
