using System;
using System.Threading.Tasks;

namespace Industria4.DataServices
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated { get; }

        Task<bool> LoginAsync(string userName, string password);

        Task<bool> LogoutAsync();

        int GetCurrentUserId();
    }
}
