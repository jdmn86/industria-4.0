using System;
namespace Industria4.Models.User
{
    public class AuthenticationRequest
    {
        public string username { get; set; }

        public string password { get; set; }

        public string grant_type { get; set; }
    }
}
