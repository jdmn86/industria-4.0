using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;
using Industria4.Models.User;

namespace Industria4.DataServices.Maquinas
{
    public interface IMaquinaService
    {
        Task<ObservableCollection<Maquina>> GetMaquinasFromFabAsync(int id);
        Task<ObservableCollection<Maquina>> GetMaquinasAsync();
        Task<User> AddUserToMaq(int idMaq, User user);
        Task<User> RemoveUserToMaq(int idMaquina, User user);
        Task<Maquina> SaveMaquinaAsync(Maquina maq);
        Task<Maquina> EditMaquinaAsync(Maquina maq);
        Task<ObservableCollection<Maquina>> GetMaquinasFromUserAsync(string idUser);
    }
}
