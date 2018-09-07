using System;
using System.Threading.Tasks;
using Industria4.Models;

namespace Industria4.DataServices.Accounts
{
    public interface IAccountService
    {
        Task<Role> GetRoleById(string IdUser);
        Task<bool> ChangePasswordAsync(ChangePasswordBindingModel pass);
    }
}
