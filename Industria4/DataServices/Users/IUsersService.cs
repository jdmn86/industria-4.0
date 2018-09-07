using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;
using Industria4.Models.User;

namespace Industria4.DataServices.Users
{
    public interface IUsersService
    {
         Task<User> GetCurrentProfileAsync();
		Task<Role> GetCurrentRoleAsync();
        Task<ObservableCollection<User>> GetUsersFromFabricaAsync(int id);
        Task<ObservableCollection<User>> GetUsersOutThisFabricaAsync(int id);
        Task<ObservableCollection<User>> GetApiUserAsync();
        Task<ObservableCollection<User>> GetUsersFromMaquinaAsync(int id);
        Task<ObservableCollection<User>> GetUsersOutThisMaquinaAsync(int id);
        // Task<bool> blockUserAsync(User user);
        Task<ObservableCollection<UserChat>> GetUsersChatAsync();
        Task<User> editUserAsync(User user);
        Task<User> saveUserAsync(User user);
    //    Task<User> EditRegisterUserAsync(AspNetUser aspUser);
        Task<responseRegister> RegisterUserAsync(AspNetUser aspUser);
	}
}
