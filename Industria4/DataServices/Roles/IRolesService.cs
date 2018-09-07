using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;

namespace Industria4.DataServices.Roles
{
    public interface IRolesService
    {
        Task<ObservableCollection<Role>> GetRolesAsync();
    }
}
