using System;
namespace Industria4.Models.User
{
    public class AuthenticationResponse
    {

        public DateTime issued { get; set; }

        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string token_type { get; set; }

        public DateTime expires { get; set; }
    }
}
